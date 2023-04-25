using SolidWorks.Interop.swconst;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Types of configurations. Copy of <see cref="swConfigurationType_e"/>
    /// </summary>
    public enum ConfigurationTypes
    {
        /// <summary>
        /// A normal configuration
        /// </summary>
        Standard = 0,

        /// <summary>
        /// A top-level weldment configuration
        /// </summary>
        AsMachined = 1,

        /// <summary>
        /// A derived weldment configuration
        /// </summary>
        AsWelded = 2,

        /// <summary>
        /// A flat pattern configuration
        /// </summary>
        SheetMetal = 3,

        /// <summary>
        /// A derived SpeedPak configuration
        /// </summary>
        SpeedPak = 4,
    }
}
