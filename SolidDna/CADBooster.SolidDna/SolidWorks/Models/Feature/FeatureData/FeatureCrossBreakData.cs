using SolidWorks.Interop.sldworks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Cross Break feature data
    /// </summary>
    public class FeatureCrossBreakData : SolidDnaObject<ICrossBreakFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureCrossBreakData(ICrossBreakFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
