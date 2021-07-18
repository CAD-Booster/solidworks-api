using SolidWorks.Interop.sldworks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Surface Offset feature data
    /// </summary>
    public class FeatureSurfaceOffsetData : SolidDnaObject<ISurfaceOffsetFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureSurfaceOffsetData(ISurfaceOffsetFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
