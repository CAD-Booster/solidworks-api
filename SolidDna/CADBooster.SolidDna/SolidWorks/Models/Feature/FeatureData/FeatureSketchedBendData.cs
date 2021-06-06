using SolidWorks.Interop.sldworks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Sketched Bend feature data
    /// </summary>
    public class FeatureSketchedBendData : SolidDnaObject<ISketchedBendFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureSketchedBendData(ISketchedBendFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
