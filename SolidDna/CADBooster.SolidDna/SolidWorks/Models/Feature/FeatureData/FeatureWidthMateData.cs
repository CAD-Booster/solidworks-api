using SolidWorks.Interop.sldworks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Width Mate feature data
    /// </summary>
    public class FeatureWidthMateData : SolidDnaObject<IWidthMateFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureWidthMateData(IWidthMateFeatureData model) : base(model)
        {

        }

        #endregion
    }
}