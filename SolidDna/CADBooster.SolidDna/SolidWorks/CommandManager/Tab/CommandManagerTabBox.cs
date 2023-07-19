using SolidWorks.Interop.sldworks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Can be seen as a group, filled with commands, in which you can add, get or remove commands.
    /// Only one instance needed within the <see cref="CommandManagerTab"/>, will be stored there as well.
    /// </summary>
    public class CommandManagerTabBox : SolidDnaObject<ICommandTabBox>
    {
        /// <summary>
        /// Can be seen as a group, filled with commands, in which you can add, get or remove commands.
        /// Only one instance needed within the <see cref="CommandManagerTab"/>, will be stored there as well.
        /// </summary>
        public CommandManagerTabBox(ICommandTabBox box) : base(box) { }
    }
}
