using SolidWorks.Interop.sldworks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Surface Cut feature data
    /// </summary>
    public class FeatureSurfaceCutData : SolidDnaObject<ISurfaceCutFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureSurfaceCutData(ISurfaceCutFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
