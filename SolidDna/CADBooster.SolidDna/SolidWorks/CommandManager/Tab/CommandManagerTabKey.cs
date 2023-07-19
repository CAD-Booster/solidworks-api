namespace CADBooster.SolidDna
{
    /// <summary>
    /// Represents a key to uniquely identify a command tab. Will be created for every <see cref="ModelType"/> we assign it to. The <see cref="Title"/> should be the same for every
    /// model type. For example, when we want to have our command manager tab(/toolbar/ribbon) visible when a part or assembly model is active, this class will be called twice.
    /// </summary>
    public class CommandManagerTabKey
    {
        /// <summary>
        /// Takes two parameters which will be then set to the public properties of this class.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="modelType"></param>
        public CommandManagerTabKey(string title, ModelType modelType)
        {
            Title = title;
            ModelType = modelType;
        }

        /// <summary>
        /// The unique title of the tab. Should be the same for each tab that is going to be created.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Model type which this tab is attached to, could be either a part, assembly or drawing.
        /// </summary>
        public ModelType ModelType { get; }
    }
}
