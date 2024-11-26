using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swpublished;
using System;
using System.Windows;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Integrates into SolidWorks as an official partner product add-in and registers for callbacks provided by SolidWorks.
    /// Implements <see cref="ISwPEManager"/> to verify the add-in partner status on startup.
    /// Implement <see cref="SolidAddIn"/> instead of this class if you are not a SolidWorks partner product.
    /// An add-in will not start up in 3DEXPERIENCE SolidWorks (=SolidWorks Connected) if you implement this class but do not have a valid partner license key for the currently active SolidWorks version year.
    /// 
    /// IMPORTANT: The class that overrides <see cref="ISwAddin"/> MUST be the same class that 
    /// contains the ComRegister and ComUnregister functions due to how SolidWorks loads add-ins
    /// </summary>
    internal abstract class PartnerProductAddIn : SolidAddIn, ISwPEManager
    {
        #region Public Properties

        /// <summary>
        /// The Partner Product license key for this SolidWorks add-in.
        /// Enter your key here if your add-in is a registered partner product for SolidWorks 2021 or newer. Make sure the key is valid for the current SolidWorks version.
        /// Set this value in your add-in constructor because the <see cref="IdentifyToSW"/> event is fired before <see cref="SolidAddIn.ConnectToSW"/>.
        /// Let it be an empty string if your add-in is not a registered partner product.
        /// If the key is valid, the add-in appears under the group 'Partner Gold Add-ins' or 'Partner Solution Add-ins'.
        /// If the key is empty or not valid, the add-in appears under the group 'Other Add-ins'.
        /// If the key length is not 128 characters, SolidWorks throws an exception and your add-in will not load.
        /// So we catch that exception and set the status to <see cref="PartnerAddInKeyStatus.IncorrectPartnerLicenseKeyLength"/>.
        /// More info: <see href="https://help.solidworks.com/2024/english/api/sldworksapiprogguide/GettingStarted/SolidWorks_Partner_Program_2.htm" />
        /// </summary>
        public string SolidWorksAddInPartnerLicenseKey { get; set; } = "";

        #endregion

        #region Public Abstract / Virtual Methods

        /// <summary>
        /// Called after SolidWorks has determined if this add-in has a valid Partner Program product key.
        /// Check this status if your add-in is not starting up.
        /// Runs directly after the <see cref="SolidAddIn"/> constructor and even before <see cref="SolidAddIn.ConnectToSW"/>.
        /// See <see cref="SolidWorksAddInPartnerLicenseKey"/> for more info.
        /// </summary>
        public abstract void PartnerProductStatusSet(PartnerAddInKeyStatus status);

        #endregion

        /// <summary>
        /// Called when SolidWorks tries to determine if this add-in is registered with the Partner Program.
        /// This method is called before <see cref="SolidAddIn.ConnectToSW"/>.
        /// See <see cref="SolidWorksAddInPartnerLicenseKey"/>.
        /// </summary>
        /// <param name="classFactory"></param>
        public void IdentifyToSW(object classFactory)
        {
            if (!(classFactory is ISwPEClassFactory factory))
                return;

            PartnerAddInKeyStatus status;
            if (SolidWorksAddInPartnerLicenseKey.IsNullOrEmpty())
            {
                // If your add-in is not a SolidWorks Partner product, do not leave the partner key empty. Instead, implement SolidAddIn instead of PartnerProductAddIn.
                // If we pass an empty string, the status will be PartnerAddInKeyStatus.Fail.
                // The add-in will load correctly in the desktop version of SolidWorks, but it will not load in 3DEXPERIENCE SolidWorks.
                status = IdentifyAddInToSolidWorks(factory, "");
            }
            else if (SolidWorksAddInPartnerLicenseKey.Length != 128)
            {
                // The length of the partner license key should be exactly 128 characters long.
                // SolidWorks throws an exception when you pass a string of the incorrect length and your add-in will not load.
                // But we have to call ISwPEClassFactory.SetPartnerKey or the add-in will not load either, so we pass an empty string and ignore the return value.
                IdentifyAddInToSolidWorks(factory, "");
                status = PartnerAddInKeyStatus.IncorrectPartnerLicenseKeyLength;
            }
            else
            {
                // Register our add-in as a partner product.
                status = IdentifyAddInToSolidWorks(factory, SolidWorksAddInPartnerLicenseKey);
            }

            // Inform listeners
            PartnerProductStatusSet(status);
        }

        /// <summary>
        /// Register our add-in as a partner product. 
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static PartnerAddInKeyStatus IdentifyAddInToSolidWorks(ISwPEClassFactory factory, string key)
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
    }
}
