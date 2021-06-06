using SolidWorks.Interop.sldworks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Sketch Pattern feature data
    /// </summary>
    public class FeatureSketchPatternData : SolidDnaObject<ISketchPatternFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureSketchPatternData(ISketchPatternFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
