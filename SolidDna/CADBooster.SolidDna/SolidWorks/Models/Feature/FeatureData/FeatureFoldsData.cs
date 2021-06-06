using SolidWorks.Interop.sldworks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Folds feature data
    /// </summary>
    public class FeatureFoldsData : SolidDnaObject<IFoldsFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureFoldsData(IFoldsFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
