using SolidWorks.Interop.sldworks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Hole Wizard feature data
    /// </summary>
    public class FeatureHoleWizardData : SolidDnaObject<IWizardHoleFeatureData2>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureHoleWizardData(IWizardHoleFeatureData2 model) : base(model)
        {

        }

        #endregion
    }
}
