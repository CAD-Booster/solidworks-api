using SolidWorks.Interop.swconst;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// The return value for the <see cref="PartnerProductAddIn.IdentifyToSW"/> call to SolidWorks to verify our add-in partner status.
    /// Same values as <see cref="swPartnerEntitlementStatus_e"/>, but with an additional value for incorrect license key length.
    /// </summary>
    public enum PartnerAddInKeyStatus
    {
        /// <summary>
        /// Not an official value in <see cref="swPartnerEntitlementStatus_e"/>.
        /// The length of <see cref="PartnerProductAddIn.SolidWorksAddInPartnerLicenseKey"/> should be exactly 128 characters long.
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
        AddInNameMismatch = 2,

        /// <summary>
        /// Failed because the add-in GUID does not match the add-in key request form.
        /// </summary>
        AddInGuidMismatch = 4,

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
