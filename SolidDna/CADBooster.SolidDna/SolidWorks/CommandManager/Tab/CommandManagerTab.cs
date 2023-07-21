using SolidWorks.Interop.sldworks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Provides a way to manage and interact with command tabs. By default, it has one <see cref="CommandManagerTabBox"/>.
    /// When you add a separator, it splits the tab box into two.
    /// </summary>
    public class CommandManagerTab : SolidDnaObject<ICommandTab>
    {
        /// <summary>
        /// Public accessible command manager tab box. Can be used to add commands, get commands or remove commands.
        /// </summary>
        public CommandManagerTabBox Box { get; }

        /// <summary>
        /// Takes an object of type <see cref="ICommandTab"/> and calls the constructor of its base class.
        /// It will create a new instance of <see cref="CommandManagerTabBox"/>, it then utilizes a method on the BaseObject.
        /// After the CommandManagerTabBox is created, it will be assigned to the '<see cref="Box"/>'-property of this class.
        /// </summary>
        public CommandManagerTab(ICommandTab tab) : base(tab)
        {
            Box = new CommandManagerTabBox(BaseObject.AddCommandTabBox());
        }
        
        public override void Dispose()
        {
            Box?.Dispose();
            base.Dispose();
        }
    }
}
