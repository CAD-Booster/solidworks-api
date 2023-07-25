using System;
using SolidWorks.Interop.swconst;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CADBooster.SolidDna.SolidWorks.CommandManager.Tab
{
    /// <summary>
    /// Provides an easier-to-understand enum for choosing how to show the tab items within the CommandManagerTab.
    /// It has the flags attribute, enabling multiple styles to be combined using bitwise operators. Combines the
    /// '<see cref="swCommandTabButtonFlyoutStyle_e"/>'-enum and '<see cref="CommandManagerItemTabView"/>'-enum.
    /// </summary>
    [Flags]
    public enum CommandManagerFlyoutStyle
    {
        /// <summary>
        /// The item is not shown in the tab
        /// </summary>
        None = 0,

        /// <summary>
        /// The item is shown with an icon only
        /// </summary>
        IconOnly = 1,

        /// <summary>
        /// The item is shown with the icon, then the text below it
        /// </summary>
        IconWithTextBelow = 2,

        /// <summary>
        /// The item is shown with the icon then the text to the right
        /// </summary>
        IconWithTextAtRight = 4,

        /// <summary>
        /// Sets style to have no flyout
        /// Represents <see cref="swCommandTabButtonFlyoutStyle_e.swCommandTabButton_NoFlyout"/>
        /// </summary>
        NoFlyout = 8,

        /// <summary>
        /// No idea what this enum does exactly, ToDo - We need to find out what this does when we're going to fix the flyout menus.
        /// Represents the <see cref="swCommandTabButtonFlyoutStyle_e.swCommandTabButton_SimpleFlyout"/>
        /// </summary>
        SimpleFlyout = 16,

        /// <summary>
        /// No idea what this enum does exactly, ToDo - We need to find out what this does when we're going to fix the flyout menus.
        /// Represents the <see cref="swCommandTabButtonFlyoutStyle_e.swCommandTabButton_ActionFlyout"/>
        /// </summary>
        ActionFlyout = 32
    }
}
