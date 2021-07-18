using SolidWorks.Interop.sldworks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks model configuration
    /// </summary>
    public class ModelConfiguration : SolidDnaObject<Configuration>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ModelConfiguration(Configuration model) : base(model)
        {

        }

        #endregion
    }
}
