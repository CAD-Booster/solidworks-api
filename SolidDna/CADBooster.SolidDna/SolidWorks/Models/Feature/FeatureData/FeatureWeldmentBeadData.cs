using SolidWorks.Interop.sldworks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Weldment Bead feature data
    /// </summary>
    public class FeatureWeldmentBeadData : SolidDnaObject<IWeldmentBeadFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureWeldmentBeadData(IWeldmentBeadFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
