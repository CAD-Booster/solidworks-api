using SolidWorks.Interop.sldworks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Sensor feature
    /// </summary>
    public class FeatureSensor : SolidDnaObject<ISensor>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureSensor(ISensor model) : base(model)
        {

        }

        #endregion
    }
}
