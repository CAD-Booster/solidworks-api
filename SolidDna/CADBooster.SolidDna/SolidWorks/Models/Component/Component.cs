using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Collections.Generic;
using System.Linq;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Represents a component (Part or Assembly) in a SolidWorks assembly.
    /// An assembly has a root component, which is the assembly itself.
    /// Each component can have root components. The structure is the same as you see in the feature tree.
    /// </summary>
    public class Component : SolidDnaObject<Component2>
    {
        #region Public Properties

        /// <summary>
        /// Get the Model from the component
        /// </summary>
        public Model AsModel => new Model (BaseObject.GetModelDoc2() as ModelDoc2);

        /// <summary>
        /// Get children from this Component
        /// </summary>
        public List<Component> Children => ((object[])BaseObject.GetChildren())?.Cast<Component2>().Select(x => new Component(x)).ToList() ??
                                           new List<Component>();

        /// <summary>
        /// Get the real name of the component, without the sub-assembly name and without instance numbers.
        /// </summary>
        public string CleanName
        {
            get
            {
                var nameWithInstanceNumber = Name;
                var nameWithoutInstanceNumber = nameWithInstanceNumber.LastIndexOf('-') == -1
                    ? nameWithInstanceNumber
                    : nameWithInstanceNumber.Remove(nameWithInstanceNumber.LastIndexOf('-'));

                return nameWithoutInstanceNumber.LastIndexOf('/') == -1
                    ? nameWithoutInstanceNumber
                    : nameWithoutInstanceNumber.Substring(nameWithoutInstanceNumber.LastIndexOf('/') + 1);
            }
        }

        /// <summary>
        /// The name of the configuration for this component.
        /// </summary>
        public string ConfigurationName
        {
            get => BaseObject.ReferencedConfiguration;
            set => BaseObject.ReferencedConfiguration = value;
        } 

        /// <summary>
        /// The name of the display state for this component.
        /// </summary>
        public string DisplayStateName
        {
            get => BaseObject.ReferencedDisplayState2;
            set => BaseObject.ReferencedDisplayState2 = value;
        }

        /// <summary>
        /// The complete path to the component.
        /// </summary>
        public string FilePath => BaseObject.GetPathName();

        /// <summary>
        /// True if this component is an assembly
        /// </summary>
        public bool IsAssembly => ModelType == ComponentTypes.Assembly;

        /// <summary>
        /// Check if this sub-assembly is flexible.
        /// </summary>
        public bool IsFlexible => BaseObject.Solving == (int)swComponentSolvingOption_e.swComponentFlexibleSolving;

        /// <summary>
        /// True if this component is a part
        /// </summary>
        public bool IsPart => ModelType == ComponentTypes.Part;

        /// <summary>
        /// Check if the Component is the root component.
        /// In an assembly, this is the assembly itself. In a part, this is the part itself.
        /// Not all methods return useful results when the component is the root.
        /// </summary>
        public bool IsRoot => BaseObject.IsRoot();

        /// <summary>
        /// Check if the component is suppressed in the current assembly configuration. Call <see cref="Suppress"/> or <see cref="Unsuppress"/> to change the state of this component.
        /// </summary>
        public bool IsSuppressed => BaseObject.IsSuppressed();

        /// <summary>
        /// Check if the component is a virtual component.
        /// Virtual components are saved within the assembly, not to a separate file.
        /// </summary>
        public bool IsVirtual => BaseObject.IsVirtual;

        /// <summary>
        /// Check if this component is visible. Returns false when the visibility is hidden or unknown.
        /// </summary>
        public bool IsVisible
        {
            get => BaseObject.Visible == (int)swComponentVisibilityState_e.swComponentVisible;
            set => BaseObject.Visible = (int)(value
                ? swComponentVisibilityState_e.swComponentVisible
                : swComponentVisibilityState_e.swComponentHidden);
        }

        /// <summary>
        /// Get the type of component, either a part or an assembly.
        /// </summary>
        public ComponentTypes ModelType => FilePath.ToLower().EndsWith(".sldprt")
            ? ComponentTypes.Part
            : ComponentTypes.Assembly;

        /// <summary>
        /// Get the name of the component
        /// </summary>
        public string Name => BaseObject.Name2;

        /// <summary>
        /// Get the parent component for this component. Is null for the root component.
        /// </summary>
        public Component Parent
        {
            get
            {
                // The root component does not have a parent
                if (IsRoot) return null;
                
                // Get the parent component. If we are working with a top-level component, this returns null
                var parentComponent = BaseObject.GetParent();
                if (parentComponent != null)
                {
                    // This is not a top-level component
                    return new Component(parentComponent);
                }

                // This is a top-level component. SolidWorks returns null for its parent, but we will find the root component, the real parent.
                var assemblyModel = GetParentAssembly();

                // Get the root component via the active configuration
                var rootComponent = assemblyModel.ActiveConfiguration.UnsafeObject.GetRootComponent3(true);

                // Wrap the root IComponent2 as a Component
                return rootComponent == null ? null : new Component(rootComponent);
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public Component(Component2 component) : base(component)
        {

        }

        #endregion

        #region Flexible / rigid component

        /// <summary>
        /// Select the sub-assembly component and mark it as flexible.
        /// </summary>
        /// <returns>True if successful</returns>
        public bool SetFlexible() => SetFlexibleRigid(swComponentSolvingOption_e.swComponentFlexibleSolving);

        /// <summary>
        /// Select the sub-assembly component and mark it as rigid.
        /// </summary>
        /// <returns>True if successful</returns>
        public bool SetRigid() => SetFlexibleRigid(swComponentSolvingOption_e.swComponentRigidSolving);

        /// <summary>
        /// Select the sub-assembly component and mark it as rigid or flexible.
        /// </summary>
        /// <param name="solving"></param>
        /// <returns>True if successful</returns>
        private bool SetFlexibleRigid(swComponentSolvingOption_e solving)
        {
            return SolidDnaErrors.Wrap(() =>
                {
                    // Make sure the component is a sub-assembly
                    if (ModelType != ComponentTypes.Assembly || IsRoot)
                        return false;

                    // Make sure the active model is an assembly
                    var assemblyModel = GetParentAssembly();
                    if (assemblyModel.UnsafeObject == null || !assemblyModel.IsAssembly)
                        return false;

                    // Select the component
                    var selected = BaseObject.Select4(false, null, false);
                    if (!selected)
                        return false;

                    // Get the current component suppression state so we can reuse it
                    var suppressionState = (swComponentSuppressionState_e)BaseObject.GetSuppression();

                    // Call the assembly to mark the selected component as rigid/flexible.
                    // Use as many existing properties and methods as possible so we only change the rigid/flexible setting
                    return assemblyModel.AsAssembly().CompConfigProperties5((int)suppressionState, (int)solving,
                        IsVisible, false, ConfigurationName, BaseObject.ExcludeFromBOM, BaseObject.IsEnvelope());
                }, SolidDnaErrorTypeCode.SolidWorksModel, SolidDnaErrorCode.SolidWorksModelAssemblyComponentRigidFlexibleError);
        }

        #endregion

        #region Suppress / unsuppress component

        /// <summary>
        /// Suppress this component in the current assembly configuration.
        /// </summary>
        /// <returns>Result enum</returns>
        public ComponentSuppressionResults Suppress() => (ComponentSuppressionResults)BaseObject.SetSuppression2((int)swComponentSuppressionState_e.swComponentSuppressed);

        /// <summary>
        /// Unsuppress this component in the current assembly configuration.
        /// </summary>
        /// <returns>Result enum</returns>
        public ComponentSuppressionResults Unsuppress() => (ComponentSuppressionResults)BaseObject.SetSuppression2((int)swComponentSuppressionState_e.swComponentResolved);

        #endregion

        #region Get assembly from component

        /// <summary>
        /// Get the assembly that owns this component. A bit hacky but it works.
        /// </summary>
        /// <returns></returns>
        public Model GetParentAssembly()
        {
            // Get the string to select this component. The string ends with the name of the root assembly
            var selectByIdString = BaseObject.GetSelectByIDString();

            // Get the assembly name from the bit after the last @ symbol. Add the assembly extension, just to make sure
            var lastIndex = selectByIdString.LastIndexOf('@');
            var rootAssemblyName = selectByIdString.Substring(lastIndex + 1) + ".sldasm";

            // Get the assembly model by its name. Should never be null because the top-level assembly model is always loaded.
            var modelDoc = SolidWorksEnvironment.Application.UnsafeObject.GetOpenDocument(rootAssemblyName);
            return new Model(modelDoc);
        }

        #endregion

        #region ToString

        /// <summary>
        /// Returns a user-friendly string with component properties.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Name: {Name}. Is root: {IsRoot}";
        }

        #endregion

        #region Dispose

        public override void Dispose()
        {
            // Clean up embedded objects
            AsModel?.Dispose();

            // Dispose self
            base.Dispose();
        }

        #endregion
    }
}
