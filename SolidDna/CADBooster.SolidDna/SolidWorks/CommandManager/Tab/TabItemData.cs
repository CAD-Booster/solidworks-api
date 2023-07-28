using SolidWorks.Interop.swconst;

namespace CADBooster.SolidDna.SolidWorks.CommandManager.Tab
{
    /// <summary>
    /// A read-only struct to represent data for a tab item (either a CommandManagerItem or CommandManagerFlyout)
    /// </summary>
    public readonly struct TabItemData
    {
        /// <summary>
        /// Initializes the TabItemData struct with data from a CommandManagerItem object and gives the ID and Style back.
        /// </summary>
        /// <param name="item"></param>
        public TabItemData(CommandManagerItem item)
        {
            Id = item.CommandId;
            Style = (CommandManagerFlyoutStyle)item.TabView;
        }

        /// <summary>
        /// Initializes the TabItemData struct with data from a CommandManagerFlyout object and gives the ID and Style back.
        /// </summary>
        /// <param name="flyout"></param>
        public TabItemData(CommandManagerFlyout flyout)
        {
            Id = flyout.CommandId;
            Style = (CommandManagerFlyoutStyle)flyout.TabView | CommandManagerFlyoutStyle.ActionFlyout;
        }

        /// <summary>
        /// Stores the CommandId of the item or flyout.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Combines the two integers from <see cref="swCommandTabButtonFlyoutStyle_e"/> and <see cref="CommandManagerItemTabView"/> into one.
        /// </summary>
        public CommandManagerFlyoutStyle Style { get; }
    }
}
