using SolidWorks.Interop.sldworks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Flat Pattern Folder feature
    /// </summary>
    public class FeatureFlatPatternFolder : SolidDnaObject<IFlatPatternFolder>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureFlatPatternFolder(IFlatPatternFolder model) : base(model)
        {

        }

        #endregion
    }
}