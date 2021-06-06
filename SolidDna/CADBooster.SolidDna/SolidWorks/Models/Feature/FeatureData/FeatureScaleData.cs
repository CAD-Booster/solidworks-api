using SolidWorks.Interop.sldworks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Scale feature data
    /// </summary>
    public class FeatureScaleData : SolidDnaObject<IScaleFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureScaleData(IScaleFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
