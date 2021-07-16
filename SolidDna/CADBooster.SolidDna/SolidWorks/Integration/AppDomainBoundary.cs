using Dna;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using static Dna.FrameworkDI;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Helper class used to bridge the gap between app domains
    /// </summary>
    public class AppDomainBoundary
    {
        #region Protected Members

        /// <summary>
        /// A list of assemblies to use when resolving any missing references
        /// </summary>
        protected static List<AssemblyName> mReferencedAssemblies = new List<AssemblyName>();

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the list of all known reference assemblies in this solution
        /// </summary>
        public AssemblyName[] ReferencedAssemblies => mReferencedAssemblies.ToArray();

        #endregion

        #region App Domain Setup

        /// <summary>
        /// Sets up the application
        /// </summary>
        /// <param name="assemblyFilePath">The path to the assembly</param>
        /// <param name="configureDllPath">Path to the dll that contains the custom configure services method</param>
        public static void Setup(string assemblyFilePath, string configureDllPath)
        {
            // Help resolve any assembly references
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            // Add references from this assembly (CADBooster.SolidDna) including itself
            // to be resolved by the assembly resolver
            AddReferenceAssemblies<AddInIntegration>(includeSelf: true);

            // Always setup IoC on the normal app domain
            // This is so both sides of the application code can
            // so access the dependencies (like loggers)
            // even though they may be different instances
            SetupIoC(assemblyFilePath, configureDllPath);
        }

        #region IoC

        /// <summary>
        /// Configures the application IoC
        /// </summary>
        /// <param name="assemblyFilePath">The path to the assembly</param>
        /// <param name="pathToConfigureDll">Absolute path to dll where the IConfigureServices implementation lies</param>
        public static void SetupIoC(string assemblyFilePath, string pathToConfigureDll = null)
        {
            // Create default construction
            Framework.Construct(new DefaultFrameworkConstruction(configure =>
            {
                // If the add-in path is not null
                if (!string.IsNullOrEmpty(assemblyFilePath))
                    // Add configuration file for the name of this file
                    // For example if it is MyAddin.dll then the configuration file
                    // will be in the same folder called MyAddin.appsettings.json"
                    configure.AddJsonFile(Path.ChangeExtension(assemblyFilePath, "appsettings.json"), optional: true);
            }));

            // If we have a dll to try and find the configure method from...
            if (File.Exists(pathToConfigureDll))
            {
                // AddIn class type
                var addinType = typeof(AddInIntegration);

                // Load all methods...
                var match = Assembly.LoadFile(pathToConfigureDll).GetTypes()
                    // Find AddInIntegration parent class
                    .Where(f => f.IsSubclassOf(addinType))
                      .Select(t => (
                        // Store class
                        methodClass: t,
                        // Select the ConfigureServices method
                        method: t.GetMethod(nameof(AddInIntegration.ConfigureServices)))
                      )
                      // Only use first method for now
                      .FirstOrDefault();

                if (match.method != null)
                {
                    var classInstance = Activator.CreateInstance(match.methodClass, null);
                    match.method.Invoke(classInstance, new[] { Framework.Construction });
                }
            }

            // Build DI
            Framework.Construction.Build();

            // Log details
            Logger?.LogDebugSource($"DI Setup complete");
            Logger?.LogDebugSource($"Assembly File Path {assemblyFilePath}");
        }

        #endregion

        #endregion

        #region Assembly Resolve Methods

        /// <summary>
        /// Adds any reference assemblies to the assemblies that get resolved when loading assemblies
        /// based on the reference type. To add all references from a project, pass in any type that is
        /// contained in the project as the reference type
        /// </summary>
        /// <typeparam name="ReferenceType">The type contained in the assembly where the references are</typeparam>
        /// <param name="includeSelf">True to include the calling assembly</param>
        public static void AddReferenceAssemblies<ReferenceType>(bool includeSelf = false)
        {
            // Find all reference assemblies from the type
            var referencedAssemblies = typeof(ReferenceType).Assembly.GetReferencedAssemblies();

            // If there are any references
            if (referencedAssemblies?.Length > 0)
                // Add them
                mReferencedAssemblies.AddRange(referencedAssemblies);

            // If we should including calling assembly...
            if (includeSelf)
                // Add self
                mReferencedAssemblies.Add(typeof(ReferenceType).Assembly.GetName());
        }

        /// <summary>
        /// Attempts to resolve missing assemblies based on a list of known references
        /// primarily from SolidDna and the Add-in project itself
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            // Try and find a reference assembly that matches...
            var resolvedAssembly = mReferencedAssemblies.FirstOrDefault(f => string.Equals(f.FullName, args.Name, StringComparison.InvariantCultureIgnoreCase));

            // If we didn't find any assembly
            if (resolvedAssembly == null)
                // Return null
                return null;

            // If we found a match...
            try
            {
                // Try and load the assembly
                var assembly = Assembly.Load(resolvedAssembly.Name);

                // If it loaded...
                if (assembly != null)
                    // Return it
                    return assembly;

                // Otherwise, throw file not found
                throw new FileNotFoundException();
            }
            catch
            {
                //
                // Try to load by filename - split out the filename of the full assembly name
                // and append the base path of the original assembly (i.e. look in the same directory)
                //
                // NOTE: this doesn't account for special search paths but then that never
                //       worked before either
                //
                var parts = resolvedAssembly.Name.Split(',');
                var filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + parts[0].Trim() + ".dll";

                // Try and load assembly and let it throw FileNotFound if not there 
                // as it's an expected failure if not found
                return Assembly.LoadFrom(filePath);
            }
        }

        #endregion
    }
}
