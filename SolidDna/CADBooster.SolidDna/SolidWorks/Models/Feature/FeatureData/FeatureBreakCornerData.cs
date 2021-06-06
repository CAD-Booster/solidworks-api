using SolidWorks.Interop.sldworks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Break Corner feature data
    /// </summary>
    public class FeatureBreakCornerData : SolidDnaObject<IBreakCornerFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureBreakCornerData(IBreakCornerFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
