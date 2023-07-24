using SolidWorks.Interop.sldworks;
using System.Collections.Generic;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Provides a way to manage and interact with command tabs. By default, it has one <see cref="CommandManagerTabBox"/>.
    /// When you add a separator, it splits the tab box into two.
    /// </summary>
    public class CommandManagerTab : SolidDnaObject<ICommandTab>
    {
        /// <summary>
        /// Public accessible list with command manager tab boxes. Can be used to add commands, get commands or remove commands. Separated to multiple boxes
        /// so we don't have to add separators ourselves anymore. In the previous versions we didn't save all the boxes we created and added our commands to, now we do.
        /// </summary>
        public List<CommandManagerTabBox> TabBoxes { get; } = new List<CommandManagerTabBox>();

        /// <summary>
        /// Takes an object of type <see cref="ICommandTab"/> and calls the constructor of its base class.
        /// It will create a new instance of <see cref="CommandManagerTabBox"/>, it then utilizes a method on the BaseObject.
        /// After the CommandManagerTabBox is created, it will be assigned to the '<see cref="TabBoxes"/>'-property of this class.
        /// </summary>
        public CommandManagerTab(ICommandTab tab) : base(tab) { }
        
        public override void Dispose()
        {
            foreach (var tabBox in TabBoxes)
                tabBox?.Dispose();
            
            base.Dispose();
        }
    }
}
