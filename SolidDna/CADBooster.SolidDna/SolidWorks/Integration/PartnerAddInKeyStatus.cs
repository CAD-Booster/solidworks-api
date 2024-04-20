using SolidWorks.Interop.swconst;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// The return value for the <see cref="SolidAddIn.IdentifyToSW"/> call to SolidWorks to verify our add-in partner status.
    /// From <see cref="swPartnerEntitlementStatus_e"/>.
    /// </summary>
    public enum PartnerAddInKeyStatus
    {
        /// <summary>
        /// Not an official value in <see cref="swPartnerEntitlementStatus_e"/>.
        /// The length of <see cref="SolidAddIn.SolidWorksAddInPartnerLicenseKey"/> should be exactly 128 characters long (or an empty string when you are not a partner).
        /// SolidWorks throws an exception when this happens and your add-in will not load.
        /// </summary>
        IncorrectPartnerLicenseKeyLength = -1,

        /// <summary>
        /// Succeeded 
        /// </summary>
        Success = 0,

        /// <summary>
        /// Failed because no add-in key was specified or because of an unspecified reason.
        /// </summary>
        Fail = 1,

        /// <summary>
        /// Failed because the add-in name does not match the add-in key request form.
        /// </summary>
        AddinNameMismatch = 2,

        /// <summary>
        /// Failed because the add-in GUID does not match the add-in key request form.
        /// </summary>
        AddinGuidMismatch = 4,

        /// <summary>
        /// Failed because the SolidWorks version does not match the add-in key request form.
        /// </summary>
        VersionMismatch = 8,

        /// <summary>
        /// Failed because the license key is no longer valid.
        /// </summary>
        LicenseExpired = 16,

        /// <summary>
        /// Failed because the partner status for this add-in has changed.
        /// </summary>
        TierMismatch = 32
    }
}
