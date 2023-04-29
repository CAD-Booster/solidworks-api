using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CADBooster.SolidDna
{
    public static class Icons
    {
        #region Private members

        /// <summary>
        /// List of icon sizes used by SOLIDWORKS for the task pane and command manager.
        /// Icons are square, so these values are both width and height.
        /// </summary>
        private static readonly int[] mIconSizes = { 20, 32, 40, 64, 96, 128 };

        #endregion

        #region Public methods

        /// <summary>
        /// Get an array of full paths to a bmp or png's that contains the icon list 
        /// from first in the list being the smallest, to last being the largest
        /// NOTE: Supported sizes for each icon in an array is 20x20, 32x32, 40x40, 64x64, 96x96 and 128x128
        /// </summary>
        public static string[] GetArrayFromDictionary(Dictionary<int, string> iconListPaths) => iconListPaths.Values.ToArray();

        /// <summary>
        /// Get a dictionary with all icon paths based on a string format of the absolute path to the icon list images, replacing {0} with the size.
        /// For example C:\Folder\icons{0}.png would look for all sizes such as
        /// C:\Folder\icons.png
        /// C:\Folder\icons.png
        /// C:\Folder\icons.png
        /// ... and so on
        /// </summary>
        /// <param name="pathFormat">The absolute path, with {0} used to replace with the icon size</param>
        public static Dictionary<int, string> GetFormattedPathDictionary(string pathFormat)
        {
            // Create an empty dictionary
            var dictionary = new Dictionary<int, string>();

            // Make sure we have something
            if (pathFormat.IsNullOrWhiteSpace())
                return dictionary;

            // Fill the dictionary with all paths that exist
            foreach (var iconSize in mIconSizes)
            {
                var path = FormatPath(pathFormat, iconSize);
                if (File.Exists(path))
                    dictionary.Add(iconSize, path);
            }

            return dictionary;
        }

        /// <summary>
        /// Convert a single string with a format for an absolute path to an array of existing paths.
        /// </summary>
        /// <param name="pathFormat">The absolute path, with {0} used to replace with the icon size</param>
        /// <returns>An array of icon paths</returns>
        public static string[] GetPathArrayFromPathFormat(string pathFormat)
        {
            var iconPaths = new Dictionary<int, string>();
            foreach (var iconSize in mIconSizes)
            {
                // Replace "{0}" in the string with the icon size
                var path = FormatPath(pathFormat, iconSize);
                
                // Don't check if the path exists because SolidWorks does that for us. If all files don't exist and we return an empty array, the task pane is not created.
                iconPaths.Add(iconSize, path);
            }

            // Get icon paths from the dictionary
            return iconPaths.Values.ToArray();
        }

        #endregion

        /// <summary>
        /// Replace {0} in the path format string with the icon size.
        /// </summary>
        /// <param name="pathFormat"></param>
        /// <param name="iconSize"></param>
        /// <returns></returns>
        private static string FormatPath(string pathFormat, int iconSize)
        {
            // Make sure the path format contains "{0}"
            if (pathFormat == null || !pathFormat.Contains("{0}"))
                throw new SolidDnaException(SolidDnaErrors.CreateError(
                    SolidDnaErrorTypeCode.SolidWorksCommandManager,
                    SolidDnaErrorCode.SolidWorksCommandGroupInvalidPathFormatError));

            // Replace {0} with the icon size
            return string.Format(pathFormat, iconSize);
        }
    }
}
