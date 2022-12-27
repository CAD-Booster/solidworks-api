using SolidWorks.Interop.swconst;
using System;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Represents option flags for creating a new configuration.
    /// Copy of <see cref="swConfigurationOptions2_e"/>.
    /// </summary>
    [Flags]
    public enum ModelConfigurationNewOptions
    {
        /// <summary>
        /// Use an alternate name 
        /// </summary>
        UseAlternateName = 1,

        /// <summary>
        /// Set to show sub assemblies in the bill of materials. If unset, the BOM shows child components only.
        /// </summary>
        ShowSubAssembliesInBom = 2,

        /// <summary>
        /// Suppress all newly added features and mates in this configuration by default.
        /// </summary>
        SuppressNewFeaturesAndMates = 4,

        /// <summary>
        /// Hide all newly added components in this configuration by default.
        /// </summary>
        HideNewComponents = 8,

        /// <summary>
        /// Suppress all newly added components in this configuration by default.
        /// </summary>
        SuppressNewComponents = 16,
        
        /// <summary>
        /// Do not use
        /// </summary>
        [Obsolete]
        InheritProperties = 32,

        /// <summary>
        /// Link this configuration to a parent configuration so that its parent part name is shown in a BOM.
        /// </summary>
        ShowParentPartNameInBom = 64,

        /// <summary>
        /// By default, a newly added configuration is activated. Add this setting to avoid that.
        /// </summary>
        DoNotActivate = 128,

        /// <summary>
        /// Dissolve the configuration in the BOM and promote all of its child components.
        /// </summary>
        PromoteInBOM = 256,

        /// <summary>
        /// Show the configuration description in the Description column of a BOM, instead of the custom property Description.
        /// </summary>
        UseDescriptionInBOM = 512,
    }
}
