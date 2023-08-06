using CADBooster.SolidDna.SolidWorks.CommandManager.Tab;
using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// A command group for the top command bar in SolidWorks
    /// </summary>
    public class CommandManagerGroup : SolidDnaObject<ICommandGroup>
    {
        /// <summary>
        /// Keeps track if this group has been created already
        /// </summary>
        private bool mCreated;

        /// <summary>
        /// A dictionary with all icon sizes and their paths.
        /// Entries are only added when path exists.
        /// </summary>
        private readonly Dictionary<int, string> mIconListPaths;

        /// <summary>
        /// A dictionary for the main group icon, with all icon sizes and their paths.
        /// Entries are only added when path exists.
        /// </summary>
        private readonly Dictionary<int, string> mMainIconPaths;

        /// <summary>
        /// A list of all tabs that have been created
        /// </summary>
        private readonly Dictionary<CommandManagerTabKey, CommandManagerTab> mTabs = new Dictionary<CommandManagerTabKey, CommandManagerTab>();

        /// <summary>
        /// The command items and flyouts to add to this group
        /// </summary>
        private List<ICommandManagerItem> Items { get; }

        /// <summary>
        /// Whether this command group has a Menu.
        /// NOTE: The menu is the regular drop-down menu like File, Edit, View etc...
        /// </summary>
        public bool HasMenu => BaseObject.HasMenu;

        /// <summary>
        /// Whether this command group has a Toolbar.
        /// NOTE: The toolbar is the small icons like the top-left SolidWorks menu New, Save, Open etc...
        /// </summary>
        public bool HasToolbar => BaseObject.HasToolbar;

        /// <summary>
        /// The type of documents to show this command group in as a menu
        /// </summary>
        public ModelTemplateType MenuVisibleInDocumentTypes => (ModelTemplateType)BaseObject.ShowInDocumentType;

        /// <summary>
        /// The tooltip of this command group
        /// </summary>
        public string Tooltip { get; }

        /// <summary>
        /// The Id used when this command group was created
        /// </summary>
        public int UserId { get; }

        /// <summary>
        /// Creates a command manager group with all its belonging information such as title, userID, hints, tooltips and icons.
        /// </summary>
        /// <param name="commandGroup">The SolidWorks command group</param>
        /// <param name="items">The command items to add</param>
        /// <param name="userId">The unique Id this group was assigned with when created</param>
        /// <param name="tooltip">The tool tip</param>
        /// <param name="hasMenu">Whether the CommandGroup should appear in the Tools dropdown menu.</param>
        /// <param name="hasToolbar">Whether the CommandGroup should appear in the Command Manager and as a separate toolbar.</param>
        /// <param name="documentTypes">The document types where this menu/toolbar is visible</param>
        /// <param name="iconListsPath">Absolute path to all icon sprites with including {0} for the image size</param>
        /// <param name="mainIconPath">Absolute path to all main icons with including {0} for the image size</param>
        public CommandManagerGroup(ICommandGroup commandGroup, List<ICommandManagerItem> items, int userId, string tooltip, bool hasMenu, bool hasToolbar,
                                   ModelTemplateType documentTypes, string iconListsPath, string mainIconPath) : base(commandGroup)
        {
            // Store user Id, used to remove the command group
            UserId = userId;

            // Set items
            Items = items;
            
            // Set tooltip
            Tooltip = tooltip;

            // Show for certain types of documents, or when no document is active.
            BaseObject.ShowInDocumentType = (int) documentTypes;

            // Have a menu
            BaseObject.HasMenu = hasMenu;

            // Have a toolbar
            BaseObject.HasToolbar = hasToolbar;

            // Set icon list
            mIconListPaths = Icons.GetFormattedPathDictionary(iconListsPath);

            // Set the main icon list
            mMainIconPaths = Icons.GetFormattedPathDictionary(mainIconPath);

            // Listen out for callbacks
            PlugInIntegration.CallbackFired += PlugInIntegration_CallbackFired;
        }

        /// <summary>
        /// Creates the command group based on its current children
        /// NOTE: Once created, parent command manager must remove and re-create the group
        /// This group cannot be re-used after creating, any edits will not take place
        /// </summary>
        /// <param name="manager">The command manager that is our owner</param>
        /// <param name="title"> </param>
        public void Create(CommandManager manager, string title)
        {
            if (mCreated)
                throw new SolidDnaException(SolidDnaErrors.CreateError(SolidDnaErrorTypeCode.SolidWorksCommandManager, SolidDnaErrorCode.SolidWorksCommandGroupReActivateError));

            // Set all relevant icon properties, depending on the solidworks version
            SetIcons();

            // Add items
            Items.ForEach(AddCommandItem);

            // Activate the command group
            mCreated = BaseObject.Activate();

            // Get the command ID that solidworks generated for each item
            Items.ForEach(SaveCommandId);

            // Add items that are visible for parts
            AddItemsToTabForModelType(manager, title, ModelType.Part);

            // Add items that are visible for assemblies
            AddItemsToTabForModelType(manager, title, ModelType.Assembly);

            // Add items that are visible for drawings
            AddItemsToTabForModelType(manager, title, ModelType.Drawing);

            // If we failed to create, throw
            if (!mCreated)
                throw new SolidDnaException(SolidDnaErrors.CreateError(SolidDnaErrorTypeCode.SolidWorksCommandManager, SolidDnaErrorCode.SolidWorksCommandGroupActivateError));
        }

        /// <summary>
        /// Get the command manager items for a model type.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="modelType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static List<ICommandManagerItem> GetItemsForModelType(List<ICommandManagerItem> items, ModelType modelType)
        {
            // Get the items that should be added to the tab
            var itemsForAllModelTypes = items.Where(f => f.AddToTab && f.TabView != CommandManagerItemTabView.None);

            // Return the items for this model type
            switch (modelType)
            {
                case ModelType.Part:     return itemsForAllModelTypes.Where(f => f.VisibleForParts).ToList();
                case ModelType.Assembly: return itemsForAllModelTypes.Where(f => f.VisibleForAssemblies).ToList();
                case ModelType.Drawing:  return itemsForAllModelTypes.Where(f => f.VisibleForDrawings).ToList();
                default:                 throw new ArgumentException("Invalid model type for command manager items");
            }
        }

        /// <summary>
        /// Adds a command item to the group
        /// </summary>
        /// <param name="commandManagerItem">The item to add</param>
        private void AddCommandItem(ICommandManagerItem commandManagerItem)
        {
            // Flyouts are already added to the command manager when you create them.
            // Separators are not added as items

            if (commandManagerItem is CommandManagerItem item)
            {
                // Add the item. We pass a preferred position for each item and receive the actual position back.
                var actualPosition = BaseObject.AddCommandItem2(item.Name, item.Position, item.Hint, item.Tooltip, item.ImageIndex,
                                                                $"{nameof(SolidAddIn.Callback)}({item.CallbackId})", null, UserId, (int)item.ItemType);

                // Store the actual ID / position we receive. If we have multiple items and, for example, set each position at the default -1, we receive sequential numbers, starting at 0.
                // Starts at zero for each command manager tab / ribbon.
                item.Position = actualPosition;
            }
        }

        /// <summary>
        /// Add all items and flyouts that are visible for the given model type to a tab.
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="title"> </param>
        /// <param name="modelType"></param>
        private void AddItemsToTabForModelType(CommandManager manager, string title, ModelType modelType)
        {
            // Get items for this model type
            var items = GetItemsForModelType(Items, modelType);

            // Split the items into a list of lists, split at the separator
            var itemsPerTabBox = GetSplitListsAtSeparator(items);

            // Get the tab
            var tab = GetNewOrExistingCommandManagerTab(modelType, manager, title);

            // Add each list to its own tab box
            foreach (var subItems in itemsPerTabBox)
                AddItemsToTab(tab, subItems);
        }

        /// <summary>
        /// Split a list of items at the separator. Returns a list of lists.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private static List<List<ICommandManagerItem>> GetSplitListsAtSeparator(List<ICommandManagerItem> items)
        {
            var currentList = new List<ICommandManagerItem>();
            var results = new List<List<ICommandManagerItem>> { currentList };  // Always add the first list

            // Loop through each item in the original list.
            foreach (var item in items)
            {
                // Check if we should start a new list because we found a separator
                if (item is CommandManagerSeparator)
                {
                    // Only create a new list if the current list is not empty
                    if (currentList.Count > 0)
                    {
                        // Start a new list
                        currentList = new List<ICommandManagerItem>();

                        // Add the newly created list to the results list
                        results.Add(currentList);
                    }
                }
                else
                {
                    // Only add items and flyouts to the current list
                    currentList.Add(item);
                }
            }

            return results;
        }

        /// <summary>
        /// Adds all items to the command tab.
        /// </summary>
        /// <param name="tab"></param>
        /// <param name="items">Items to add</param>
        private static void AddItemsToTab(CommandManagerTab tab, List<ICommandManagerItem> items)
        {
            // Get the ID and style for each item and flyout
            var tabItemData = GetTabItemData(items);

            // If there are items to add, do something.. 
            if (tabItemData.Count <= 0) return;

            // Create new tab box
            var tabBox = tab.UnsafeObject.AddCommandTabBox() ?? throw new SolidDnaException(SolidDnaErrors.CreateError(
                SolidDnaErrorTypeCode.SolidWorksCommandManager,
                SolidDnaErrorCode.SolidWorksCommandGroupCreateTabBoxError));

            // Add the new tab box to list of tab boxes.
            tab.TabBoxes.Add(new CommandManagerTabBox(tabBox));

            // Convert the list of TabData to arrays of ids and styles
            var ids = tabItemData.Select(tabData => tabData.Id).ToArray();
            var styles = tabItemData.Select(tabData => (int)tabData.Style).ToArray();

            tabBox.AddCommands(ids, styles);
        }

        /// <summary>
        /// Get a list of <see cref="TabItemData"/> for the given list of <see cref="ICommandManagerItem"/>. Only adds items and flyouts.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private static List<TabItemData> GetTabItemData(List<ICommandManagerItem> items)
        {
            // Initiate new list of values
            var tabItemData = new List<TabItemData>();

            // Add each id and style to the list of tab data 
            foreach (var commandManagerItem in items)
            {
                switch (commandManagerItem)
                {
                    case CommandManagerFlyout flyout:
                    {
                        // Add flyout data. We add a style enum to flyouts.
                        tabItemData.Add(new TabItemData(flyout));
                        break;
                    }
                    case CommandManagerItem item:
                    {
                        // Add item data.
                        tabItemData.Add(new TabItemData(item));
                        break;
                    }
                }
            }

            return tabItemData;
        }

        /// <summary>
        /// Check <see cref="mTabs"/> for an existing tab with the given title and model type. If it doesn't exist, create a new command manager tab.
        /// </summary>
        /// <param name="modelType"></param>
        /// <param name="manager"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        private CommandManagerTab GetNewOrExistingCommandManagerTab(ModelType modelType, CommandManager manager, string title)
        {
            // Get the tab if it already exists
            var existingTab = mTabs.FirstOrDefault(f => f.Key.Title.Equals(title) && f.Key.ModelType == modelType).Value;
            if (existingTab != null)
            {
                // Return the existing tab
                return existingTab;
            }

            // Otherwise create it
            var tab = manager.GetCommandTab(modelType, title);

            // Keep track of this tab
            mTabs.Add(new CommandManagerTabKey(title, modelType), tab);

            // Return the new tab
            return tab;
        }

        /// <summary>
        /// Fired when a SolidWorks callback is fired
        /// </summary>
        /// <param name="name">The name of the callback that was fired</param>
        private void PlugInIntegration_CallbackFired(string name)
        {
            // Find the item, if any
            var item = Items.FirstOrDefault(f => f.CallbackId == name);

            // Call the action
            item?.OnClick?.Invoke();
        }

        /// <summary>
        /// Get the command ID that solidworks generated from the command group and save it to the item.
        /// </summary>
        /// <param name="item"></param>
        private void SaveCommandId(ICommandManagerItem item)
        {
            //This is only necessary for CommandManagerItems, not for flyouts or separators.
            if (item is CommandManagerItem commandManagerItem)
                commandManagerItem.CommandId = BaseObject.CommandID[item.Position];
        }

        /// <summary>
        /// Set the icon list properties on the base object.
        /// NOTE: The order in which you specify the icons must be the same for this property and MainIconList.
        /// For example, if you specify an array of paths to 20 x 20 pixels, 32 x 32 pixels, and 40 x 40 pixels icons for this property, 
        /// then you must specify an array of paths to 20 x 20 pixels, 32 x 32 pixels, and 40 x 40 pixels icons for MainIconList.
        /// </summary>
        private void SetIcons()
        {
            // If we set all properties, the wrong image sizes appear in the Customize window. So we check the SolidWorks version first.
            if (SolidWorksEnvironment.Application.SolidWorksVersion.Version >= 2016)
            {
                // The list of icons for the toolbar or menu. There should be a sprite (a combination of all icons) for each icon size.
                BaseObject.IconList = Icons.GetArrayFromDictionary(mIconListPaths);

                // The icon that is visible in the Customize window 
                BaseObject.MainIconList = Icons.GetArrayFromDictionary(mMainIconPaths);
            }
            else
            {
                var icons = Icons.GetArrayFromDictionary(mIconListPaths);
                if (icons.Length <= 0) return;

                // Largest icon for this one
                BaseObject.LargeIconList = icons.Last();

                // The list of icons
                BaseObject.MainIconList = icons;

                // Use largest icon still (otherwise command groups are always small icons)
                BaseObject.SmallIconList = icons.Last();
            }
        }

        /// <summary>
        /// Returns a user-friendly string with group properties.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"Group with {Items.Count} items. Has menu: {HasMenu}. Has toolbar: {HasToolbar}";

        /// <summary>
        /// Unsubscribe from callbacks and safely dispose the current '<see cref="CommandManagerGroup"/>'-object
        /// </summary>
        public override void Dispose()
        {
            // Stop listening out for callbacks
            PlugInIntegration.CallbackFired -= PlugInIntegration_CallbackFired;

            // Dispose all tabs
            foreach (var tab in mTabs.Values)
                tab.Dispose();

            base.Dispose();
        }
    }
}