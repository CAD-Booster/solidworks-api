﻿using CADBooster.SolidDna;
using System.IO;
using System.Runtime.InteropServices;

namespace SolidDNA.ScriptRunner
{
    // 
    //  *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //
    //     Welcome to SolidDNA by AngelSix and CAD Booster
    //
    //        SolidDNA is a modern framework designed to make developing SolidWorks Add-ins easy.
    //
    //        With this template you have a ready-to-go add-in that will load inside of SolidWorks
    //        and a bunch of useful example projects available here 
    //        https://github.com/cad-booster/solidworks-api/tree/develop/Tutorials
    //
    //
    //     Registering Add-in Dll
    //
    //        To get your dll to run inside SolidWorks as an add-in you need to register it.
    //        Inside this project template in the Resources folder is the SolidWorksAddinInstaller.exe.
    //        Compile your project, open up the SolidWorksAddinInstaller.exe, then browse for your
    //        output dll file (for example /bin/Debug/SolidDNA.ScriptRunner.dll) and click Install.
    //
    //        Now when you start SolidWorks your Add-in should load and should appear in the 
    //        Tools > Add-ins menu. 
    //
    //        NOTE: You only need to register your add-in once, or when you move the location or 
    //              change the filename.
    //        
    //
    //     Debugging Code
    //
    //        In order to press F5 to start up SolidWorks and instantly begin debugging your code,
    //        open up Project Properties, go to Debug, select Start External Program, and point
    //        to `C:\Program Files\SOLIDWORKS Corp\SOLIDWORKS\SLDWORKS.exe` by default.
    //        If your install is in a different location just change this path.
    //
    //        Also the Project `Properties > Application > Assembly Information` has the
    //        `Make Assembly COM Visible` checked.
    //
    //
    //     Startup Flow
    //
    //        When your SolidDna add-in first loads, SolidWorks will call the ConnectToSW method
    //        inside your AddInIntegration class. 
    //
    //        This method will fire the following methods in this order:
    // 
    //         - PreConnectToSolidWorks
    //         - PreLoadPlugIns
    //         - ApplicationStartup
    //         - ConnectedToSolidWorks
    //        
    //        Once your add-in is unloaded by SolidWorks the DisconnectFromSW method will be called
    //        which will in turn fire the following methods:
    //
    //         - DisconnectedFromSolidWorks
    //
    //  *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //

    /// <summary>
    /// Register as a SolidWorks Add-In
    /// </summary>
    [Guid("58EAD1BB-6E63-4B53-BE14-EDB1BF3C4A93"), ComVisible(true)]  // Replace the GUID with your own.
    public class MyAddIn : SolidAddIn
    {
        /// <summary>
        /// Specific application start-up code
        /// </summary>
        public override void ApplicationStartup()
        {

        }

        /// <summary>
        /// Use this to do early initialization and any configuration of the 
        /// PlugInIntegration class properties.
        /// </summary>
        public override void PreConnectToSolidWorks()
        {

        }

        /// <summary>
        /// Steps to take before any plug-in loads
        /// </summary>
        public override void PreLoadPlugIns()
        {

        }
    }

    /// <summary>
    /// Registers as a SolidDna PlugIn to be loaded by our AddIn Integration class 
    /// when the SolidWorks add-in gets loaded.
    /// 
    /// NOTE: We can have multiple plug-ins in a single add-in
    /// </summary>
    public class MySolidDnaPlugIn : SolidPlugIn<MySolidDnaPlugIn>
    {
        #region Private Members

        /// <summary>
        /// The Taskpane UI for our plug-in
        /// </summary>
        private TaskpaneIntegration<MyTaskpaneUI, MyAddIn> mTaskpane;

        #endregion

        #region Region Public Properties

        /// <summary>
        /// My Add-in title
        /// </summary>
        public override string AddInTitle => "SolidDNA ScriptRunner";

        /// <summary>
        /// My Add-in description
        /// </summary>
        public override string AddInDescription => "Compiles and runs C# scripts live";

        #endregion

        #region Connect To SolidWorks

        public override void ConnectedToSolidWorks()
        {
            // Create our taskpane UI
            mTaskpane = new TaskpaneIntegration<MyTaskpaneUI, MyAddIn>()
            {
                // Set taskpane icons. {0} is replaced by the actual image sizes.
                IconPathFormat = Path.Combine(this.AssemblyPath(), "Assets\\icons{0}.png"),
                WpfControl = new MyAddinControl()
            };

            // Add it to taskpane
            mTaskpane.AddToTaskpaneAsync();
        }

        public override void DisconnectedFromSolidWorks()
        {

        }

        #endregion
    }
}
