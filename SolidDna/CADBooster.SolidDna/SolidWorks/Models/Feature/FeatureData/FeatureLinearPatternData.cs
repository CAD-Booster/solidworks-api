using SolidWorks.Interop.sldworks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Linear Pattern feature data
    /// </summary>
    public class FeatureLinearPatternData : SolidDnaObject<ILinearPatternFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureLinearPatternData(ILinearPatternFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
