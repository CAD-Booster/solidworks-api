using SolidWorks.Interop.sldworks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Can be seen as a group with commands, in which you can add, get or remove commands.
    /// Each group of <see cref="CommandManagerItem"/>s needs its own tab box. We store the list of tab boxes in <see cref="CommandManagerTab"/>.
    /// </summary>
    public class CommandManagerTabBox : SolidDnaObject<ICommandTabBox>
    {
        /// <summary>
        /// Can be seen as a group, filled with commands, in which you can add, get or remove commands.
        /// Each group of <see cref="CommandManagerItem"/>s needs its own tab box. We store the list of tab boxes in <see cref="CommandManagerTab"/>.
        /// </summary>
        public CommandManagerTabBox(ICommandTabBox box) : base(box) { }
    }
}
