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

        #endregion

        #region Feature Methods

        /// <summary>
        /// Gets the <see cref="ModelFeature"/> of the item in the feature tree based on its name
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
                using (var modelFeature = new ModelFeature((Feature)mBaseObject.FeatureByName(featureName)))
                {
                    // Run action
                    action(modelFeature);
                }
            },
                SolidDnaErrorTypeCode.SolidWorksModel,
                SolidDnaErrorCode.SolidWorksModelAssemblyGetFeatureByNameError);
        }

        #endregion
    }
}
