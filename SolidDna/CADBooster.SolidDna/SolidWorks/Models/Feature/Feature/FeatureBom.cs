using SolidWorks.Interop.sldworks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Bom feature
    /// </summary>
    public class FeatureBom : SolidDnaObject<IBomFeature>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureBom(IBomFeature model) : base(model)
        {

        }

        #endregion
    }
}
