using SolidWorks.Interop.sldworks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Loft feature data
    /// </summary>
    public class FeatureLoftData : SolidDnaObject<ILoftFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureLoftData(ILoftFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
