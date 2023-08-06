using System;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// A separator for the command manager. Sits between items and or flyouts.
    /// We create separators by creating a command manager tab box, then add all items and flyouts to it.
    /// </summary>
    public class CommandManagerSeparator : ICommandManagerItem
    {
        /// <summary>
        /// A separator for the command manager. Sits between items and or flyouts.
        /// The defaults are fine unless you want to set the model types for which this separator is visible.
        /// </summary>
        public CommandManagerSeparator()
        {
            
        }

        /// <summary>
        /// True if the command should be added to the tab
        /// </summary>
        public bool AddToTab { get; set; } = true;

        /// <summary>
        /// The unique Callback ID (set by creator). Not used for separators.
        /// </summary>
        public string CallbackId { get; }

        /// <summary>
        /// The command Id for this flyout item. Not used for separators.
        /// </summary>
        public int CommandId { get; }

        /// <summary>
        /// The action to call when the item is clicked. Not used for separators.
        /// </summary>
        public Action OnClick { get; set; }

        /// <summary>
        /// The position of the item in the list. Not used for separators.
        /// </summary>
        public int Position { get; set; }
        
        /// <summary>
        /// The tab view style (whether and how to show in the large icon tab bar view).
        /// Separator is added when the value is anything but <see cref="CommandManagerItemTabView.None"/>.
        /// </summary>
        public CommandManagerItemTabView TabView { get; set; } = CommandManagerItemTabView.IconOnly;
        
        /// <summary>
        /// True to show this item in the command tab when an assembly is open
        /// </summary>
        public bool VisibleForAssemblies { get; set; } = true;

        /// <summary>
        /// True to show this item in the command tab when a drawing is open
        /// </summary>
        public bool VisibleForDrawings { get; set; } = true;

        /// <summary>
        /// True to show this item in the command tab when a part is open
        /// </summary>
        public bool VisibleForParts { get; set; } = true;

        public override string ToString() => "Separator";
    }
}
