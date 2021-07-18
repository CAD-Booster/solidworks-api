using SolidWorks.Interop.sldworks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Combine Bodies feature data
    /// </summary>
    public class FeatureCombineBodiesData : SolidDnaObject<ICombineBodiesFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureCombineBodiesData(ICombineBodiesFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
