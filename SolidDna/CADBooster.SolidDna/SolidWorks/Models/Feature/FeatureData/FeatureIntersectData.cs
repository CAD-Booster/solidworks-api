using SolidWorks.Interop.sldworks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Intersect feature data
    /// </summary>
    public class FeatureIntersectData : SolidDnaObject<IIntersectFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureIntersectData(IIntersectFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
