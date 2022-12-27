using SolidWorks.Interop.swconst;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// The opens for a document type used in calls such as <see cref="AssemblyDocument.SetComponentSuppressionState"/>.
    /// From <see cref="swComponentSuppressionState_e"/>
    /// </summary>
    public enum ComponentSuppressionStates
    {
        /// <summary>
        /// Fully suppressed, including child components.
        /// </summary>
        Suppressed = 0,
        
        /// <summary>
        /// Makes only this component Lightweight
        /// </summary>
        Lightweight = 1,
        
        /// <summary>
        /// Fully resolved, so including all child components
        /// </summary>
        ResolvedWithChildren = 2,
        
        /// <summary>
        /// Makes only this component Resolved
        /// </summary>
        Resolved = 3,
        
        /// <summary>
        /// Fully Lightweight, so including all child components
        /// </summary>
        LightweightWithChildren = 4,
        
        /// <summary>
        /// The ID of the component does not match the the ID the component had when the assembly was last saved.
        /// </summary>
        InternalIdMismatch = 5
    }
}