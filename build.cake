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

var windowsRepackAssemblies = new List<FilePath>
{
	windowsBuildDirectory + File("Foundations.dll"),
	windowsBuildDirectory + File("Foundations.HttpClient.dll"),
	windowsBuildDirectory + File("Material.Portable.dll")
};
var formsRepackAssemblies = new List<FilePath>
{
	formsBuildDirectory + File("Foundations.dll"),
	formsBuildDirectory + File("Foundations.HttpClient.dll"),
	formsBuildDirectory + File("Material.Portable.dll")
};
var iOSRepackAssemblies = new List<FilePath>
{
	iOSBuildDirectory + File("Foundations.dll"),
	iOSBuildDirectory + File("Foundations.HttpClient.dll"),
	iOSBuildDirectory + File("Material.Portable.dll"),
	iOSBuildDirectory + File("Robotics.Mobile.Core.dll"),
	iOSBuildDirectory + File("Robotics.Mobile.Core.iOS.dll")
};
var androidRepackAssemblies = new List<FilePath>
{
	androidBuildDirectory + File("Foundations.dll"),
	androidBuildDirectory + File("Foundations.HttpClient.dll"),
	androidBuildDirectory + File("Material.Portable.dll"),
	androidBuildDirectory + File("Robotics.Mobile.Core.dll"),
	androidBuildDirectory + File("Robotics.Mobile.Core.Droid.dll")
};

var ilRepackItems = new List<Tuple<FilePath, FilePath, List<FilePath>>>
{
	new Tuple<FilePath, FilePath, List<FilePath>>(
		windowsBuildDirectory + File(mergedAssembly),
		windowsBuildDirectory + File("Material.Windows.dll"),
		windowsRepackAssemblies),
	new Tuple<FilePath, FilePath, List<FilePath>>(
		iOSBuildDirectory + File(mergedAssembly),
		iOSBuildDirectory + File("Material.iOS.dll"),
		iOSRepackAssemblies),
	new Tuple<FilePath, FilePath, List<FilePath>>(
		androidBuildDirectory + File(mergedAssembly),
		androidBuildDirectory + File("Material.Android.dll"),
		androidRepackAssemblies),
	new Tuple<FilePath, FilePath, List<FilePath>>(
		formsBuildDirectory + File(mergedAssembly),
		formsBuildDirectory + File("Material.Forms.dll"),
		formsRepackAssemblies),
};

var ilRepackFrameworkLocations = new List<FilePath> 
{
	File("C:/Program Files (x86)/Reference Assemblies/Microsoft/Framework/MonoAndroid/v6.0"),
	File("C:/Program Files (x86)/Reference Assemblies/Microsoft/Framework/Xamarin.iOS/v1.0")
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

Task("ILRepack")
     .IsDependentOn("Build")
     .Does(() =>
{
 	foreach(var ilRepackItem in ilRepackItems)
	{
		var settings = new ILRepackSettings
		{
			TargetKind = TargetKind.Dll,
			XmlDocs = true,
			NDebug = false,
			Libs = ilRepackFrameworkLocations
		};

		ILRepack(
			ilRepackItem.Item1, 
			ilRepackItem.Item2, 
			ilRepackItem.Item3,
			settings);
	}
});

Task("NuGet-Pack")
    .IsDependentOn("ILRepack")
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
