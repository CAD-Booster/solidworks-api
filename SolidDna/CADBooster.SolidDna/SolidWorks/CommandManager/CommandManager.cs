using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace CADBooster.SolidDna
{
    public class CommandManager : SolidDnaObject<ICommandManager>
    {
        /// <summary>
        /// A list of all created command groups
        /// </summary>
        private readonly List<CommandManagerGroup> mCommandGroups = new List<CommandManagerGroup>();

        /// <summary>
        /// A list of all created command flyouts
        /// </summary>
        private List<CommandManagerFlyout> mCommandFlyouts = new List<CommandManagerFlyout>();

        /// <summary>
        /// Unique Id for flyouts (just increment every time we add one)
        /// </summary>
        private int mFlyoutIdCount = 1000;

        /// <summary>
        /// Creates a command manager which let us create and access custom toolbars/tabs/ribbons) and menus.
        /// There is only one command manager per SolidWorks instance.
        /// </summary>
        public CommandManager(ICommandManager commandManager) : base(commandManager) { }

        /// <summary>
        /// Create a command group from a list of <see cref="CommandManagerItem"/> items. Uses a single list of items, separators and flyouts.
        /// </summary>
        /// <param name="title">Name of the CommandGroup to create. Is also used for the tab.</param>
        /// <param name="commandManagerItems">All items that should appear in this tab. You can mix items (<see cref="CommandManagerItem"/>), separators (<see cref="CommandManagerSeparator"/>) and flyouts (<see cref="CommandManagerFlyout"/>).</param>
        /// <param name="mainIconPathFormat">Absolute path to the image files that contain the main icon.
        /// The main icon is visible in the Customize window. If you don't set a main icon, SolidWorks uses the first icon in <paramref name="iconListsPathFormat"/>.
        /// Based on a string format, replacing {0} with the size. For example C:\Folder\MainIcon{0}.png</param>
        /// <param name="iconListsPathFormat">Absolute path to the image files that contain the button icons. Based on a string format, replacing {0} with the size. For example C:\Folder\Icons{0}.png</param>
        /// <param name="position">Position of the CommandGroup in the CommandManager for all document templates.
        /// NOTE: Specify 0 to add the CommandGroup to the beginning of the CommandManager, or specify -1 to add it to the end of the CommandManager.
        /// NOTE: You can also use ICommandGroup::MenuPosition to control the position of the CommandGroup in specific document templates.</param>
        /// <param name="ignorePreviousVersion">True to remove all previously saved customization and toolbar information before creating a new CommandGroup, false to not.
        /// Call CommandManager.GetGroupDataFromRegistry before calling this method to determine how to set IgnorePreviousVersion.
        /// Set IgnorePreviousVersion to true to prevent SOLIDWORKS from saving the current toolbar setting to the registry, even if there is no previous version.</param>
        /// <param name="hasMenu">Whether the CommandGroup should appear in the Tools dropdown menu.</param>
        /// <param name="hasToolbar">Whether the CommandGroup should appear in the Command Manager and as a separate toolbar.</param>
        /// <param name="documentTypes">The document types where this menu is visible. Only works for menus.
        /// To set toolbar button visibilities, set the three VisibleForX properties in <see cref="CommandManagerItem"/>. To hide flyouts items, use the item's UpdateCallback function.</param>
        /// <returns></returns>
        public CommandManagerGroup CreateCommandGroupAndTabs(string title, List<ICommandManagerItem> commandManagerItems, string mainIconPathFormat = "", string iconListsPathFormat = "", 
                                                              int position = -1, bool ignorePreviousVersion = true, bool hasMenu = true, bool hasToolbar = true, 
                                                              ModelTemplateType documentTypes = ModelTemplateType.Part | ModelTemplateType.Assembly | ModelTemplateType.Drawing)
        {
            // Wrap any error creating the taskpane in a SolidDna exception
            return SolidDnaErrors.Wrap(() =>
            {
                // Lock the list
                lock (mCommandGroups)
                {
                    // Make sure the list is not null. Check it once here so we never have to check again.
                    if (commandManagerItems == null)
                        commandManagerItems = new List<ICommandManagerItem>();

                    // Create the command group
                    var group = CreateCommandGroup(title, commandManagerItems, position, ignorePreviousVersion, hasMenu, hasToolbar, documentTypes, iconListsPathFormat, mainIconPathFormat);

                    // Track all flyouts
                    mCommandFlyouts = commandManagerItems.OfType<CommandManagerFlyout>().ToList();

                    // Create the group
                    group.Create(this, title);

                    // Add this group to the list
                    mCommandGroups.Add(group);

                    // Return the group
                    return group;
                }
            },
                SolidDnaErrorTypeCode.SolidWorksCommandManager,
                SolidDnaErrorCode.SolidWorksCommandGroupCreateError);
        }

        /// <summary>
        /// Create a command group flyout containing a list of <see cref="CommandManagerItem"/> items
        /// </summary>
        /// <param name="title">Name of the flyout to create</param>
        /// <param name="items">The command items to add</param>
        /// <param name="iconListsPathFormat">The icon list absolute path based on a string format of the absolute path to the icon list images, replacing {0} with the size. 
        ///     For example C:\Folder\icons{0}.png</param>
        /// <param name="tooltip">Tool tip for the new flyout</param>
        /// <param name="hint">Text displayed in SOLIDWORKS status bar when a user's mouse pointer is over the flyout</param>
        /// <returns></returns>
        [Obsolete("Replaced by CreateFlyoutGroup2, which allows you to set separate icon lists for the main icon and underlying command icons.")]
        public CommandManagerFlyout CreateFlyoutGroup(string title, List<CommandManagerItem> items, string iconListsPathFormat, string tooltip = "", string hint = "")
            => CreateFlyoutGroup2(title, items, iconListsPathFormat, iconListsPathFormat, tooltip, hint);

        /// <summary>
        /// Create a command group flyout containing a list of <see cref="CommandManagerItem"/> items. This is the newer version of
        /// <see cref="CreateFlyoutGroup"/>, and makes it possible to add the main icons separately from the underlying commands.
        /// <paramref name="mainIconPathFormat"/> is the main icon for the flyout on the toolbar its self. <paramref name="iconListsPathFormat"/> contains the icons for all the underlying commands.
        /// </summary>
        /// <param name="title">Name of the flyout to create</param>
        /// <param name="items">The command items to add</param>
        /// <param name="mainIconPathFormat">Absolute path to the image files that contain the main flyout icon that appears in the tab. Based on a string format, replacing {0} with the size. For example C:\Folder\FlyoutIcon{0}.png</param>
        /// <param name="iconListsPathFormat">Absolute path to the image files that contain the button icons. Based on a string format, replacing {0} with the size. For example C:\Folder\Icons{0}.png</param>
        /// <param name="tooltip">The name of this item. Appears as the name and above the <paramref name="hint"/> in the tooltip.</param>
        /// <param name="hint">Text displayed in SOLIDWORKS status bar when a user's mouse pointer is over the flyout. Also visible in the tooltip below the <paramref name="tooltip"/></param>
        /// <returns></returns>
        public CommandManagerFlyout CreateFlyoutGroup2(string title, List<CommandManagerItem> items, string mainIconPathFormat, string iconListsPathFormat, string tooltip, string hint)
        {
            // Make sure the item list is not null. Check it once here so we never have to check again.
            if (items == null)
                items = new List<CommandManagerItem>();

            // Get icon paths
            var mainIconPaths = Icons.GetPathArrayFromPathFormat(mainIconPathFormat);
            var commandIconPaths = Icons.GetPathArrayFromPathFormat(iconListsPathFormat);

            // Create unique callback Id
            var callbackId = Guid.NewGuid().ToString("N");

            // Attempt to create the command flyout
            var unsafeCommandFlyout = BaseObject.CreateFlyoutGroup2(mFlyoutIdCount, title, tooltip, hint, mainIconPaths, commandIconPaths, $"{nameof(SolidAddIn.Callback)}({callbackId})", null);

            // Create managed object
            var flyout = new CommandManagerFlyout(unsafeCommandFlyout, mFlyoutIdCount++, callbackId, items, title, hint, tooltip);

            // Return it
            return flyout;
        }

        /// <summary>
        /// Creates a command group
        /// </summary>
        /// <param name="title">The title</param>
        /// <param name="items">The command items to add</param>
        /// <param name="position">Position of the CommandGroup in the CommandManager for all document templates.
        /// NOTE: Specify 0 to add the CommandGroup to the beginning of the CommandManager, or specify -1 to add it to the end of the CommandManager.
        /// NOTE: You can also use ICommandGroup::MenuPosition to control the position of the CommandGroup in specific document templates.</param>
        /// <param name="ignorePreviousVersion">True to remove all previously saved customization and toolbar information before creating a new CommandGroup, false to not.
        ///     Call CommandManager.GetGroupDataFromRegistry before calling this method to determine how to set IgnorePreviousVersion.
        ///     Set IgnorePreviousVersion to true to prevent SOLIDWORKS from saving the current toolbar setting to the registry, even if there is no previous version.</param>
        /// <param name="hasMenu">Whether the CommandGroup should appear in the Tools dropdown menu.</param>
        /// <param name="hasToolbar">Whether the CommandGroup should appear in the Command Manager and as a separate toolbar.</param>
        /// <param name="documentTypes">The document types where this menu/toolbar is visible.</param>
        /// <param name="iconListsPathFormat">The icon list absolute path based on a string format of the absolute path to the icon list images, replacing {0} with the size. 
        ///     For example C:\Folder\icons{0}.png</param>
        /// <param name="mainIconPathFormat">The icon absolute path base on a string format of the absolute path to the main icon images, replacing {0} with the size.
        /// The main icon is visible in the Customize window. If you don't set a main icon, SolidWorks uses the first icon in <paramref name="iconListsPathFormat"/>.</param>
        /// <returns></returns>
        private CommandManagerGroup CreateCommandGroup(string title, List<ICommandManagerItem> items, int position = -1, bool ignorePreviousVersion = true,
                                                       bool hasMenu = true, bool hasToolbar = true, 
                                                       ModelTemplateType documentTypes = ModelTemplateType.Part | ModelTemplateType.Assembly | ModelTemplateType.Drawing, 
                                                       string iconListsPathFormat = "", string mainIconPathFormat = "")
        {
            // NOTE: We may need to look carefully at this Id if things get removed and re-added based on this SolidWorks note:
            //     
            //       If you change the definition of an existing CommandGroup (i.e., add or remove toolbar buttons), you must assign a 
            //       new unique user-defined UserID to that CommandGroup. You must perform this action to avoid conflicts with any 
            //       previously existing CommandGroups and to allow for backward and forward compatibility of the CommandGroups in your application.
            // 

            // Get the next Id
            var id = mCommandGroups.Count == 0 ? 1 : mCommandGroups.Max(f => f.UserId) + 1;

            // Store error code
            var errors = -1;

            // Attempt to create the command group
            var unsafeCommandGroup = BaseObject.CreateCommandGroup2(id, title, "", "", position, ignorePreviousVersion, ref errors);

            // Check for errors
            if (errors != (int)swCreateCommandGroupErrors.swCreateCommandGroup_Success)
            {
                // Get enum name
                var errorEnumString = ((swCreateCommandGroupErrors)errors).ToString();
                throw new SolidDnaException(SolidDnaErrors.CreateError(SolidDnaErrorTypeCode.SolidWorksCommandManager, SolidDnaErrorCode.SolidWorksCommandGroupCreateError, errorEnumString));
            }

            // Otherwise we got the command group
            var group = new CommandManagerGroup(unsafeCommandGroup, items, id, title, hasMenu, hasToolbar, documentTypes, iconListsPathFormat, mainIconPathFormat);

            // Return it
            return group;
        }

        /// <summary>
        /// Removes the specific command flyout
        /// </summary>
        /// <param name="flyout">The command flyout to remove</param>
        private void RemoveCommandFlyout(CommandManagerFlyout flyout)
        {
            lock (mCommandFlyouts)
                BaseObject.RemoveFlyoutGroup(flyout.UserId);
        }

        /// <summary>
        /// Removes the specific command group
        /// </summary>
        /// <param name="group">The command group to remove</param>
        /// <param name="runtimeOnly">True to remove the CommandGroup, saving its toolbar information in the registry. False to remove both the CommandGroup and its toolbar information in the registry</param>
        private void RemoveCommandGroup(CommandManagerGroup group, bool runtimeOnly = false)
        {
            lock (mCommandGroups)
                BaseObject.RemoveCommandGroup2(group.UserId, runtimeOnly);
        }

        /// <summary>
        /// Gets the command tab for this 
        /// </summary>
        /// <param name="type">The type of document to get the tab for. Use only Part, Assembly or Drawing one at a time, otherwise the first found tab gets returned</param>
        /// <param name="title">The title of the command tab to get</param>
        /// <param name="createIfNotExist">True to create the tab if it does not exist</param>
        /// <param name="clearExistingItems">Removes any existing items on the tab if true</param>
        /// <returns></returns>
        public CommandManagerTab GetCommandTab(ModelType type, string title, bool createIfNotExist = true, bool clearExistingItems = true)
        {
            // Try and get the tab
            var unsafeTab = BaseObject.GetCommandTab((int)type, title);

            // If we did not get it, just ignore
            if (unsafeTab == null && !createIfNotExist)
                return null;

            // If we want to remove any previous tabs...
            while (clearExistingItems && unsafeTab != null)
            {
                // Remove it
                BaseObject.RemoveCommandTab(unsafeTab);

                // Clean COM object
                Marshal.ReleaseComObject(unsafeTab);

                // Try and get another
                unsafeTab = BaseObject.GetCommandTab((int)type, title);
            }

            // Create it if it doesn't exist
            if (unsafeTab == null)
                unsafeTab = BaseObject.AddCommandTab((int)type, title);

            // If it's still null, we failed
            if (unsafeTab == null)
                throw new SolidDnaException(SolidDnaErrors.CreateError(SolidDnaErrorTypeCode.SolidWorksCommandManager, SolidDnaErrorCode.SolidWorksCommandGroupCreateTabError));

            // Return tab
            return new CommandManagerTab(unsafeTab);
        }

        /// <summary>
        /// Dispose all <see cref="CommandGroup"/>(s) and <see cref="FlyoutGroup"/>(s) from the <see cref="mCommandGroups"/> and <see cref="mCommandFlyouts"/> lists correctly.
        /// </summary>
        public override void Dispose()
        {
            // Remove all command groups
            mCommandGroups?.ForEach(f => RemoveCommandGroup(f));

            // Remove all command flyouts
            mCommandFlyouts?.ForEach(f => RemoveCommandFlyout(f));

            base.Dispose();
        }
    }
}
