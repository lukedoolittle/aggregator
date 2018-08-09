#addin "Cake.Xamarin"

#tool "nuget:?package=ILRepack"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var solution = Argument("solution", "Quantfabric.sln");
var mergedAssembly = Argument("mergedAssembly", "Material.Core.dll");

var nugetLocation = Argument("nugetLocation", "./nuget/Material");
var nuspec = Argument("nuspec", "Quantfabric.Material.nuspec");

var key = Argument("key", "");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////
var nugetLibDirectory = Directory(nugetLocation) + Directory("lib");
var nugetToolsDirectory = Directory(nugetLocation) + Directory("tools");
var nugetContentDirectory = Directory(nugetLocation) + Directory("content");

var windowsBuildDirectory = Directory("./src/Material.Windows/bin") + Directory(configuration);
var iOSBuildDirectory = Directory("./src/Material.iOS/bin") + Directory(configuration);
var androidBuildDirectory = Directory("./src/Material.Android/bin") + Directory(configuration);
var uwpBuildDirectory = Directory("./src/Material.UWP/bin") + Directory(configuration);
var formsBuildDirectory = Directory("./src/Material.Forms/bin") + Directory(configuration);

var windowsLibDirectory = nugetLibDirectory + Directory("net45");
var iOSLibDirectory = nugetLibDirectory + Directory("Xamarin.iOS10");
var androidLibDirectory = nugetLibDirectory + Directory("MonoAndroid60");
var uwpLibDirectory = nugetLibDirectory + Directory("uap10.0");
var formsLibDirectory = nugetLibDirectory + Directory("portable45-net45+win8+wpa81");
var coreLibDirectory = nugetLibDirectory + Directory("netcoreapp");

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
	CleanDirectory(uwpBuildDirectory);
	CleanDirectory(formsBuildDirectory);

	CleanDirectory(nugetLibDirectory);
	
	DeleteFiles(nupkgFilePattern);
});

Task("Create-Directories")
    .IsDependentOn("Clean")
    .Does(() =>
{
	EnsureDirectoryExists(nugetLibDirectory);
	EnsureDirectoryExists(nugetToolsDirectory);
	EnsureDirectoryExists(nugetContentDirectory);
	EnsureDirectoryExists(windowsLibDirectory);
	EnsureDirectoryExists(androidLibDirectory);
	EnsureDirectoryExists(iOSLibDirectory);
	EnsureDirectoryExists(uwpLibDirectory);
	EnsureDirectoryExists(formsLibDirectory);
	EnsureDirectoryExists(coreLibDirectory);
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
    MSBuild(solution, settings =>
	  settings
		.SetConfiguration(configuration)
		.SetPlatformTarget(PlatformTarget.MSIL));
});

Task("NuGet-Pack")
    .IsDependentOn("Build")
    .Does(() =>
{
	NuGetPack(
		nuspecFile, 
		new NuGetPackSettings
		{
			OutputDirectory = nugetLocation
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
