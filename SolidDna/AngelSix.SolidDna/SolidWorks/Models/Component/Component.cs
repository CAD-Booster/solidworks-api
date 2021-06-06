using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Collections.Generic;
using System.Linq;

namespace AngelSix.SolidDna
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
        /// The complete path to the component. Can be null.
        /// </summary>
        public string FilePath => BaseObject.GetPathName();

        /// <summary>
        /// Check if this sub-assembly is flexible.
        /// </summary>
        public bool IsFlexible => BaseObject.Solving == (int)swComponentSolvingOption_e.swComponentFlexibleSolving;

        /// <summary>
        /// Check if the Component is Root
        /// </summary>
        public bool IsRoot => BaseObject.IsRoot();

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
        /// Get the name of the component
        /// </summary>
        public string Name => BaseObject.Name2;

        /// <summary>
        /// Get the parent component for this component.
        /// Is null for the root component and for top-level components.
        /// </summary>
        public Component Parent => BaseObject.GetParent() == null ? null : new Component(BaseObject.GetParent());

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public Component(Component2 component) : base(component)
        {

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
