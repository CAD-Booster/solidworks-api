using SolidWorks.Interop.sldworks;
using System;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Exposes all Part Document calls from a <see cref="Model"/>
    /// </summary>
    public class PartDocument
    {
        #region Protected Members

        /// <summary>
        /// The base model document. Note we do not dispose of this (the parent Model will)
        /// </summary>
        protected PartDoc mBaseObject;

        #endregion

        #region Public Properties

        /// <summary>
        /// The raw underlying COM object
        /// WARNING: Use with caution. You must handle all disposal from this point on
        /// </summary>
        public PartDoc UnsafeObject => mBaseObject;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public PartDocument(PartDoc model)
        {
            mBaseObject = model;
        }

        #endregion

        #region Feature Methods
        /// <summary>
        /// Gets the <see cref="ModelFeature"/> of the item in the feature tree based on its name.
        /// Returns the actual model feature or null when not found.
        /// </summary>
        /// <param name="featureName">Name of the feature</param>
        /// <returns>The <see cref="ModelFeature"/> for the named feature</returns>
        public ModelFeature GetFeatureByName(string featureName)
        {
            // Wrap any error
            return SolidDnaErrors.Wrap(() => GetModelFeatureByNameOrNull(featureName),
                SolidDnaErrorTypeCode.SolidWorksModel,
                SolidDnaErrorCode.SolidWorksModelAssemblyGetFeatureByNameError);
        }

        /// <summary>
        /// Gets the <see cref="ModelFeature"/> of the item in the feature tree based on its name and perform a function on it.
        /// </summary>
        /// <param name="featureName">Name of the feature</param>
        /// <param name="function">The function to perform on this feature</param>
        /// <returns>The <see cref="ModelFeature"/> for the named feature</returns>
        public T GetFeatureByName<T>(string featureName, Func<ModelFeature, T> function)
        {
            // Wrap any error
            return SolidDnaErrors.Wrap(() =>
            {
                // Create feature
                using (var modelFeature = GetModelFeatureByNameOrNull(featureName))
                {
                    // Run function
                    return (T)function.Invoke(modelFeature);
                }
            },
                SolidDnaErrorTypeCode.SolidWorksModel,
                SolidDnaErrorCode.SolidWorksModelAssemblyGetFeatureByNameError);
        }

        /// <summary>
        /// Gets the <see cref="ModelFeature"/> of the item in the feature tree based on its name and perform an action on it.
        /// </summary>
        /// <param name="featureName">Name of the feature</param>
        /// <param name="action">The action to perform on this feature</param>
        /// <returns>The <see cref="ModelFeature"/> for the named feature</returns>
        public void GetFeatureByName(string featureName, Action<ModelFeature> action)
        {
            // Wrap any error
            SolidDnaErrors.Wrap(() =>
            {
                // Create feature
                using (var modelFeature = GetModelFeatureByNameOrNull(featureName))
                {
                    // Run action
                    action(modelFeature);
                }
            },
                SolidDnaErrorTypeCode.SolidWorksModel,
                SolidDnaErrorCode.SolidWorksModelAssemblyGetFeatureByNameError);
        }

        /// <summary>
        /// Get the <see cref="ModelFeature"/> of the item in the feature tree based on its name.
        /// Returns the actual model feature or null when not found.
        /// </summary>
        /// <param name="featureName"></param>
        /// <returns></returns>
        private ModelFeature GetModelFeatureByNameOrNull(string featureName)
        {
            var feature = (Feature)mBaseObject.FeatureByName(featureName);
            return feature == null ? null : new ModelFeature(feature);
        }


        #endregion
    }
}
