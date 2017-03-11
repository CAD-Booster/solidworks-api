﻿namespace AngelSix.SolidDna
{
    /// <summary>
    /// A command item for the top command bar in SolidWorks
    /// </summary>
    public class CommandManagerItem
    {
        #region Public Properties

        /// <summary>
        /// The unique Id of this item (set by the parent)
        /// </summary>
        public int UniqueId { get; set; }

        /// <summary>
        /// The command Id of this item (set by the parent)
        /// </summary>
        public int CommandId { get; set; }

        /// <summary>
        /// The name of the item
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The position of the item in the list.
        /// NOTE: Specify 0 to add the CommandGroup to the beginning of the CommandMananger, or specify -1 to add it to the end of the CommandManager.
        /// </summary>
        public int Position { get; set; } = -1;

        /// <summary>
        /// The hint to show for this item
        /// </summary>
        public string Hint { get; set; }

        /// <summary>
        /// The tooltip to show for this item
        /// </summary>
        public string Tooltip { get; set; }

        /// <summary>
        /// The index position of the image to use for this item from the parent image list (zero-index)
        /// </summary>
        public int ImageIndex { get; set; }

        /// <summary>
        /// The type of item this is, such as a menu item or a toolbar item or both
        /// </summary>
        public CommandItemType ItemType { get; set; } = CommandItemType.MenuItem | CommandItemType.ToolbarItem;

        /// <summary>
        /// The tab view style (whether and how to show in the large icon tab bar view)
        /// </summary>
        public CommandManagerItemTabView TabView { get; set; } = CommandManagerItemTabView.IconWithTextBelow;

        /// <summary>
        /// True to show this item in tha command tab when a part is open
        /// </summary>
        public bool VisibleForParts { get; set; } = true;

        /// <summary>
        /// True to show this item in tha command tab when an assembly is open
        /// </summary>
        public bool VisibleForAssemblies { get; set; } = true;

        /// <summary>
        /// True to show this item in tha command tab when a drawing is open
        /// </summary>
        public bool VisibleForDrawings { get; set; } = true;

        #endregion

        #region Public Callbacks



        #endregion
    }
}