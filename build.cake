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
var uwpBuildDirectory = Directory("./src/Material.UWP/bin") + Directory(configuration);

var windowsLibList = new List<FilePath> 
{
	windowsBuildDirectory + File("Foundations.Cryptography.dll"),
	windowsBuildDirectory + File("Foundations.Cryptography.pdb"),
	windowsBuildDirectory + File("Foundations.Cryptography.xml"),
	windowsBuildDirectory + File("Foundations.dll"),
	windowsBuildDirectory + File("Foundations.pdb"),
	windowsBuildDirectory + File("Foundations.xml"),
	windowsBuildDirectory + File("Foundations.Http.dll"),
	windowsBuildDirectory + File("Foundations.Http.pdb"),
	windowsBuildDirectory + File("Foundations.Http.xml"),
	windowsBuildDirectory + File("Foundations.HttpClient.dll"),
	windowsBuildDirectory + File("Foundations.HttpClient.pdb"),
	windowsBuildDirectory + File("Foundations.HttpClient.xml"),
	windowsBuildDirectory + File("Material.Portable.dll"),
	windowsBuildDirectory + File("Material.Portable.pdb"),
	windowsBuildDirectory + File("Material.Portable.xml"),
	windowsBuildDirectory + File("Material.dll"),
	windowsBuildDirectory + File("Material.pdb"),
	windowsBuildDirectory + File("Material.xml")
};
var iOSLibList = new List<FilePath> 
{
	iOSBuildDirectory + File("Foundations.Cryptography.dll"),
	iOSBuildDirectory + File("Foundations.Cryptography.pdb"),
	iOSBuildDirectory + File("Foundations.Cryptography.xml"),
	iOSBuildDirectory + File("Foundations.dll"),
	iOSBuildDirectory + File("Foundations.pdb"),
	iOSBuildDirectory + File("Foundations.xml"),
	iOSBuildDirectory + File("Foundations.HttpClient.dll"),
	iOSBuildDirectory + File("Foundations.HttpClient.pdb"),
	iOSBuildDirectory + File("Foundations.HttpClient.xml"),
	iOSBuildDirectory + File("Material.Portable.dll"),
	iOSBuildDirectory + File("Material.Portable.pdb"),
	iOSBuildDirectory + File("Material.Portable.xml"),
	iOSBuildDirectory + File("Material.dll"),
	iOSBuildDirectory + File("Material.pdb"),
	iOSBuildDirectory + File("Material.xml"),
	iOSBuildDirectory + File("Robotics.Mobile.Core.dll"),
	iOSBuildDirectory + File("Robotics.Mobile.Core.iOS.dll")
};
var androidLibList = new List<FilePath> 
{
	androidBuildDirectory + File("Foundations.Cryptography.dll"),
	androidBuildDirectory + File("Foundations.Cryptography.pdb"),
	androidBuildDirectory + File("Foundations.Cryptography.xml"),
	androidBuildDirectory + File("Foundations.dll"),
	androidBuildDirectory + File("Foundations.dll"),
	androidBuildDirectory + File("Foundations.pdb"),
	androidBuildDirectory + File("Foundations.HttpClient.dll"),
	androidBuildDirectory + File("Foundations.HttpClient.pdb"),
	androidBuildDirectory + File("Foundations.HttpClient.xml"),
	androidBuildDirectory + File("Material.Portable.dll"),
	androidBuildDirectory + File("Material.Portable.pdb"),
	androidBuildDirectory + File("Material.Portable.xml"),
	androidBuildDirectory + File("Material.dll"),
	androidBuildDirectory + File("Material.pdb"),
	androidBuildDirectory + File("Material.xml"),
	androidBuildDirectory + File("Robotics.Mobile.Core.dll"),
	androidBuildDirectory + File("Robotics.Mobile.Core.Droid.dll")
};
var uwpLibList = new List<FilePath> 
{
	uwpBuildDirectory + File("Foundations.Cryptography.dll"),
	uwpBuildDirectory + File("Foundations.Cryptography.pdb"),
	uwpBuildDirectory + File("Foundations.Cryptography.xml"),
	uwpBuildDirectory + File("Foundations.dll"),
	uwpBuildDirectory + File("Foundations.dll"),
	uwpBuildDirectory + File("Foundations.pdb"),
	uwpBuildDirectory + File("Foundations.HttpClient.dll"),
	uwpBuildDirectory + File("Foundations.HttpClient.pdb"),
	uwpBuildDirectory + File("Foundations.HttpClient.xml"),
	uwpBuildDirectory + File("Material.Portable.dll"),
	uwpBuildDirectory + File("Material.Portable.pdb"),
	uwpBuildDirectory + File("Material.Portable.xml"),
	uwpBuildDirectory + File("Material.dll"),
	uwpBuildDirectory + File("Material.pdb"),
	uwpBuildDirectory + File("Material.xml")
};

var nugetLibDirectory = Directory(nugetLocation) + Directory("lib");

var windowsLibDirectory = nugetLibDirectory + Directory("net452");
var iOSLibDirectory = nugetLibDirectory + Directory("Xamarin.iOS10");
var androidLibDirectory = nugetLibDirectory + Directory("MonoAndroid60");
var uwpLibDirectory = nugetLibDirectory + Directory("uap10.0");

var uwpXAMLDirectory = uwpBuildDirectory + Directory("Material");
var uwpLibXAMLDirectory = uwpLibDirectory + Directory("Material");

var nugetItems = new List<Tuple<ConvertableDirectoryPath, List<FilePath>>>
{
	new Tuple<ConvertableDirectoryPath, List<FilePath>>(
		windowsLibDirectory,
		windowsLibList),
	new Tuple<ConvertableDirectoryPath, List<FilePath>>(
		iOSLibDirectory,
		iOSLibList),
	new Tuple<ConvertableDirectoryPath, List<FilePath>>(
		androidLibDirectory,
		androidLibList),
	new Tuple<ConvertableDirectoryPath, List<FilePath>>(
		uwpLibDirectory,
		uwpLibList)
};

var nugetDirectories = new List<DirectoryPath>
{
	nugetLibDirectory,
	Directory(nugetLocation) + Directory("tools"),
	Directory(nugetLocation) + Directory("content"),
	windowsLibDirectory,
	androidLibDirectory,
	iOSLibDirectory,
	uwpLibDirectory
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
        settings
			.SetConfiguration(configuration)
			.SetPlatformTarget(PlatformTarget.MSIL));
    }
    else
    {
      // Use XBuild
      XBuild(solution, settings =>
        settings			
			.SetConfiguration(configuration)
			.WithTarget("Any CPU"));
    }
});

Task("Move")
    .IsDependentOn("Build")
    .Does(() =>
{
	foreach(var nugetItem in nugetItems)
	{
		foreach(var file in nugetItem.Item2)
		{
			CopyFileToDirectory(file, nugetItem.Item1);
		}
	}

	CopyDirectory(uwpXAMLDirectory, uwpLibXAMLDirectory);
});

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
