#tool "nuget:?package=ilmerge"
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var solution = Argument("solution", "Quantfabric.sln");
var primaryAssembly = Argument("primaryAssembly", "Material.dll");
var mergedAssembly = Argument("mergedAssembly", primaryAssembly);

var nugetLocation = Argument("nugetLocation", "./build/Material");

var nuspec = Argument("nuspec", "Quantfabric.Material.nuspec");
var nupkg = Argument("nupkg", nugetLocation);

var key = Argument("key", "");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////
var windowsBuildDirectory = Directory("./src/Material.Windows/bin") + Directory(configuration);
var iOSBuildDirectory = Directory("./src/Material.iOS/bin") + Directory(configuration);
var androidBuildDirectory = Directory("./src/Material.Android/bin") + Directory(configuration);

var windowsLibList = new List<FilePath> 
{
	windowsBuildDirectory + File("Foundations.Cryptography.dll"),
	windowsBuildDirectory + File("Foundations.dll"),
	windowsBuildDirectory + File("Foundations.Http.dll"),
	windowsBuildDirectory + File("Foundations.HttpClient.dll"),
	windowsBuildDirectory + File("Material.Portable.dll"),
	windowsBuildDirectory + File("Material.dll")
};
var iOSLibList = new List<FilePath> 
{
	iOSBuildDirectory + File("Foundations.Cryptography.dll"),
	iOSBuildDirectory + File("Foundations.dll"),
	iOSBuildDirectory + File("Foundations.Http.dll"),
	iOSBuildDirectory + File("Foundations.HttpClient.dll"),
	iOSBuildDirectory + File("Material.Portable.dll"),
	iOSBuildDirectory + File("Robotics.Mobile.Core.dll"),
	iOSBuildDirectory + File("Robotics.Mobile.Core.iOS.dll"),
	iOSBuildDirectory + File("Material.dll"),
};
var androidLibList = new List<FilePath> 
{
	androidBuildDirectory + File("Foundations.Cryptography.dll"),
	androidBuildDirectory + File("Foundations.dll"),
	androidBuildDirectory + File("Foundations.Http.dll"),
	androidBuildDirectory + File("Foundations.HttpClient.dll"),
	androidBuildDirectory + File("Material.Portable.dll"),
	androidBuildDirectory + File("Robotics.Mobile.Core.dll"),
	androidBuildDirectory + File("Robotics.Mobile.Core.Droid.dll"),
	androidBuildDirectory + File("Material.dll")
};

var nugetLibDirectory = Directory(nugetLocation) + Directory("lib");
var windowsLibDirectory = nugetLibDirectory + Directory("net452");
var iOSLibDirectory = nugetLibDirectory + Directory("Xamarin.iOS10");
var androidLibDirectory = nugetLibDirectory + Directory("MonoAndroid60");

var ilMergeItems = new List<Tuple<ConvertableDirectoryPath, ConvertableDirectoryPath, string, ConvertableDirectoryPath, List<FilePath>>>
{
	new Tuple<ConvertableDirectoryPath, ConvertableDirectoryPath, string, ConvertableDirectoryPath, List<FilePath>>(
		windowsBuildDirectory,
		windowsLibDirectory,
		null,
		null,
		windowsLibList),
	new Tuple<ConvertableDirectoryPath, ConvertableDirectoryPath, string, ConvertableDirectoryPath, List<FilePath>>(
		iOSBuildDirectory,
		iOSLibDirectory,
		"v1",
		Directory("C:/Program Files (x86)/Reference Assemblies/Microsoft/Framework/Xamarin.iOS/v1.0"),
		iOSLibList),
	new Tuple<ConvertableDirectoryPath, ConvertableDirectoryPath, string, ConvertableDirectoryPath, List<FilePath>>(
		androidBuildDirectory,
		androidLibDirectory,
		"v6",
		Directory("C:/Program Files (x86)/Reference Assemblies/Microsoft/Framework/MonoAndroid/v6.0"),
		androidLibList)
};

var nugetDirectories = new List<DirectoryPath>
{
	nugetLibDirectory,
	Directory(nugetLocation) + Directory("tools"),
	Directory(nugetLocation) + Directory("content"),
	windowsLibDirectory,
	androidLibDirectory,
	iOSLibDirectory,
};

var nuspecFile = Directory(nugetLocation) + File(nuspec);
var nupkgFilePattern = nugetLocation + "/*.nupkg";

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(windowsBuildDirectory);
	CleanDirectory(iOSBuildDirectory);
	CleanDirectory(androidBuildDirectory);
	CleanDirectory(nugetLibDirectory);
	
	DeleteFiles(nupkgFilePattern);
});

Task("Create-Directories")
    .IsDependentOn("Clean")
    .Does(() =>
{
	foreach(var directory in nugetDirectories)
	{
		EnsureDirectoryExists(directory);
	}
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Create-Directories")
    .Does(() =>
{
    NuGetRestore(solution);
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    if(IsRunningOnWindows())
    {
      // Use MSBuild
      MSBuild(solution, settings =>
        settings.SetConfiguration(configuration));
    }
    else
    {
      // Use XBuild
      XBuild(solution, settings =>
        settings.SetConfiguration(configuration));
    }
});

Task("Move")
    .IsDependentOn("Build")
    .Does(() =>
{
	foreach(var item in ilMergeItems)
	{
		foreach(var file in item.Item5)
		{
			CopyFileToDirectory(file, item.Item2);
		}
	}
});

// TODO: get this to work with Xamarin assemblies
/*Task("Assembly-Merge")
     .IsDependentOn("Build")
     .Does(() =>
{
 	foreach(var ilMergeItem in ilMergeItems)
	{
		var settings = new ILMergeSettings
		{
			TargetKind = TargetKind.Dll
		};

		if (ilMergeItem.Item4 != null)
		{
			var targetPlatform = new TargetPlatform(
				TargetPlatformVersion.v1, 
				ilMergeItem.Item4);
			settings.TargetPlatform = targetPlatform;
		}

		ILMerge(
			ilMergeItem.Item2 + File(mergedAssembly), 
			ilMergeItem.Item1 + File(primaryAssembly), 
			ilMergeItem.Item5,
			settings);
	}
});*/

Task("NuGet-Pack")
    .IsDependentOn("Move")
    .Does(() =>
{
	NuGetPack(
		nuspecFile, 
		new NuGetPackSettings
		{
			OutputDirectory = nupkg
		});
});

Task("NuGet-Publish")
	.IsDependentOn("NuGet-Pack")
	.WithCriteria(() => HasArgument("key"))
    .Does(() =>
{
	var nupkgFiles = GetFiles(nupkgFilePattern);
	
	foreach(var nupkgFile in nupkgFiles)
	{
		NuGetPush(
			nupkgFile, 
			new NuGetPushSettings 
			{
				Source = "https://www.nuget.org",
				ApiKey = key
			});
	}
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("NuGet-Publish");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
