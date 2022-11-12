using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AngelSix.SolidWorksApi.AddinInstaller
{
    public static class RegistryHelpers
    {
        /// <summary>
        /// Find all add-ins that are currently installed, and their properties.
        /// </summary>
        /// <returns></returns>
        public static ObservableCollection<AddInProperties> GetAddInPropertiesList()
        {
            const RegistryHive hive = RegistryHive.LocalMachine;
            const RegistryView view = RegistryView.Registry64;
            const string keyPath = "SOFTWARE\\SolidWorks\\AddIns";

            var addInProperties = new List<AddInProperties>();
            using (var registryKey = RegistryKey.OpenBaseKey(hive, view).OpenSubKey(keyPath))
            {
                if (registryKey == null)
                    return new ObservableCollection<AddInProperties>();

                foreach (var subKeyName in registryKey.GetSubKeyNames())
                {
                    using (var key = registryKey.OpenSubKey(subKeyName))
                    {
                        var title = (string) key?.GetValue("Title");

                        // The Presentation Manager can be listed multiple times and it's not listed in the add-in window, so we skip it.
                        if (title == null || title.Equals("Presentation Manager"))
                            continue;

                        // Use the key name, which contains the GUID of the add-in, to get the path to the add-in DLL.
                        var path = GetPath(subKeyName);
                        addInProperties.Add(new AddInProperties(path, title));
                    }
                }
            }

            return new ObservableCollection<AddInProperties>(addInProperties);
        }

        /// <summary>
        /// Find the path of an add-in using its GUID.
        /// The CLSID registry key stores the properties of all registered classes, so we use that to look up the add-in path.
        /// </summary>
        /// <param name="guidWithBraces"></param>
        private static string GetPath(string guidWithBraces)
        {
            const RegistryHive hive = RegistryHive.ClassesRoot;
            const RegistryView view = RegistryView.Registry64;
            const string pathUnknown = "Path unknown";

            using (var registryKey = RegistryKey.OpenBaseKey(hive, view).OpenSubKey("CLSID"))
            {
                if (registryKey == null)
                    return pathUnknown;

                using (var key = registryKey.OpenSubKey($"{guidWithBraces}\\InprocServer32"))
                {
                    var value = (string) key?.GetValue("CodeBase");
                    if (value == null || value.Length <= 8) 
                        return pathUnknown;

                    // The value contains the path but has the prefix "file:///", so we remove the prefix first
                    return value.Substring(8);
                }
            }
        }
    }
}
