using SolidWorks.Interop.sldworks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Dr Section feature
    /// </summary>
    public class FeatureDrSection : SolidDnaObject<IDrSection>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureDrSection(IDrSection model) : base(model)
        {

        }

        #endregion
    }
}
