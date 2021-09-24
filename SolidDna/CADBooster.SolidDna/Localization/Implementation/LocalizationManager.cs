using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// A resource manager that supports cross-platform localization
    /// </summary>
    public class LocalizationManager : ILocalizationManager
    {
        #region Public Properties

        public string DefaultCulture { get; set; }

        public ResourceDefinition StringResourceDefinition { get; set; }

        public List<IResourceFormatProvider> Providers { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public LocalizationManager()
        {
            DefaultCulture = "en-US";

            // Add the providers we want to use by default
            Providers = new List<IResourceFormatProvider>
            {
                // Support XML format
                new XmlFormatProvider()
            };
        }

        #endregion

        /// <summary>
        /// Finds a string of the given name, taking into account the culture information.
        /// If no culture is specified, the default culture is used
        /// 
        /// IMPORTANT:
        /// NOTE: Make sure any and all await calls inside this function and its children
        ///       use ConfigureAwait(false). This is because the parent has to support 
        ///       a synchronous version of this call, so the method cannot sync back with
        ///       its calling context without risk of deadlock.
        /// </summary>
        /// <param name="name">The name of the resource to find</param>
        /// <param name="culture">The culture information to use</param>
        /// <returns>Returns the string if found, or null if not found</returns>
        public async Task<string> GetStringAsync(string name, string culture = null)
        {
            // Make sure we have a string format 
            if (StringResourceDefinition == null)
                return null;

            // If we have no providers we cannot do anything
            if (Providers == null)
                return null;

            // Get file format if specified
            var format = ResourceFormatProviderHelpers.GetFormat(StringResourceDefinition);

            // Find a provider that supports the format
            var supportedProviders = Providers.Where(f => f.SupportsFormat(format));

            // If we have no supported format providers, return null
            if (supportedProviders.Count() == 0)
                return null;

            // Now that we have format providers, attempt to get the value from one, stopping as soon as one is successful
            string value = null;

            foreach (var provider in supportedProviders)
            {
                if (await provider.GetStringAsync(StringResourceDefinition, name, culture, (result) => { value = result; }).ConfigureAwait(false))
                    break;
            }

            // Return whatever value we found
            return value;
        }
    }
}
