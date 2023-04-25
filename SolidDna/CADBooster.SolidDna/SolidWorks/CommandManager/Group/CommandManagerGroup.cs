using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
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
        #region Private Members

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

        #endregion

        #region Public Properties

        /// <summary>
        /// The Id used when this command group was created
        /// </summary>
        public int UserId { get; }

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
        /// If true, adds a command box to the toolbar for parts that has a dropdown
        /// of all commands that are part of this group. The tooltip of the command 
        /// group is used as the name.
        /// </summary>
        public bool AddDropdownBoxForParts { get; set; }

        /// <summary>
        /// If true, adds a command box to the toolbar for assemblies that has a dropdown
        /// of all commands that are part of this group. The tooltip of the command 
        /// group is used as the name.
        /// </summary>
        public bool AddDropdownBoxForAssemblies { get; set; }

        /// <summary>
        /// If true, adds a command box to the toolbar for drawings that has a dropdown
        /// of all commands that are part of this group. The tooltip of the command 
        /// group is used as the name.
        /// </summary>
        public bool AddDropdownBoxForDrawings { get; set; }

        /// <summary>
        /// The type of documents to show this command group in as a menu
        /// </summary>
        public ModelTemplateType MenuVisibleInDocumentTypes
        {
            get => (ModelTemplateType)BaseObject.ShowInDocumentType;
            set =>
                // Set base object
                BaseObject.ShowInDocumentType = (int)value;
        }

        /// <summary>
        /// Whether this command group has a Menu.
        /// NOTE: The menu is the regular drop-down menu like File, Edit, View etc...
        /// </summary>
        public bool HasMenu
        {
            get => BaseObject.HasMenu;
            set =>
                // Set base object
                BaseObject.HasMenu = value;
        }

        /// <summary>
        /// Whether this command group has a Toolbar.
        /// NOTE: The toolbar is the small icons like the top-left SolidWorks menu New, Save, Open etc...
        /// </summary>
        public bool HasToolbar
        {
            get => BaseObject.HasToolbar;
            set => BaseObject.HasToolbar = value;
        }

        /// <summary>
        /// The command items to add to this group
        /// </summary>
        public List<CommandManagerItem> Items { get; set; }

        /// <summary>
        /// The command flyouts to add to this group
        /// </summary>
        public List<CommandManagerFlyout> Flyouts { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="commandGroup">The SolidWorks command group</param>
        /// <param name="items">The command items to add</param>
        /// <param name="flyoutItems">The flyout command items that contain a list of other commands</param>
        /// <param name="userId">The unique Id this group was assigned with when created</param>
        /// <param name="title">The title</param>
        /// <param name="hint">The hint</param>
        /// <param name="tooltip">The tool tip</param>
        /// <param name="hasMenu">Whether the CommandGroup should appear in the Tools dropdown menu.</param>
        /// <param name="hasToolbar">Whether the CommandGroup should appear in the Command Manager and as a separate toolbar.</param>
        /// <param name="addDropdownBoxForParts">If true, adds a command box to the toolbar for parts that has a dropdown of all commands that are part of this group. The tooltip of the command group is used as the name.</param>
        /// <param name="addDropdownBoxForAssemblies">If true, adds a command box to the toolbar for assemblies that has a dropdown of all commands that are part of this group. The tooltip of the command group is used as the name.</param>
        /// <param name="addDropdownBoxForDrawings">If true, adds a command box to the toolbar for drawings that has a dropdown of all commands that are part of this group. The tooltip of the command group is used as the name.</param>
        /// <param name="documentTypes">The document types where this menu/toolbar is visible</param>
        /// <param name="iconListsPath">Absolute path to all icon sprites with including {0} for the image size</param>
        /// <param name="mainIconPath">Absolute path to all main icons with including {0} for the image size</param>
        public CommandManagerGroup(
            ICommandGroup commandGroup, 
            List<CommandManagerItem> items, 
            List<CommandManagerFlyout> flyoutItems, 
            int userId, 
            string title, 
            string hint,
            string tooltip,
            bool hasMenu, 
            bool hasToolbar,
            bool addDropdownBoxForParts,
            bool addDropdownBoxForAssemblies,
            bool addDropdownBoxForDrawings, 
            ModelTemplateType documentTypes,
            string iconListsPath,
            string mainIconPath) : base(commandGroup)
        {
            // Store user Id, used to remove the command group
            UserId = userId;

            // Set items
            Items = items;

            // Set flyouts
            Flyouts = flyoutItems;

            // Set title
            Title = title;

            // Set hint
            Hint = hint;

            // Set tooltip
            Tooltip = tooltip;

            // Show for certain types of documents, or when no document is active.
            MenuVisibleInDocumentTypes = documentTypes;

            // Have a menu
            HasMenu = hasMenu;

            // Have a toolbar
            HasToolbar = hasToolbar;

            // Dropdowns
            AddDropdownBoxForParts = addDropdownBoxForParts;
            AddDropdownBoxForAssemblies = addDropdownBoxForAssemblies;
            AddDropdownBoxForDrawings = addDropdownBoxForDrawings;

            // Set icon list
            mIconListPaths = Icons.GetFormattedPathDictionary(iconListsPath);

            // Set the main icon list
            mMainIconPaths = Icons.GetFormattedPathDictionary(mainIconPath);

            // Listen out for callbacks
            PlugInIntegration.CallbackFired += PlugInIntegration_CallbackFired;
        }

        #endregion

        #region Callbacks

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

            // Find the flyout, if any
            var flyout = Flyouts?.FirstOrDefault(f => f.CallbackId == name);

            // Call the action
            flyout?.OnClick?.Invoke();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a command item to the group
        /// </summary>
        /// <param name="item">The item to add</param>
        public void AddCommandItem(CommandManagerItem item)
        {
            // Add the item
            var id = BaseObject.AddCommandItem2(item.Name, item.Position, item.Hint, item.Tooltip, item.ImageIndex, $"Callback({item.CallbackId})", null, UserId, (int)item.ItemType);

            // Set the Id we got
            item.UniqueId = id;
        }

        /// <summary>
        /// Creates the command group based on its current children
        /// NOTE: Once created, parent command manager must remove and re-create the group
        /// This group cannot be re-used after creating, any edits will not take place
        /// </summary>
        /// <param name="manager">The command manager that is our owner</param>
        public void Create(CommandManager manager)
        {
            if (mCreated)
                throw new SolidDnaException(SolidDnaErrors.CreateError(
                    SolidDnaErrorTypeCode.SolidWorksCommandManager,
                    SolidDnaErrorCode.SolidWorksCommandGroupReActivateError));

            // Set all relevant icon properties, depending on the solidworks version
            SetIcons();

            // Add items
            Items?.ForEach(AddCommandItem);

            // Activate the command group
            mCreated = BaseObject.Activate();

            // Get command Ids
            Items?.ForEach(item => item.CommandId = BaseObject.CommandID[item.UniqueId]);

            #region Command Tab

            // Add items that are visible for parts
            AddItemsToTabForModelType(manager, ModelType.Part, AddDropdownBoxForParts);

            // Add items that are visible for assemblies
            AddItemsToTabForModelType(manager, ModelType.Assembly, AddDropdownBoxForAssemblies);

            // Add items that are visible for drawings
            AddItemsToTabForModelType(manager, ModelType.Drawing, AddDropdownBoxForDrawings);

            #endregion

            // If we failed to create, throw
            if (!mCreated)
                throw new SolidDnaException(SolidDnaErrors.CreateError(
                    SolidDnaErrorTypeCode.SolidWorksCommandManager,
                    SolidDnaErrorCode.SolidWorksCommandGroupActivateError));
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
        /// Add all items and flyouts that are visible for the given model type to a tab.
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="modelType"></param>
        /// <param name="addDropDown"></param>
        private void AddItemsToTabForModelType(CommandManager manager, ModelType modelType, bool addDropDown)
        {
            // Get items for this model type
            var items = GetItemsForModelType(Items, modelType);

            // Get flyouts for this model type
            var flyouts = GetFlyoutsForModelType(Flyouts, modelType);

            // Add items to a tab
            AddItemsToTab(modelType, manager, items, flyouts);

            // Add dropdown box?
            if (addDropDown)
            {
                var commandManagerItems = new List<CommandManagerItem>
                {
                    new CommandManagerItem
                    {
                        // Use this groups toolbar ID
                        CommandId = BaseObject.ToolbarId,
                        // Default style to text below for now
                        TabView = CommandManagerItemTabView.IconWithTextBelow
                    }
                };

                AddItemsToTab(modelType, manager, commandManagerItems, null);
            }
        }

        /// <summary>
        /// Get the command manager items for a model type.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="modelType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static List<CommandManagerItem> GetItemsForModelType(List<CommandManagerItem> items, ModelType modelType)
        {
            // Get the items that should be added to the tab
            var itemsForAllModelTypes = items?.Where(f => f.AddToTab && f.TabView != CommandManagerItemTabView.None);

            // Return the items for this model type
            switch (modelType)
            {
                case ModelType.Part: return itemsForAllModelTypes?.Where(f => f.VisibleForParts).ToList();
                case ModelType.Assembly: return itemsForAllModelTypes?.Where(f => f.VisibleForAssemblies).ToList();
                case ModelType.Drawing: return itemsForAllModelTypes?.Where(f => f.VisibleForDrawings).ToList();
                default: throw new ArgumentException("Invalid model type for command manager items");
            }
        }

        /// <summary>
        /// Get the command manager flyouts for a model type.
        /// </summary>
        /// <param name="flyouts"></param>
        /// <param name="modelType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static List<CommandManagerFlyout> GetFlyoutsForModelType(List<CommandManagerFlyout> flyouts, ModelType modelType)
        {
            // Get the flyouts that should be added to the tab
            var flyoutsForAllModelTypes = flyouts?.Where(f => f.TabView != CommandManagerItemTabView.None);

            // Return the flyouts for this model type
            switch (modelType)
            {
                case ModelType.Part: return flyoutsForAllModelTypes?.Where(f => f.VisibleForParts).ToList();
                case ModelType.Assembly: return flyoutsForAllModelTypes?.Where(f => f.VisibleForAssemblies).ToList();
                case ModelType.Drawing: return flyoutsForAllModelTypes?.Where(f => f.VisibleForDrawings).ToList();
                default: throw new ArgumentException("Invalid model type for command manager flyouts");
            }
        }

        /// <summary>
        /// Adds all <see cref="Items"/> to the command tab of the given title and model type
        /// </summary>
        /// <param name="type">The tab for this type of model</param>
        /// <param name="manager">The command manager</param>
        /// <param name="items">Items to add</param>
        /// <param name="flyouts">Flyout Items to add</param>
        /// <param name="title">The title of the tab</param>
        public void AddItemsToTab(ModelType type, CommandManager manager, List<CommandManagerItem> items, List<CommandManagerFlyout> flyouts, string title = "")
        {
            // Use default title if not specified
            if (string.IsNullOrEmpty(title))
                title = Title;

            CommandManagerTab tab;

            // Get the tab if it already exists
            if (mTabs.Any(f => string.Equals(f.Key.Title, title) && f.Key.ModelType == type))
                tab = mTabs.First(f => string.Equals(f.Key.Title, title) && f.Key.ModelType == type).Value;
            // Otherwise create it
            else
            {
                // Get the tab
                tab = manager.GetCommandTab(type, title, createIfNotExist: true);

                // Keep track of this tab
                mTabs.Add(new CommandManagerTabKey { ModelType = type, Title = title }, tab);
            }

            // New list of values
            var ids = new List<int>();
            var styles = new List<int>();

            // Add each items Id and style
            items?.ForEach(item =>
            {
                // Add command Id
                ids.Add(item.CommandId);

                // Add style
                styles.Add((int)item.TabView);
            });

            flyouts?.ForEach(item =>
            {
                // Add command Id
                ids.Add(item.CommandId);

                // Add style
                styles.Add((int)item.TabView | (int)swCommandTabButtonFlyoutStyle_e.swCommandTabButton_ActionFlyout);
            });

            // If there are items to add...
            if (ids?.Count > 0)
            {
                // Add all the items
                tab.Box.UnsafeObject.AddCommands(ids.ToArray(), styles.ToArray());

                // Add a separator before each item that wants one.
                AddSeparators(items, tab);
            }
        }

        /// <summary>
        /// Add a separator before each <see cref="CommandManagerItem"/> that needs one.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="tab"></param>
        private static void AddSeparators(List<CommandManagerItem> items, CommandManagerTab tab)
        {
            // Get the current tab box
            var tabBox = (CommandTabBox) tab.Box.UnsafeObject;

            // Add a separator before each item that wants one
            var itemsThatNeedSeparator = items.Where(f => f.AddSeparatorBeforeThisItem).ToList();
            foreach (var item in itemsThatNeedSeparator)
            {
                // Add the separator and split the current tab box in two.
                // Returns the newly created tab box that contains the current items and all items on the right of it.
                var newTabBox = tab.UnsafeObject.AddSeparator(tabBox, item.CommandId);

                // Stop if the don't receive a new tab box. This can happen if not all items are visible in all model types.
                if (newTabBox == null)
                    break;
                
                // Use the new tab box to create the next separator.
                tabBox = newTabBox;
            }
        }

        #endregion

        #region ToString

        /// <summary>
        /// Returns a user-friendly string with group properties.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Title}: {Items?.Count ?? 0} items, {Flyouts?.Count ?? 0} flyouts. Has menu: {HasMenu}. Has toolbar: {HasToolbar}";
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Disposing
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

        #endregion
    }
}
