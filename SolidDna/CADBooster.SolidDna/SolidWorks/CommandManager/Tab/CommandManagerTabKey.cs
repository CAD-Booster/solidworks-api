namespace CADBooster.SolidDna
{
    /// <summary>
    /// Represents a key to uniquely identify a command tab. Will be created for every <see cref="ModelType"/> we assign it to. The <see cref="Title"/> should be the same for every
    /// model type. For example, when we want to have our command manager tab(/toolbar/ribbon) visible when a part or assembly model is active, this class will be called twice.
    /// </summary>
    public class CommandManagerTabKey
    {
        /// <summary>
        /// The title of the tab
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Model type which this tab is attached to, could be either a part, assembly or drawing.
        /// </summary>
        public ModelType ModelType { get; set; }
    }
}
