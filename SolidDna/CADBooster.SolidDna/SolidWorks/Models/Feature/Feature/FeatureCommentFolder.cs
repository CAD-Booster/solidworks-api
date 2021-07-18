using SolidWorks.Interop.sldworks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Comment Folder feature
    /// </summary>
    public class FeatureCommentFolder : SolidDnaObject<ICommentFolder>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureCommentFolder(ICommentFolder model) : base(model)
        {

        }

        #endregion
    }
}
