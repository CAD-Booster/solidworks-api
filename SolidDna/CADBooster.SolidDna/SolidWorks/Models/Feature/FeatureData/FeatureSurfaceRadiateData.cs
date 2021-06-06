using SolidWorks.Interop.sldworks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Surface Radiate feature data
    /// </summary>
    public class FeatureSurfaceRadiateData : SolidDnaObject<ISurfaceRadiateFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureSurfaceRadiateData(ISurfaceRadiateFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
