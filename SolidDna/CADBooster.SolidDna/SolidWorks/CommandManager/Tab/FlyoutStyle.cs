using System;
using SolidWorks.Interop.swconst;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Combines <see cref="swCommandTabButtonFlyoutStyle_e"/> and <see cref="CommandManagerItemTabView"/>.
    /// Provides an easier-to-understand enum for choosing how to show the tab items within the CommandManagerTab.
    /// You can combine styles by using bitwise operators. 
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
        /// Sets style to have no flyout. Represents <see cref="swCommandTabButtonFlyoutStyle_e.swCommandTabButton_NoFlyout"/>.
        /// When we combine this value with <see cref="CommandManagerFlyout.TabView"/> to create <see cref="TabItemData.Style"/>, there are no visible changes.
        /// </summary>
        NoFlyout = 8,

        /// <summary>
        /// Sets style to have a simple flyout. Represents the <see cref="swCommandTabButtonFlyoutStyle_e.swCommandTabButton_SimpleFlyout"/>
        /// When we combine this value with <see cref="CommandManagerFlyout.TabView"/> to create <see cref="TabItemData.Style"/>, there are no visible changes.
        /// </summary>
        SimpleFlyout = 16,

        /// <summary>
        /// Sets style to have an action flyout. Represents the <see cref="swCommandTabButtonFlyoutStyle_e.swCommandTabButton_ActionFlyout"/>
        /// When we combine this value with <see cref="CommandManagerFlyout.TabView"/> to create <see cref="TabItemData.Style"/>, there are no visible changes.
        /// </summary>
        ActionFlyout = 32
    }
}
