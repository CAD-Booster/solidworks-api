using SolidWorks.Interop.swconst;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Whether a model was created in the desktop version of SolidWorks, in 
    /// Same values as <see cref="sw3DExperienceModelType_e"/>.
    /// </summary>
    public enum ModelType3DExperience
    {
        /// <summary>
        /// This is a standard model that was created in SolidWorks Desktop.
        /// </summary>
        Standard = 0,

        /// <summary>
        /// This model comes from PartSupply. 
        /// </summary>
        PartSupply = 1,

        /// <summary>
        /// This model comes from 3DExperience.
        /// </summary>
        ThreeDExperience = 2,

        /// <summary>
        /// This is a multi-CAD model from xCAD.
        /// </summary>
        XCad = 3,
    }
}
