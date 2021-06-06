using SolidWorks.Interop.sldworks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Surface Ruled feature data
    /// </summary>
    public class FeatureSurfaceRuledData : SolidDnaObject<IRuledSurfaceFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureSurfaceRuledData(IRuledSurfaceFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
