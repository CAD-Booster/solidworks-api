using SolidWorks.Interop.sldworks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Hinge Mate feature data
    /// </summary>
    public class FeatureHingeMateData : SolidDnaObject<IHingeMateFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureHingeMateData(IHingeMateFeatureData model) : base(model)
        {

        }

        #endregion
    }
}