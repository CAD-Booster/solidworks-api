namespace CADBooster.SolidDna
{
    /// <summary>
    /// The type of SolidWorks application that is currently running.
    /// Same values as <see cref="SolidWorks.Interop.swconst.swApplicationType_e"/>.
    /// </summary>
    public enum SolidWorksApplicationType
    {
        /// <summary>
        /// The standard SolidWorks desktop application
        /// </summary>
        Desktop = 0,

        /// <summary>
        /// 3DEXPERIENCE SolidWorks, also known as SolidWorks Connected. Was introduced in SolidWorks 2020.
        /// </summary>
        ThreeDExperienceSolidWorks = 1,

        /// <summary>
        /// SolidWorks Desktop with the 3DEXPERIENCE connector add-in enabled. Was introduced in SolidWorks 2023.
        /// </summary>
        DesktopWithConnectorAddIn,
    }
}
