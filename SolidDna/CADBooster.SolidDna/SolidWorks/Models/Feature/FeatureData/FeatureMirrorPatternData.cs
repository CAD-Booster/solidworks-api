using SolidWorks.Interop.sldworks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Mirror Pattern feature data
    /// </summary>
    public class FeatureMirrorPatternData : SolidDnaObject<IMirrorPatternFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureMirrorPatternData(IMirrorPatternFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
