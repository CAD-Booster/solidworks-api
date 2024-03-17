using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Exposes all Assembly Document calls from a <see cref="Model"/>
    /// </summary>
    public class AssemblyDocument
    {
        #region Protected Members

        /// <summary>
        /// The base model document. Note we do not dispose of this (the parent Model will)
        /// </summary>
        protected AssemblyDoc mBaseObject;

        #endregion

        #region Public Properties

        /// <summary>
        /// The raw underlying COM object
        /// WARNING: Use with caution. You must handle all disposal from this point on
        /// </summary>
        public AssemblyDoc UnsafeObject => mBaseObject;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public AssemblyDocument(AssemblyDoc model)
        {
            mBaseObject = model;
        }

        #endregion

        #region Component methods

        /// <summary>
        /// Set the suppression state for a list of components in a certain configuration.
        /// You cannot set components as Lightweight with this method.
        /// </summary>
        /// <param name="components"></param>
        /// <param name="state"></param>
        /// <param name="configurationOption"></param>
        /// <param name="configurationName">If you select <see cref="ModelConfigurationOptions.SpecificConfiguration"/>, pass the configuration name here.</param>
        /// <returns>True if successful, false if it fails or if the list is empty</returns>
        public bool SetComponentSuppressionState(List<Component> components, ComponentSuppressionStates state,
            ModelConfigurationOptions configurationOption = ModelConfigurationOptions.ThisConfiguration, string configurationName = null)
        {
            // Check if there are components in the list
            if (!components.Any()) return false;

            // Convert the list of SolidDna Components into an array of SolidWorks IComponent2
            var swComponents = components.Select(x => x.UnsafeObject).ToArray();

            // Change the suppression state
            return mBaseObject.SetComponentState((int)state, swComponents, (int)configurationOption, configurationName, true);
        }

        /// <summary>
        /// Set the configuration for a file that was just dropped into the assembly.
        /// Use this method after receiving a <see cref="Model.FileDropped"/> event to set the configuration name of the dropped model.
        /// </summary>
        /// <param name="configurationName"></param>
        /// <returns>True if successful</returns>
        public bool SetConfigurationForDroppedFile(string configurationName) => UnsafeObject.SetDroppedFileConfigName(configurationName);

        #endregion

        #region Feature Methods

        /// <summary>
        /// Get the <see cref="ModelFeature"/> of the item in the feature tree based on its name.
        /// Only works for features in the root assembly, not in sub-assemblies.
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
        /// Get the <see cref="ModelFeature"/> of the item in the feature tree based on its name and perform a function on it.
        /// Only works for features in the root assembly, not in sub-assemblies.
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
                        return (T) function.Invoke(modelFeature);
                    }
                },
                SolidDnaErrorTypeCode.SolidWorksModel,
                SolidDnaErrorCode.SolidWorksModelAssemblyGetFeatureByNameError);
        }

        /// <summary>
        /// Gets the <see cref="ModelFeature"/> of the item in the feature tree based on its name and perform an action on it.
        /// Only works for features in the root assembly, not in sub-assemblies.
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
        /// Only works for features in the root assembly, not in sub-assemblies.
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
