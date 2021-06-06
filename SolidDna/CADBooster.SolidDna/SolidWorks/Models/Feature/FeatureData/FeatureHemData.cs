using SolidWorks.Interop.sldworks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Hem feature data
    /// </summary>
    public class FeatureHemData : SolidDnaObject<IHemFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureHemData(IHemFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
