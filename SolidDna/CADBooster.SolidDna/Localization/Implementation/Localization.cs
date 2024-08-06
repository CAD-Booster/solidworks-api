using System.Threading.Tasks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Static shortcut functions for localization 
    /// </summary>
    public static class Localization
    {
        /// <summary>
        /// The default culture if no other culture is set or available.
        /// </summary>
        public static string DefaultCulture => LocalizationManager.DefaultCulture;

        /// <summary>
        /// The active localization manager.
        /// Since we only have en-us strings so far, we use a single instance.
        /// </summary>
        public static LocalizationManager LocalizationManager = new LocalizationManager
        {
            StringResourceDefinition = new ResourceDefinition
            {
                Type = ResourceDefinitionType.EmbeddedResource,
                Location = "CADBooster.SolidDna.Localization.Strings.Strings-{0}.xml",
                UseDefaultCultureIfNotFound = true
            }
        };

        /// <summary>
        /// Gets a string resource
        /// </summary>
        /// <param name="name">The name of the string resource to retrieve</param>
        /// <param name="culture">The culture to use if different from the default</param>
        /// <returns>Returns the value of the string if found, and null if not found</returns>
        public static async Task<string> GetStringAsync(string name, string culture = null)
        {
            // NOTE: No null check because it should always be injected or throw if not as the expected result would be the actual resource string
            //       We do not want to fail silently
            return await LocalizationManager.GetStringAsync(name, culture);
        }

        /// <summary>
        /// Gets a string resource
        /// </summary>
        /// <param name="name">The name of the string resource to retrieve</param>
        /// <param name="culture">The culture to use if different from the default</param>
        /// <returns>Returns the value of the string if found, and null if not found</returns>
        public static string GetString(string name, string culture = null)
        {
            // NOTE: No null check because it should always be injected or throw if not as the expected result would be the actual resource string
            //       We do not want to fail silently
            return AsyncHelpers.RunSync(() => LocalizationManager.GetStringAsync(name, culture));
        }
    }
}
