using SolidWorks.Interop.sldworks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Smart Component feature data
    /// </summary>
    public class FeatureSmartComponentData : SolidDnaObject<ISmartComponentFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureSmartComponentData(ISmartComponentFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
