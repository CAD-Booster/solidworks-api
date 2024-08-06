using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swpublished;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Integrates into SolidWorks as an add-in and registers for callbacks provided by SolidWorks
    /// 
    /// IMPORTANT: The class that overrides <see cref="ISwAddin"/> MUST be the same class that 
    /// contains the ComRegister and ComUnregister functions due to how SolidWorks loads add-ins
    /// </summary>
    public abstract class SolidAddIn : ISwAddin, ISwPEManager
    {
        #region Protected Members

        /// <summary>
        /// Flag if we have loaded into memory (as ConnectedToSolidWorks can happen multiple times if unloaded/reloaded)
        /// </summary>
        protected bool mLoaded;

        #endregion

        #region Public Properties

        /// <summary>
        /// Provides functions related to SolidDna plug-ins
        /// </summary>
        public PlugInIntegration PlugInIntegration { get; private set; } = new PlugInIntegration();

        /// <summary>
        /// A list of available plug-ins loaded once SolidWorks has connected
        /// </summary>
        public List<SolidPlugIn> PlugIns { get; set; } = new List<SolidPlugIn>();

        /// <summary>
        /// The title displayed for this SolidWorks Add-in
        /// </summary>
        public string SolidWorksAddInTitle { get; set; } = "CADBooster SolidDna AddIn";

        /// <summary>
        /// The description displayed for this SolidWorks Add-in
        /// </summary>
        public string SolidWorksAddInDescription { get; set; } = "All your pixels are belong to us!";

        /// <summary>
        /// The Partner Product license key for this SolidWorks add-in.
        /// Enter your key here if your add-in is a registered partner product for SolidWorks 2021 or newer. Make sure the key is valid for the current SolidWorks version.
        /// Set this value in your add-in constructor because the <see cref="IdentifyToSW"/> event is fired before <see cref="ConnectToSW"/>.
        /// Let it be an empty string if your add-in is not a registered partner product.
        /// If the key is valid, the add-in appears under the group 'Partner Gold Add-ins' or 'Partner Solution Add-ins'.
        /// If the key is empty or not valid, the add-in appears under the group 'Other Add-ins'.
        /// If the key length is not 128 characters, SolidWorks throws an exception and your add-in will not load. So we catch that exception and set the status to <see cref="PartnerAddInKeyStatus.IncorrectPartnerLicenseKeyLength"/>.
        /// More info: <see href="https://help.solidworks.com/2024/english/api/sldworksapiprogguide/GettingStarted/SolidWorks_Partner_Program_2.htm" />
        /// </summary>
        public string SolidWorksAddInPartnerLicenseKey { get; set; } = "";

        /// <summary>
        /// The resulting partner add-in status. If <see cref="SolidWorksAddInPartnerLicenseKey"/> is an empty string, the status will be PartnerAddInKeyStatus.Fail.
        /// This is not a problem, but your add-in will appear under the group 'Other Add-ins'. 
        /// See <see cref="SolidWorksAddInPartnerLicenseKey"/> for more info.
        /// </summary>
        public PartnerAddInKeyStatus SolidWorksAddInPartnerKeyStatus { get; private set; }

        #endregion

        #region Public Events

        /// <summary>
        /// Called once SolidWorks has loaded our add-in and is ready.
        /// Now is a good time to create task panes, menu bars or anything else.
        ///  
        /// NOTE: This call will be made twice, one in the default domain and one in the AppDomain as the SolidDna plug-ins
        /// </summary>
        public event Action ConnectedToSolidWorks = () => { };

        /// <summary>
        /// Called once SolidWorks has unloaded our add-in.
        /// Now is a good time to clean up task panes, menu bars or anything else.
        /// 
        /// NOTE: This call will be made twice, one in the default domain and one in the AppDomain as the SolidDna plug-ins
        /// </summary>
        public event Action DisconnectedFromSolidWorks = () => { };

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public SolidAddIn()
        {
            PlugInIntegration.ParentAddIn = this;
        }

        #endregion

        #region Public Abstract / Virtual Methods

        /// <summary>
        /// Specific application startup code when SolidWorks is connected 
        /// and before any plug-ins or listeners are informed
        /// 
        /// NOTE: This call will not be in the same AppDomain as the SolidDna plug-ins
        /// </summary>
        /// <returns></returns>
        public abstract void ApplicationStartup();

        /// <summary>
        /// Run immediately when <see cref="ConnectToSW(object, int)"/> is called to do any pre-setup.
        /// For example, call <see cref="Logger.AddFileLogger{TAddIn}"/> to add a file logger for SolidDna messages.
        /// </summary>
        public abstract void PreConnectToSolidWorks();

        /// <summary>
        /// Run before loading plug-ins.
        /// This call should be used to add plug-ins to be loaded, via <see cref="PlugInIntegration.AddPlugIn{T}"/>
        /// </summary>
        /// <returns></returns>
        public abstract void PreLoadPlugIns();

        #endregion

        #region SolidWorks Add-in Callbacks

        /// <summary>
        /// Receives all callbacks from command manager items and flyouts. 
        /// We tell SolidWorks to call a method in the <see cref="SolidAddIn"/> class in <see cref="SetUpCallbacks"/>
        /// We tell it to call this method in <see cref="CommandManagerGroup.AddCommandItem"/> and <see cref="CommandManagerFlyout.AddCommandItem"/>.
        /// We forward this to <see cref="PlugInIntegration.OnCallback"/>, which then finds the correct command manager item or flyout and calls its OnClick method.
        /// </summary>
        /// <param name="arg"></param>
        public void Callback(string arg)
        {
            // Log it
            Logger.LogDebugSource($"SolidWorks Callback fired {arg}");

            PlugInIntegration.OnCallback(arg);
        }

        /// <summary>
        /// Called when SolidWorks has loaded our add-in and wants us to do our connection logic
        /// </summary>
        /// <param name="thisSw">The current SolidWorks instance</param>
        /// <param name="cookie">The current SolidWorks cookie ID</param>
        /// <returns></returns>
        public bool ConnectToSW(object thisSw, int cookie)
        {
            try
            {
                // Add this add-in to the list of currently active add-ins.
                AddInIntegration.AddAddIn(this);

                // Fire event
                PreConnectToSolidWorks();

                // Log it
                Logger.LogTraceSource($"Fired PreConnectToSolidWorks...");

                // Get the directory path to this actual add-in dll
                var assemblyPath = this.AssemblyPath();

                // Log it
                Logger.LogDebugSource($"{SolidWorksAddInTitle} Connected to SolidWorks...");

                //
                //   NOTE: Do not need to create it here, as we now create it inside PlugInIntegration.Setup in its own AppDomain
                //         If we change back to loading directly (not in an app domain) then uncomment this 
                //
                // Store a reference to the current SolidWorks instance
                // Initialize SolidWorks (SolidDNA class)
                //SolidWorks = new SolidWorksApplication((SldWorks)ThisSW, Cookie);

                // Tell solidworks which method to call when it receives a button click on a command manager item or flyout.
                SetUpCallbacks(thisSw, cookie);

                // Log it
                Logger.LogDebugSource($"Storing the SOLIDWORKS instance...");

                // Set up the current SolidWorks instance as a SolidDNA class.
                AddInIntegration.ConnectToActiveSolidWorks(((SldWorks)thisSw).RevisionNumber(), cookie);

                // Log it
                Logger.LogDebugSource($"Firing PreLoadPlugIns...");

                // If this is the first load
                if (!mLoaded)
                {
                    // Any pre-load steps
                    PreLoadPlugIns();

                    // Log it
                    Logger.LogDebugSource($"Configuring PlugIns...");

                    // Perform any plug-in configuration
                    PlugInIntegration.ConfigurePlugIns(assemblyPath, this);

                    // Now loaded so don't do it again
                    mLoaded = true;
                }

                // Log it
                Logger.LogDebugSource($"Firing ApplicationStartup...");

                // Call the application startup function for an entry point to the application
                ApplicationStartup();

                // Log it
                Logger.LogDebugSource($"Firing ConnectedToSolidWorks...");

                // Inform listeners
                ConnectedToSolidWorks();

                // Log it
                Logger.LogDebugSource($"PlugInIntegration ConnectedToSolidWorks...");

                // And plug-in domain listeners
                PlugInIntegration.ConnectedToSolidWorks(this);

                // Return ok
                return true;
            }
            catch (Exception ex)
            {
                // Log it
                Logger.LogCriticalSource($"Unexpected error: {ex}");

                return false;
            }
        }

        /// <summary>
        /// Called when SolidWorks is about to unload our add-in and wants us to do our disconnection logic
        /// </summary>
        /// <returns></returns>
        public bool DisconnectFromSW()
        {
            // Log it
            Logger.LogDebugSource($"{SolidWorksAddInTitle} Disconnected from SolidWorks...");

            // Log it
            Logger.LogDebugSource($"Firing DisconnectedFromSolidWorks...");

            // Inform listeners
            DisconnectedFromSolidWorks();

            // And plug-in domain listeners
            PlugInIntegration.DisconnectedFromSolidWorks(this);

            // Log it
            Logger.LogDebugSource($"Tearing down...");

            // Remove it from the list and tear down SOLIDWORKS when it was the last add-in.
            AddInIntegration.RemoveAddInAndTearDownSolidWorksWhenLast(this);

            // Remove the loggers for this add-in
            Logger.RemoveLoggers(this);

            // Clear our references
            PlugInIntegration = null;

            // Reset mLoaded so we can restart this add-in
            mLoaded = false;

            // Return ok
            return true;
        }

        /// <summary>
        /// Called when SolidWorks tries to determine if this add-in is registered with the Partner Program.
        /// See <see cref="SolidWorksAddInPartnerLicenseKey"/>.
        /// </summary>
        /// <param name="classFactory"></param>
        public void IdentifyToSW(object classFactory)
        {
            if (!(classFactory is ISwPEClassFactory factory)) return;

            if (SolidWorksAddInPartnerLicenseKey.IsNullOrEmpty())
            {
                // The partner license key should be an empty string when your add-in is not a SolidWorks partner product.
                // If we pass an empty string, the status will be PartnerAddInKeyStatus.Fail and the add-in will load correctly. This is not a problem.
                SolidWorksAddInPartnerKeyStatus = IdentifyAddinToSolidWorks(factory, "");
            }
            else if (SolidWorksAddInPartnerLicenseKey.Length != 128)
            {
                // The length of the partner license key should be exactly 128 characters long when your add-in is a SolidWorks partner product.
                // SolidWorks throws an exception when you pass a string of the incorrect length and your add-in will not load.
                // But we have to call ISwPEClassFactory.SetPartnerKey or the add-in will not load either, so we pass an empty string and ignore the return value.
                IdentifyAddinToSolidWorks(factory, "");
                SolidWorksAddInPartnerKeyStatus = PartnerAddInKeyStatus.IncorrectPartnerLicenseKeyLength;
            }
            else
            {
                // Register our add-in as a partner product.
                SolidWorksAddInPartnerKeyStatus = IdentifyAddinToSolidWorks(factory, SolidWorksAddInPartnerLicenseKey);
            }
        }

        /// <summary>
        /// Register our add-in as a partner product. Pass an empty string if your add-in is not a partner product.
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static PartnerAddInKeyStatus IdentifyAddinToSolidWorks(ISwPEClassFactory factory, string key)
        {
            try
            {
                return (PartnerAddInKeyStatus)factory.SetPartnerKey(key, out var tokenForFutureUse);
            }
            catch (Exception e)
            {
                // The exception is normally swallowed by SolidWorks, so rethrowing doesn't help.
                Logger.LogCriticalSource("An exception occurred while trying to identify the add-in to SolidWorks.", exception: e);
                MessageBox.Show($"An exception occurred while setting the add-in partner status in SolidWorks: {e.Message}");
                return PartnerAddInKeyStatus.Fail;
            }
        }

        /// <summary>
        /// Tell SolidWorks that it should call the <see cref="Callback"/> method in this class whenever it receives a Command Manager item or flyout button click.
        /// We forward this to <see cref="PlugInIntegration.OnCallback"/>, which then finds the correct command manager item or flyout and calls its OnClick method.
        /// </summary>
        /// <param name="thisSw"></param>
        /// <param name="cookie"></param>
        private void SetUpCallbacks(object thisSw, int cookie)
        {
            // Log it
            Logger.LogDebugSource($"Setting AddinCallbackInfo...");

            var ok = ((SldWorks)thisSw).SetAddinCallbackInfo2(0, this, cookie);
        }

        #endregion

        #region Connected to SolidWorks Event Calls

        /// <summary>
        /// When the add-in has connected to SolidWorks
        /// </summary>
        public void OnConnectedToSolidWorks()
        {
            // Log it
            Logger.LogDebugSource($"Firing ConnectedToSolidWorks event...");

            ConnectedToSolidWorks();
        }

        /// <summary>
        /// When the add-in has disconnected to SolidWorks
        /// </summary>
        public void OnDisconnectedFromSolidWorks()
        {
            // Log it
            Logger.LogDebugSource($"Firing DisconnectedFromSolidWorks event...");

            DisconnectedFromSolidWorks();
        }

        #endregion

        #region Com Registration

        /// <summary>
        /// The COM registration call to add our registry entries to the SolidWorks add-in registry
        /// </summary>
        /// <param name="t"></param>
        [ComRegisterFunction]
        protected static void ComRegister(Type t)
        {
            try
            {
                // Create new instance of a blank add-in
                var addIn = new BlankSolidAddIn();

                // Get assembly name
                var assemblyName = t.Assembly.Location;

                // Log it
                Logger.LogInformationSource($"Registering {assemblyName}");

                // Get registry key path
                var keyPath = string.Format(@"SOFTWARE\SolidWorks\AddIns\{0:b}", t.GUID);

                // Create our registry folder for the add-in
                using (var rk = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(keyPath))
                {
                    // Load add-in when SolidWorks opens
                    rk.SetValue(null, 1);

                    //
                    // IMPORTANT: 
                    //
                    //   In this special case, COM register won't load the wrong CADBooster.SolidDna.dll file 
                    //   as it isn't loading multiple instances and keeping them in memory
                    //            
                    //   So loading the path of the CADBooster.SolidDna.dll file that should be in the same
                    //   folder as the add-in dll right now will work fine to get the add-in path
                    //
                    var pluginPath = typeof(PlugInIntegration).CodeBaseNormalized();

                    // Force auto-discovering plug-in during COM registration
                    addIn.PlugInIntegration.AutoDiscoverPlugins = true;

                    Logger.LogInformationSource("Configuring plugins...");

                    // Let plug-ins configure title and descriptions
                    addIn.PlugInIntegration.ConfigurePlugIns(pluginPath, addIn);

                    // Set SolidWorks add-in title and description
                    rk.SetValue("Title", addIn.SolidWorksAddInTitle);
                    rk.SetValue("Description", addIn.SolidWorksAddInDescription);

                    Logger.LogInformationSource($"COM Registration successful. '{addIn.SolidWorksAddInTitle}' : '{addIn.SolidWorksAddInDescription}'");
                }
            }
            catch (Exception ex)
            {
                Debugger.Break();

                // Get the path to this DLL
                var assemblyLocation = typeof(SolidAddIn).AssemblyFilePath();

                // Create a path for a text file. The assembly location is always lowercase.
                var changeExtension = assemblyLocation.Replace(".dll", ".fatal.log.txt");

                // Log an error to a new or existing text file 
                File.AppendAllText(changeExtension, $"\r\nUnexpected error: {ex}");
                
                Logger.LogCriticalSource($"COM Registration error. {ex}");
                throw;
            }
        }

        /// <summary>
        /// The COM unregister call to remove our custom entries we added in the COM register function
        /// </summary>
        /// <param name="t"></param>
        [ComUnregisterFunction]
        protected static void ComUnregister(Type t)
        {
            // Get registry key path
            var keyPath = string.Format(@"SOFTWARE\SolidWorks\AddIns\{0:b}", t.GUID);

            // Remove our registry entry
            Microsoft.Win32.Registry.LocalMachine.DeleteSubKeyTree(keyPath);

        }

        #endregion
    }
}
