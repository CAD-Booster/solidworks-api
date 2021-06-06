using SolidWorks.Interop.sldworks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Dome feature data
    /// </summary>
    public class FeatureDomeData : SolidDnaObject<IDomeFeatureData2>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureDomeData(IDomeFeatureData2 model) : base(model)
        {

        }

        #endregion
    }
}
