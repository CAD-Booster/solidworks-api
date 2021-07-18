using SolidWorks.Interop.sldworks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Circular Pattern feature data
    /// </summary>
    public class FeatureCircularPatternData : SolidDnaObject<ICircularPatternFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureCircularPatternData(ICircularPatternFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
