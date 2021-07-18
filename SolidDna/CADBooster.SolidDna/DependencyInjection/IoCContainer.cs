using Dna;
using System;
using static Dna.FrameworkDI;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Access to available IoC services
    /// </summary>
    public static class IoC
    {
        #region Specific Dependency shortcuts

        /// <summary>
        /// Access the <see cref="ILocalizationManager"/>
        /// </summary>
        public static ILocalizationManager Localization => Get<ILocalizationManager>();

        #endregion

        #region Method shortcuts

        /// <summary>
        /// Attempts to get the injected service of the specified type
        /// </summary>
        /// <typeparam name="T">The type of service to fetch</typeparam>
        /// <returns></returns>
        public static T Get<T>()
        {
            return IoCContainer.Get<T>();
        }

        /// <summary>
        /// Attempts to get the injected service of the specified type
        /// </summary>
        /// <param name="type">The type of service to fetch</param>
        /// <returns></returns>
        public static object Get(Type type)
        {
            return IoCContainer.Get(type);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets up the IoC and injects all required elements
        /// </summary>
        /// <param name="addinPath">The full path to the add-in dll file</param>
        /// <param name="configureServices">Provides a callback to inject any services into the Dna.Framework DI system</param>
        public static void SetUpForFirstAddIn()
        {
            // If another add-in is already loaded, IoC is already set up so we can skip it.
            if (Framework.Construction != null) return;

            // Create default construction
            Framework.Construct(new DefaultFrameworkConstruction());

            // Add localization manager
            Framework.Construction.AddLocalizationManager();

            // Build DI
            Framework.Construction.Build();

            // Log details
            Logger?.LogDebugSource($"DI Setup complete");
        }

        #endregion 
    }

    /// <summary>
    /// Access to available IoC services
    /// </summary>
    public static class IoCContainer
    {
        #region Public Methods

        /// <summary>
        /// Attempts to get the injected service of the specified type
        /// </summary>
        /// <typeparam name="T">The type of service to fetch</typeparam>
        /// <returns></returns>
        public static T Get<T>()
        {
            try
            {
                return Framework.Service<T>();
            }
            catch (Exception ex)
            {
                Logger?.LogCriticalSource($"Get '{typeof(T)}' failed. {ex.GetErrorMessage()}");
                return default;
            }
        }

        /// <summary>
        /// Attempts to get the injected service of the specified type
        /// </summary>
        /// <param name="type">The type of service to fetch</param>
        /// <returns></returns>
        public static object Get(Type type)
        {
            try
            {
                return Framework.Provider.GetService(type);
            }
            catch (Exception ex)
            {
                Logger?.LogCriticalSource($"Get '{type}' failed. {ex.GetErrorMessage()}");
                return null;
            }
        }

        #endregion
    }
}
