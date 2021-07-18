using SolidWorks.Interop.sldworks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Jog feature data
    /// </summary>
    public class FeatureJogData : SolidDnaObject<IJogFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureJogData(IJogFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
