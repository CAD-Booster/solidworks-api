namespace AngelSix.SolidWorksApi.AddinInstaller
{
    /// <summary>
    /// Collection of relevant add-in properties.
    /// </summary>
    public class AddInProperties
    {
        public AddInProperties(string path, string title)
        {
            Path = path;
            Title = title;
        }

        /// <summary>
        /// Complete path to the DLL that contains this add-in.
        /// </summary>
        public string Path { get; }
        
        /// <summary>
        /// The user-friendly add-in title.
        /// </summary>
        public string Title { get; }

        public override string ToString() => $"Add-in title: {Title}. Path: {Path}";
    }
}
