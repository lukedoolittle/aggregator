#tool "nuget:?package=ILRepack"
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var solution = Argument("solution", "Quantfabric.sln");
var primaryAssembly = Argument("primaryAssembly", "Material.dll");
var mergedAssembly = Argument("mergedAssembly", "Material.Core.dll");

var nugetLocation = Argument("nugetLocation", "./build/Material");

var nuspec = Argument("nuspec", "Quantfabric.Material.nuspec");
var nupkg = Argument("nupkg", nugetLocation);

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

var windowsLibDirectory = nugetLibDirectory + Directory("net452");
var iOSLibDirectory = nugetLibDirectory + Directory("Xamarin.iOS10");
var androidLibDirectory = nugetLibDirectory + Directory("MonoAndroid60");
var uwpLibDirectory = nugetLibDirectory + Directory("uap10.0");

var baseMergeList = new List<FilePath>
{
	File("Foundations.Cryptography.dll"),
	File("Foundations.dll"),
	File("Foundations.HttpClient.dll"),
	File("Material.Portable.dll")
};

var windowsMergeList = new List<FilePath>(baseMergeList);
windowsMergeList.Add(File("Foundations.Http.dll"));

var iOSMergeList = new List<FilePath>(baseMergeList);
iOSMergeList.Add(File("Robotics.Mobile.Core.dll"));
iOSMergeList.Add(File("Robotics.Mobile.Core.iOS.dll"));

var androidMergeList = new List<FilePath>(baseMergeList);
androidMergeList.Add(File("Robotics.Mobile.Core.dll"));
androidMergeList.Add(File("Robotics.Mobile.Core.Droid.dll"));

//var uwpMergeList = new List<FilePath>(baseMergeList);

var ilRepackItems = new List<Tuple<ConvertableDirectoryPath, List<FilePath>>>
{
	new Tuple<ConvertableDirectoryPath, List<FilePath>>(
		windowsBuildDirectory,
		windowsMergeList),
	new Tuple<ConvertableDirectoryPath, List<FilePath>>(
		iOSBuildDirectory,
		iOSMergeList),
	new Tuple<ConvertableDirectoryPath, List<FilePath>>(
		androidBuildDirectory,
		androidMergeList),
		/*
	new Tuple<ConvertableDirectoryPath, List<FilePath>>(
		uwpBuildDirectory,
		uwpMergeList)
		*/
};

var ilRepackFrameworkLocations = new List<FilePath> 
{
	File("C:/Program Files (x86)/Reference Assemblies/Microsoft/Framework/MonoAndroid/v6.0")
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

		var assemblyPaths = ilRepackItem.Item2.Select(i => (FilePath)(ilRepackItem.Item1 + i));

		ILRepack(
			ilRepackItem.Item1 + File(mergedAssembly), 
			ilRepackItem.Item1 + File(primaryAssembly), 
			assemblyPaths,
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
