[![NuGet version](https://img.shields.io/nuget/v/CADBooster.SolidDNA)](https://www.nuget.org/packages/CADBooster.SolidDna)
[![NuGet downloads](https://img.shields.io/nuget/dt/CADBooster.SolidDNA?logo=nuget&label=Downloads&color=yellow)](https://www.nuget.org/packages/CADBooster.SolidDna)
![MIT license](https://img.shields.io/badge/License-MIT-green)

# SolidDNA

![SolidDNA logo](https://raw.githubusercontent.com/CAD-Booster/SolidDNA/master/Images/logo.png)

## A user-friendly framework for SOLIDWORKS add-ins
SolidDNA is a great framework to build SOLIDWORKS add-ins because it acts as a wrapper around the core SOLIDWORKS API. If a SOLIDWORKS API topic is hard to understand or otherwise annoying, we create a more user-friendly version for it. If we don't have an API for it yet, you can still access the underlying SOLIDWORKS objects.

We'd love your help to keep expanding and improving this project. Before starting a big pull request, please create an issue and ask if it's something that would fit this project.

## Release notes
You can find all releases and their release notes on the [Release](https://github.com/CAD-Booster/SolidDNA/releases) page and on [NuGet](https://www.nuget.org/packages/CADBooster.SolidDna).

## About this fork
This repository is a fork of [SolidDNA](https://github.com/angelsix/solidworks-api) by AngelSix. Because that project wasn't being maintained, we forked it in 2021, applied our improvements and published the results. 

We fixed bugs, made SolidDNA capable of running multiple add-ins and we strong-name signed the NuGet package. Signing lets us run multiple versions of SolidDNA side by side. To achieve that, we removed the dependency injection (because all add-ins run in the same thread) and removed running in a separate app domain (because it exposed SOLIDWORKS bugs).

And we have been improving it ever since.

## Tips for creating a pull request
If you want to add a feature to SolidDNA or want to request a change, a Pull Request is the way to go. Fork the repository, create a feature branch, make your changes and create a pull request in this project.

A few tips:

- Ask us if your feature is a good idea before starting a big PR.
- Make small, focused commits so that we can follow along.
- Use the same code style as the existing code. We don't have a proper guide yet, so please look at the rest of the code.
- Don't commit any styling changes, only code changes.
- Don't make any changes to the C# language version or .NET version.

## Getting started with the SOLIDWORKS API 
We're writing a series of articles about getting started with the SOLIDWORKS API. We're starting with the absolute basics and are trying to include all weird API behaviors beginners may trip over. Because the API docs will not teach you those tricks.

- [The SOLIDWORKS Object Model + API explained](https://cadbooster.com/the-solidworks-object-model-api-explained-part-1/)
- [SOLIDWORKS API: the basics – SldWorks, ModelDoc2](https://cadbooster.com/solidworks-api-basics-sldworks-modeldoc2/)
- [How to work with Features ](https://cadbooster.com/how-to-work-with-features-in-the-solidworks-api/)
- [Persistent ID and sketch segment ID and more](https://cadbooster.com/persistent-id-sketch-segment-id-in-the-solidworks-api/)
- [All identifiers in the SOLIDWORKS API](https://cadbooster.com/all-identifiers-and-ids-in-the-solidworks-api/)
- [About return values](https://cadbooster.com/about-return-values-in-the-solidworks-api-part-6/)
- [Entities and GetCorresponding](https://cadbooster.com/entities-and-getcorresponding-in-the-solidworks-api/)
- [Understanding math and MathTransform](https://cadbooster.com/understanding-math-and-mathtransform-in-the-solidworks-api/)
- Toolbars, menus and the Command Manager (coming soon)
- How to create task panes and Property Manager Pages (coming soon)
- How to develop a SOLIDWORKS add-in
- How to create your own SOLIDWORKS add-in (coming soon)
- SolidDNA: a better framework for SOLIDWORKS add-ins (coming soon)

## About CAD Booster
We build intuitive add-ins for SOLIDWORKS to automate the boring bits of engineering. Our main products are:

- [Drew](https://cadbooster.com/solidworks-add-in/drew/): create 2D drawings twice as fast
- [Lightning](https://cadbooster.com/solidworks-add-in/lightning-fastener-filter/): makes working with fasteners fun again
- [Fastener Models](https://cadbooster.com/fastener-models/): a great Toolbox alternative

We use SolidDNA in all of our add-ins. 

# Getting Started

## NuGet Package
The easiest way to add SolidDNA to your project is to add a reference to our NuGet package:

[<img src="https://img.shields.io/nuget/v/CADBooster.SolidDNA">](https://www.nuget.org/packages/CADBooster.SolidDna)

## AngelSix videos
Here are some videos by AngelSix on how to get started with developing your own SOLIDWORKS add-ins with C# and SolidDNA. They are a little dated by now, but we got started this way.

- Channel: https://www.youtube.com/c/angelsix
- Playlist: https://www.youtube.com/playlist?list=PLrW43fNmjaQVMN1-lsB29ECnHRlA4ebYn

## How to create an add-in 

<details>

<summary>Expand/collapse example</summary>

If you are completely new to software development, you first need to download an IDE (a development environment like [Visual Studio Community](https://visualstudio.microsoft.com/downloads/)) and a git client (for version control) like [Gitkraken](https://www.gitkraken.com/git-client) or [Github Desktop](https://desktop.github.com/download/). All have free versions.

Once you have Visual Studio open:
1. Click 'Create a new project':
     1. Select the type "Class library (.NET Framework)
     2. Call it something like YourCompany.MyAddIn and select a location for your files.
     3. Select ".NET Framework 4.8" for the framework.
     4. Visual Studio will also create a new Solution for you, which can contain multiple projects.
     5. If you're creating a small add-in, a single project is enough and it can have the same name as the solution.
3. Add a reference to our NuGet package: 
    1. Go to your project tree, right-click Dependencies and click Manage NuGet Packages:
    2. Go to the Browse tab and search for CADBooster.SolidDNA.
    3. Click Install on the right.
    4. [NuGet](https://www.nuget.org/) is a package manager that lets you add functionality to your product with a few clicks.
4. **This step is no longer necessary, starting with version 4.0!** Add references to the SOLIDWORKS DLL files:
    1. Download the SOLIDWORKS DLL files from our [References](https://github.com/CAD-Booster/solidworks-api/tree/master/References) folder or get them from your SOLIDWORKS folder (C:\Program Files\SOLIDWORKS Corp\SOLIDWORKS\api\redist). Use the same version as SolidDNA does, or your code will not compile. The DLL version does not influence in which SOLIDWORKS version your add-in will run. 
    2. Copy all files into your project folder, for example to a References folder.
    3. Go to your project tree, right-click Dependencies and click Add Assembly Reference.
    4. Click Browse and select all 9 SOLIDWORKS DLLs. Only the ones that you really use will get copied to the output folder later on.
    5. Expand Dependencies > Assemblies in the tree, select all SOLIDWORKS DLLs and set their property Embed Interop Types to No.
5. Create a class that implements SolidAddIn:
    1. Right-click your project > Add > Class, name it something like MyAddIn.cs.
    2. Make sure the class is marked as "public" because we will need to register our class in the Windows Registry later.
    3. Add a Guid attribute and a ComVisible(true) attribute. [Create a new GUID](https://guidgenerator.com/) and add it, do not copy the example value. This GUID is the unique identifier for your add-in, so it can never ever change. If you become a SOLIDWORKS partner, you give them this GUID as well.
    4. Add " : SolidAddIn" after your class name to make your class implement the SolidAddIn class. Visual Studio will tell you that you need to implement three methods if you want to implement SolidAddIn, so do that.
    5. Add a MessageBox.Show line so we can verify that we started up correctly.
    5. The result should be something like this:

```csharp
using System.Runtime.InteropServices;
using CADBooster.SolidDna;

namespace CADBooster.MyFirstPlugin;

[Guid("CA31FE14-A587-40BA-B549-0C4BA03611A9"), ComVisible(true)] // Replace the GUID with your own, then never change it 
public class MyAddIn : SolidAddIn
{
    /// <summary>
    /// Run immediately when <see cref="SolidAddIn.ConnectToSW(object, int)"/> is called to do any pre-setup.
    /// For example, call <see cref="Logger.AddFileLogger{TAddIn}"/> to add a file logger for SolidDna messages.
    /// Runs before <see cref="PreLoadPlugIns"/> and before <see cref="ApplicationStartup"/>.
    /// </summary>
    public override void PreConnectToSolidWorks()
    {
        // Is called first. You generally don't need to do anything here.
    }

    /// <summary>
    /// Run before loading plug-ins.
    /// This call should be used to add plug-ins to be loaded, via <see cref="PlugInIntegration.AddPlugIn{T}"/>.
    /// Runs after <see cref="PreConnectToSolidWorks"/> and before <see cref="ApplicationStartup"/>.
    /// </summary>
    public override void PreLoadPlugIns()
    {
        // Is called second. You normally don't need to do anything here. 
		// We will discover all plugins inside your add-in DLL file by looking through 
		// all DLLs in folder that contains the SolidDNA dll.
		
		// But you make starting up a lot faster by telling us which plugin(s) to start.
		PlugInIntegration.AddPlugInToLoad<MyPlugin>();
		
		// Second option is a little slower:
		// Disable auto-discovery and set a path to your DLL file. We will discover all plugins in this DLL.
		var assemblyPath = new MyPlugin().AssemblyFilePath();
		PlugInIntegration.PlugInAssemblyPaths.Add(assemblyPath);
		PlugInIntegration.AutoDiscoverPlugins = false;
    }

    /// <summary>
    /// Specific application startup code when SolidWorks is connected and before any plug-ins or listeners are informed.
    /// </summary>
    public override void ApplicationStartup()
    {
        // Is called third. Here you do general add-in startup stuff. 
		// You can also do startup work in your plugin's ConnectedToSolidWorks method, but that runs later.
    }
}
```

6. Create a class that implements SolidPlugIn:
    1. Plugins are a SolidDNA concept that does not exist in SOLIDWORKS. An SolidAddIn can have multiple SolidPlugIns, but you generally need only one.
    2. Follow the same steps as for the add-in class. I'm not 100% sure this class also needs to be public, but I make it public, add a GUID and a ComVisible(true) attribute anyway.
    3. Add " : SolidPlugIn" after your class name to make your class implement the SolidPlugIn class. Visual Studio will tell you that you need to implement two methods and two properties to implement SolidPlugIn, so do that.
    4. The result should be something like this:

```csharp
using System.Runtime.InteropServices;
using System.Windows;
using CADBooster.SolidDna;

namespace CADBooster.MyFirstPlugin;

[Guid("BDABC95A-53A3-48D0-89B7-2FAADCDE4889"), ComVisible(true)] // Replace the GUID with your own, then never change it 
public class MyPlugin : SolidPlugIn
{
    public override string AddInTitle => "My amazing plugin";
    public override string AddInDescription => "My colleagues are going to love this";
    public override void ConnectedToSolidWorks()
    {
        MessageBox.Show("We have started up!");
    }

    public override void DisconnectedFromSolidWorks()
    {
            
    }
}
```

7. Build your solution:
    1. Select Debug from the dropdown at the top of Visual Studio.
    2. Click Build > Build Solution or press Ctrl Shift B. You will either see errors or see a message "Build succeeded" in the bottom left.
    3. Your result files will appear in your code folder under "bin/Debug/net48". There will be a DLL file for each of your projects and a copy of the SOLIDWORKS DLL files you used will also appear here.
8. Register your add-in:
    1. A SOLIDWORKS add-in is not an EXE file that you can run. You create a DLL file and tell SOLIDWORKS that it needs to look inside that add-in for a class that implements SolidAddIn. It uses the Windows Registry for this, so we need to add our add-in to the Registry.
    2. Run the [Add-in Installer](https://github.com/CAD-Booster/solidworks-api/tree/master/Tools/). We added the exe to the project so you don't have to compile it youself. 
    4. Click the second Browse button and select your project DLL files in "bin/Debug/net48".
    5. Click Install. This runs a tool called RegAsm, which identifies the public classes inside your DLL that need to be added to the Windows Registry. It also adds a key to add your add-in to the list of SOLIDWORKS add-ins. You can read more [here](https://github.com/CAD-Booster/solidworks-api/tree/master/Tools/Addin%20Installer).
    6. Later: click Uninstall to remove your add-in again. **Always do this before making big changes** like renaming your add-in or plugin or changing a GUID, or you'll mess up your registry and the add-in may not start.
9. Run your add-in:
    1. If you now start SOLIDWORKS, your add-in should start as well. Pretty cool, right?
    2. If the add-in does not start, check the list of add-ins to make sure it's enabled.
    3. If you enable the add-in, close the add-ins window and the checkbox gets unchecked again, there was some sort of error and the add-in did not start correctly.
	4. Your add-in should run in all latest SOLIDWORKS versions. If you use APIs that are not available in that SOLIDWORKS version, your code will throw an exception. You can avoid this problem by checking the current SOLIDWORKS version first, or by avoiding all recent APIs like I do.
9. Debug your add-in:
    1. We can also start up SOLIDWORKS from Visual Studio, which lets us debug our add-in and run the code line by line. This is crucial for proper software development.
    2. Go into your project folder, then inside the Properties folder.
    3. Create a file called "launchSettings.json" and add at least one profile. Make sure the path is correct, you can choose the name (like SW2024 here) yourself. I added two SOLIDWORKS versions here.
    4. Open Visual Studio again and select one of your profiles next to the Debug dropdown and the play button.
    5. Set a breakpoint in your add-in or plugin code by clicking left of a relevant line of code. This will highlight the line in red. If we reach that code, the debugger will pause there.
    6. Click play or press F5. This starts the debugger and SOLIDWORKS.
    7. Once you hit your breakpoint, hit F5 to continue until the next breakpoint or hit F10 to go to the next line.
    8. Extra tip: enable [Hot Reload](https://learn.microsoft.com/en-us/visualstudio/debugger/hot-reload?view=vs-2022&pivots=programming-language-dotnetf) so you can make changes and save your code while SOLIDWORKS is running. Hot Reload is awesome. Very often, you can make quick changes this way without having to restart SOLIDWORKS.
  
```json
{
  "profiles": {
    "SW2024": {
      "commandName": "Executable",
      "executablePath": "C:\\Program Files\\SOLIDWORKS Corp\\SOLIDWORKS (1)\\SLDWORKS.exe"
    },
    "SW2025": {
      "commandName": "Executable",
      "executablePath": "C:\\Program Files\\SOLIDWORKS Corp\\SOLIDWORKS (2)\\SLDWORKS.exe"
    }
  }
}
```

![image](https://raw.githubusercontent.com/CAD-Booster/SolidDNA/master/Images/first-plugin-debugging.png)

You now have the absolute basics in place for a SOLIDWORKS add-in! 
</details>