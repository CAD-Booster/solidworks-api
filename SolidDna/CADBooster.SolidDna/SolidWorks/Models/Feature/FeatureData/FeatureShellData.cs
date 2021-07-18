using SolidWorks.Interop.sldworks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Shell feature data
    /// </summary>
    public class FeatureShellData : SolidDnaObject<IShellFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureShellData(IShellFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
