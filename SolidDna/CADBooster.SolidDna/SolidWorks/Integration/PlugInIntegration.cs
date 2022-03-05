﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Provides functions related to SolidDna plug-ins
    /// </summary>
    public class PlugInIntegration
    {
        #region Public Properties

        /// <summary>
        /// The add-in that owns this plugin integration.
        /// </summary>
        public SolidAddIn ParentAddIn { get; set; }
        
        /// <summary>
        /// A list of assembly paths that contain plug-ins that have been added to be loaded. 
        /// Contains the absolute file paths of all DLLs that contain at least one <see cref="SolidPlugIn"/>.
        /// </summary>
        public List<string> PlugInAssemblyPaths { get; } = new List<string>();

        /// <summary>
        /// Default true. If true, searches in the directory of the application (where CADBooster.SolidDna.dll is) for any dll that
        /// contains any <see cref="SolidPlugIn"/> implementations and adds them to the <see cref="PlugInAssemblyPaths"/>
        /// during the <see cref="ConfigurePlugIns(string, SolidAddIn)"/> stage.
        /// If false, the user should add paths to all DLLs that contain <see cref="SolidPlugIn"/> types to the list
        /// <see cref="PlugInAssemblyPaths"/> during the <see cref="SolidAddIn.PreLoadPlugIns"/> method.
        /// Setting this to false will make starting up your add-in faster because we don't have the check each DLL.
        /// </summary>
        public bool AutoDiscoverPlugins { get; set; } = true;

        #endregion

        #region Public Events

        /// <summary>
        /// Called when a SolidWorks callback is fired
        /// </summary>
        public static event Action<string> CallbackFired = (name) => { };

        #endregion

        #region Setup / Tear down

        #endregion

        #region Connected to SolidWorks

        /// <summary>
        /// Called when the add-in has connected to SolidWorks
        /// </summary>
        public void ConnectedToSolidWorks(SolidAddIn solidAddIn)
        {
            solidAddIn.OnConnectedToSolidWorks();

            // Inform plug-ins
            solidAddIn.PlugIns.ForEach(plugin =>
            {
                // Log it
                Logger.LogDebugSource($"Firing ConnectedToSolidWorks event for plugin `{plugin.AddInTitle}`...");

                plugin.ConnectedToSolidWorks();
            });
        }

        /// <summary>
        /// Called when the add-in has disconnected from SolidWorks
        /// </summary>
        public void DisconnectedFromSolidWorks(SolidAddIn solidAddIn)
        {
            solidAddIn.OnDisconnectedFromSolidWorks();

            // Inform plug-ins
            solidAddIn.PlugIns.ForEach(plugin =>
            {
                // Log it
                Logger.LogDebugSource($"Firing DisconnectedFromSolidWorks event for plugin `{plugin.AddInTitle}`...");

                plugin.DisconnectedFromSolidWorks();
            });
        }

        #endregion

        #region Add Plug-in

        /// <summary>
        /// Adds a plug-in based on its <see cref="SolidPlugIn"/> implementation
        /// </summary>
        /// <typeparam name="T">The class that implements the <see cref="SolidPlugIn"/></typeparam>
        public void AddPlugIn<T>()
        {
            // Get the full path to the assembly
            var fullPath = typeof(T).Assembly.CodeBase.Replace(@"file:\", "").Replace(@"file:///", "");

            // Add the path to list if it isn't in there yet
            if (!PlugInAssemblyPaths.ContainsIgnoreCase(fullPath))
                PlugInAssemblyPaths.Add(fullPath);
        }

        #endregion

        #region SolidWorks Callbacks

        /// <summary>
        /// Called by the SolidWorks domain (<see cref="SolidAddIn"/>) when a callback is fired
        /// </summary>
        /// <param name="name">The parameter passed into the generic callback</param>
        public void OnCallback(string name)
        {
            try
            {
                // Inform listeners
                CallbackFired(name);
            }
            catch (Exception ex)
            {
                Debugger.Break();

                // Log it
                Logger.LogCriticalSource($"OnCallback failed. {ex.GetErrorMessage()}");
            }
        }

        #endregion

        #region Configure Plug Ins

        /// <summary>
        /// Runs any initialization code required on plug-ins.
        /// This is run for the ComRegister function and on startup.
        /// </summary>
        /// <param name="addinPath">The path to the add-in that is calling this setup (typically acquired using GetType().Assembly.Location)</param>
        /// <param name="solidAddIn"></param>
        public void ConfigurePlugIns(string addinPath, SolidAddIn solidAddIn)
        {
            // *********************************************************************************
            //
            // WARNING: 
            // 
            //   If SolidWorks is loading our add-ins and we have multiple that use SolidDna
            //   it loads and makes use of the existing CADBooster.SolidDna.dll file from
            //   the first add-in loaded and shares it for all future add-ins
            //
            //   This results in any static instances being shared and only one version 
            //   of SolidDna being usable on an individual SolidWorks instance 
            //
            //   This is default .NET behavior because .NET reuses DLLs with the same filename and
            //   ignores the version. Only when the DLL is strong-signed will it take the
            //   DLL version into account.
            //
            //   Keep in mind that any static values inside the CADBooster.SolidDna class
            //   are be shared between add-ins.
            //          
            //
            // *********************************************************************************

            // Load all plug-ins at this stage for faster lookup
            solidAddIn.PlugIns = GetSolidPlugIns(addinPath);

            // Log it
            Logger.LogDebugSource($"{solidAddIn.PlugIns.Count} plug-ins found");

            // Find first plug-in in the list and use that as the title and description (for COM register)
            var firstPlugInWithTitle = solidAddIn.PlugIns.FirstOrDefault(f => !string.IsNullOrEmpty(f.AddInTitle));

            // If we have a title...
            if (firstPlugInWithTitle != null)
            {
                // Log it
                Logger.LogDebugSource($"Setting Add-In Title:       {firstPlugInWithTitle.AddInTitle}");
                Logger.LogDebugSource($"Setting Add-In Description: {firstPlugInWithTitle.AddInDescription}");

                // Set title and description details
                solidAddIn.SolidWorksAddInTitle = firstPlugInWithTitle.AddInTitle;
                solidAddIn.SolidWorksAddInDescription = firstPlugInWithTitle.AddInDescription;
            }
            // Otherwise
            else
                // Log it
                Logger.LogDebugSource($"No PlugIn's found with a title.");
        }

        /// <summary>
        /// Discovers all SolidDna plug-ins
        /// </summary>
        /// <param name="addinPath">The path to the add-in that is calling this setup (typically acquired using GetType().Assembly.Location)</param>
        /// <returns></returns>
        private List<SolidPlugIn> GetSolidPlugIns(string addinPath)
        {
            // Create new empty list
            var plugIns = new List<SolidPlugIn>();

            // Find all DLLs in the same directory
            if (AutoDiscoverPlugins)
            {
                // Log it
                Logger.LogDebugSource($"Loading all PlugIns...");

                // Add new based on if found
                foreach (var path in Directory.GetFiles(addinPath, "*.dll", SearchOption.TopDirectoryOnly))
                    GetPlugIns(path, (plugin) =>
                    {
                        // Log it
                        Logger.LogDebugSource($"Found plugin {plugin.AddInTitle} in {path}");

                        plugIns.Add(plugin);
                    });
            }
            // Or load explicit ones
            else
            {
                // Log it
                Logger.LogDebugSource($"Explicitly loading {PlugInAssemblyPaths.Count} PlugIns...");

                // For each assembly path
                foreach (var path in PlugInAssemblyPaths)
                {
                    try
                    {
                        // Try and find the SolidPlugIn implementation...
                        GetPlugIns(path, (plugin) =>
                        {
                            // Log it
                            Logger.LogDebugSource($"Found plugin {plugin.AddInTitle} in {path}");

                            // Add it to the list
                            plugIns.Add(plugin);
                        });
                    }
                    catch (Exception ex)
                    {
                        // Log error
                        Logger.LogCriticalSource($"Unexpected error: {ex}");
                    }
                }
            }

            // Log it
            Logger.LogDebugSource($"Loaded {plugIns?.Count} plug-ins from {addinPath}");

            return plugIns;
        }

        /// <summary>
        /// Loads the dll into the current app domain, and finds any <see cref="SolidPlugIn"/> implementations, calling onFound when it finds them
        /// </summary>
        /// <param name="pluginFullPath">The full path to the plug-in dll to load</param>
        /// <param name="onFound">Called when a <see cref="SolidPlugIn"/> is found</param>
        private void GetPlugIns(string pluginFullPath, Action<SolidPlugIn> onFound)
        {
            // Load the assembly
            // NOTE: Calling LoadFrom instead of LoadFile will auto-resolve references in that folder
            //       otherwise they won't resolve.
            //       For this reason its important that plug-ins are in the same folder as the 
            //       CADBooster.SolidDna.dll and all other used references
            var assembly = Assembly.LoadFrom(pluginFullPath);

            // If we didn't succeed, ignore
            if (assembly == null)
                return;
            
            // Find all types in an assembly. Catch assemblies that don't allow this.
            Type[] types;
            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException)
            {
                return;
            }

            var type = typeof(SolidPlugIn);

            // See if any of the type are of SolidPlugIn
            types.Where(p => type.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract).ToList().ForEach(p =>
            {
                // Create SolidDna plugin class instance
                if (Activator.CreateInstance(p) is SolidPlugIn plugIn)
                {
                    // Store the add-in that owns this plugin
                    plugIn.ParentAddIn = ParentAddIn;

                    // Call the action that further sets up the plugin
                    onFound(plugIn);
                }
            });
        }

        #endregion
    }
}
