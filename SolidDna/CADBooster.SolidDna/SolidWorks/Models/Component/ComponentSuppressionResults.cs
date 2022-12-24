using SolidWorks.Interop.swconst;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Return value when you try to (un)suppress a single component with <see cref="Component.Suppress"/> or <see cref="Component.Unsuppress"/>.
    /// From <see cref="swSuppressionError_e"/>
    /// </summary>
    public enum ComponentSuppressionResults
    {
        /// <summary>
        /// The component object is no longer valid; for example if a configuration changed
        /// </summary>
        ComponentNoLongerValid = 0,

        /// <summary>
        /// An invalid state was specified
        /// </summary>
        InvalidStateSpecified = 1,

        /// <summary>
        /// The component state was changed successfully
        /// </summary>
        Success = 2,

        /// <summary>
        /// The component state failed to change even though the arguments were ok
        /// </summary>
        Failed = 3
    }
}
