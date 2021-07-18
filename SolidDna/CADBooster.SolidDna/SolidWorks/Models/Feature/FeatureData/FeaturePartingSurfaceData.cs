using SolidWorks.Interop.sldworks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Parting Surface feature data
    /// </summary>
    public class FeaturePartingSurfaceData : SolidDnaObject<IPartingSurfaceFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeaturePartingSurfaceData(IPartingSurfaceFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
