using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// A command flyout for the top command bar in SolidWorks
    /// </summary>
    public class CommandManagerFlyout : SolidDnaObject<IFlyoutGroup>, ICommandManagerItem
    {
        /// <summary>
        /// Flyouts only work when you add the items after clicking the flyout, SolidWorks calls them 'dynamic flyouts' in the help.
        /// It seems to work well when we only do this on the first click.
        /// </summary>
        private bool _addedItemsAfterFirstClick;

        #region Public Properties

        /// <summary>
        /// True if the command should be added to the tab
        /// </summary>
        public bool AddToTab { get; set; } = true;

        /// <summary>
        /// The ID used when this command flyout was created
        /// </summary>
        public int UserId { get; }

        /// <summary>
        /// The unique Callback ID (set by creator)
        /// </summary>
        public string CallbackId { get; }

        /// <summary>
        /// The title of this command group
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The hint of this command group
        /// </summary>
        public string Hint { get; set; }

        /// <summary>
        /// The tooltip of this command group
        /// </summary>
        public string Tooltip { get; set; }

        /// <summary>
        /// The command items to add to this flyout
        /// </summary>
        public List<CommandManagerItem> Items { get; set; }

        /// <summary>
        /// The command ID for this flyout item
        /// </summary>
        public int CommandId => BaseObject.CmdID;

        /// <summary>
        /// The position of the item in the list. Not used for flyouts.
        /// </summary>
        public int Position { get; set; } = -1;

        /// <summary>
        /// The tab view style (whether and how to show in the large icon tab bar view)
        /// </summary>
        public CommandManagerItemTabView TabView { get; set; }

        /// <summary>
        /// Defines the look and behavior of the flyout. By default, it shows the main icon of the flyout. When you click on it, the flyout expands and does not execute a command.
        /// Other options: always show the first item, show the last-used item.
        /// </summary>
        public CommandManagerFlyoutType Type { get; set; }

        /// <summary>
        /// True to show this item in the command tab when a part is open
        /// </summary>
        public bool VisibleForParts { get; set; } = true;

        /// <summary>
        /// True to show this item in the command tab when an assembly is open
        /// </summary>
        public bool VisibleForAssemblies { get; set; } = true;

        /// <summary>
        /// True to show this item in the command tab when a drawing is open
        /// </summary>
        public bool VisibleForDrawings { get; set; } = true;

        /// <summary>
        /// The action to call when the item is clicked
        /// </summary>
        public Action OnClick { get; set; }

        #endregion

        /// <summary>
        /// Create a command manager flyout (group).
        /// </summary>
        /// <param name="flyoutGroup">The SolidWorks command manager flyout group</param>
        /// <param name="userId">The unique flyout ID</param>
        /// <param name="callbackId">The unique callback ID</param>
        /// <param name="items">The command items to add</param>
        /// <param name="title">The title</param>
        /// <param name="hint">The hint</param>
        /// <param name="tooltip">The tool tip</param>
        /// <param name="tabView"> </param>
        /// <param name="type"> </param>
        public CommandManagerFlyout(IFlyoutGroup flyoutGroup, int userId, string callbackId, List<CommandManagerItem> items, string title, string hint, string tooltip, 
                                    CommandManagerItemTabView tabView, CommandManagerFlyoutType type) : base(flyoutGroup)
        {
            // Set user ID
            UserId = userId;

            // Callback ID
            CallbackId = callbackId;

            // Set items
            Items = items;

            // Set title
            Title = title;

            // Set hint
            Hint = hint;

            // Set tooltip
            Tooltip = tooltip;

            // Set tab view / style
            TabView = tabView;

            // Set type / visible item
            Type = type;

            // Listen out for callbacks
            PlugInIntegration.CallbackFired += PlugInIntegration_CallbackFired;

            // Add the items when the flyout is clicked for the first time. Does not work when you add items right away.
            OnClick = AddCommandItems;

            // NOTE: No need to set items command IDs as they are only needed when calling AddItemToTab
            //       and the flyout itself gets added, not the flyouts inner commands
        }

        /// <summary>
        /// Remove, then re-add all items to the flyout.
        /// Is called on every click of the flyout, but only does something on the first click.
        /// SolidWorks calls this a 'dynamic flyout' in the help.
        /// </summary>
        private void AddCommandItems()
        {
            // Only add items once
            if (_addedItemsAfterFirstClick)
                return;

            // Clear all existing items / buttons first.
            BaseObject.RemoveAllCommandItems();

            // Add items
            Items?.ForEach(AddCommandItem);

            // Set the flyout type
            BaseObject.FlyoutType = (int)Type;

            // Set flag
            _addedItemsAfterFirstClick = true;
        }

        /// <summary>
        /// Adds a command item to the flyout.
        /// </summary>
        /// <param name="item">The item to add</param>
        private void AddCommandItem(CommandManagerItem item)
        {
            // Add the item and receive the actual position.
            var position = BaseObject.AddCommandItem(item.Name, item.Hint, item.ImageIndex, $"{nameof(SolidAddIn.Callback)}({item.CallbackId})", null);

            // If the returned position is -1, the item was not added.
            if (position == -1)
                throw new SolidDnaException(SolidDnaErrors.CreateError(SolidDnaErrorTypeCode.SolidWorksCommandManager,
                    SolidDnaErrorCode.SolidWorksCommandFlyoutPositionError, "Can be caused by setting the image indexes wrong."));
        }

        /// <summary>
        /// Fired when a SolidWorks callback is fired
        /// </summary>
        /// <param name="name">The name of the callback that was fired</param>
        private void PlugInIntegration_CallbackFired(string name)
        {
            // Find the item, if any
            var item = Items?.FirstOrDefault(f => f.CallbackId == name);

            // Call the action
            item?.OnClick?.Invoke();
        }

        /// <summary>
        /// Disposing
        /// </summary>
        public override void Dispose()
        {
            // Stop listening out for callbacks
            PlugInIntegration.CallbackFired -= PlugInIntegration_CallbackFired;

            base.Dispose();
        }

        public override string ToString() => $"Flyout with name: {Tooltip}. CommandID: {CommandId}. Position: {Position}. Hint: {Hint}.";
    }
}
