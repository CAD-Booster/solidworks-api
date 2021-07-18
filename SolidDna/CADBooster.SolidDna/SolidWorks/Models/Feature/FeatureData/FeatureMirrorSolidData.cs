using SolidWorks.Interop.sldworks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Mirror Solid feature data
    /// </summary>
    public class FeatureMirrorSolidData : SolidDnaObject<IMirrorSolidFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureMirrorSolidData(IMirrorSolidFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
