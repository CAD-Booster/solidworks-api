using SolidWorks.Interop.sldworks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Edge Flange feature data
    /// </summary>
    public class FeatureEdgeFlangeData : SolidDnaObject<IEdgeFlangeFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureEdgeFlangeData(IEdgeFlangeFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
