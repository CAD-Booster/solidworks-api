using SolidWorks.Interop.sldworks;
using System.Collections.Generic;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Provides a way to manage and interact with command tabs. By default, it has a empty list of <see cref="CommandManagerTabBox"/>es.
    /// Adds a tab box every time there is a separator in the list of <see cref="CommandManagerItem"/>s.
    /// </summary>
    public class CommandManagerTab : SolidDnaObject<ICommandTab>
    {
        /// <summary>
        /// Public accessible list with command manager tab boxes. Can be used to add commands, get commands or remove commands.
        /// Separated into multiple boxes so we don't have to add separators after creating a single tab box with all <see cref="CommandManagerItem"/>.
        /// Now we create a tab box every time we find a separator and add only the items before the next separator.
        /// </summary>
        public List<CommandManagerTabBox> TabBoxes { get; } = new List<CommandManagerTabBox>();

        /// <summary>
        /// Wrap a <see cref="ICommandTab"/> and create an empty list of <see cref="CommandManagerTabBox"/>es.
        /// </summary>
        public CommandManagerTab(ICommandTab tab) : base(tab) { }
        
        /// <summary>
        /// Dispose the <see cref="ICommandTab"/> and all <see cref="CommandManagerTabBox"/>es.
        /// </summary>
        public override void Dispose()
        {
            foreach (var tabBox in TabBoxes)
                tabBox?.Dispose();
            
            base.Dispose();
        }
    }
}
