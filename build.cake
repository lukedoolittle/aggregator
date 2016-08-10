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

var nugetLibDirectory = Directory(nugetLocation) + Directory("lib");
var windowsLibDirectory = nugetLibDirectory + Directory("net452");
var iOSLibDirectory = nugetLibDirectory + Directory("Xamarin.iOS10");
var androidLibDirectory = nugetLibDirectory + Directory("MonoAndroid60");

var nugetItemDictionary = new Dictionary<ConvertableDirectoryPath, ConvertableDirectoryPath>
{
	{windowsBuildDirectory, windowsLibDirectory},
	{iOSBuildDirectory, iOSLibDirectory},
	{androidBuildDirectory, androidLibDirectory}
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
	foreach(var nugetItem in nugetItemDictionary)
	{
		var assemblyFiles = new List<FilePath> 
		{
			nugetItem.Key + File("Foundations.Cryptography.dll"),
			nugetItem.Key + File("Foundations.dll"),
			nugetItem.Key + File("Foundations.Http.dll"),
			nugetItem.Key + File("Foundations.HttpClient.dll")
			nugetItem.Key + File("Foundations.Serialization.dll"),
			nugetItem.Key + File("Material.Portable.dll"),
			nugetItem.Key + File("Material.dll"),
			nugetItem.Key + File("Monkey.Robotics.dll"),
		};

		foreach(var file in assemblyFiles)
		{
			CopyFileToDirectory(file, nugetItem.Value);
		}
	}
});

// TODO: get this to work with Xamarin assemblies
// Task("Assembly-Merge")
//     .IsDependentOn("Build")
//     .Does(() =>
// {
// 	foreach(var nugetItem in nugetItemDictionary)
// 	{
// 		var assemblyFiles = new List<FilePath> 
// 		{
// 			nugetItem.Key + File("Foundations.Cryptography.dll"),
// 			nugetItem.Key + File("Foundations.dll"),
// 			nugetItem.Key + File("Foundations.Http.dll"),
// 			nugetItem.Key + File("Foundations.Serialization.dll"),
// 			nugetItem.Key + File("Material.Portable.dll"),
// 		};

// 		ILMerge(
// 			nugetItem.Value + File(mergedAssembly), 
// 			nugetItem.Key + File(primaryAssembly), 
// 			assemblyFiles);
// 	}
// });

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
