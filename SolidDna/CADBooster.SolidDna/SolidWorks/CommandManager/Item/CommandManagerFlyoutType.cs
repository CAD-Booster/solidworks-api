using SolidWorks.Interop.swconst;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Types of command manager flyouts. Has the same values as <see cref="swCommandFlyoutStyle_e"/>.
    /// </summary>
    public enum CommandManagerFlyoutType
    {
        /// <summary>
        /// No command is executed when you click the flyout. It only expands.
        /// </summary>
        ExpandOnly = 0,

        /// <summary>
        /// The first item is always visible. If you click the icon, it executes the first item.
        /// If you click the arrow, the flyout expands.
        /// </summary>
        FirstItemVisible = 1,

        /// <summary>
        /// The item you clicked last is visible. If you click the icon, it executes that item.
        /// If you click the arrow, the flyout expands.
        /// </summary>
        LastUsedItemVisible = 2
    }
}
