using SolidWorks.Interop.sldworks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Feature folder feature
    /// </summary>
    public class FeatureFeatureFolder : SolidDnaObject<IFeatureFolder>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureFeatureFolder(IFeatureFolder model) : base(model)
        {

        }

        #endregion
    }
}
