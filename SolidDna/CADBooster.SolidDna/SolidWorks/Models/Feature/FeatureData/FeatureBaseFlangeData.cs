using SolidWorks.Interop.sldworks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Base Flange feature data
    /// </summary>
    public class FeatureBaseFlangeData : SolidDnaObject<IBaseFlangeFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureBaseFlangeData(IBaseFlangeFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
