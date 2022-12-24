using SolidWorks.Interop.swconst;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Types of a component.
    /// Is a subset of <see cref="swDocumentTypes_e"/>
    /// </summary>
    public enum ComponentTypes
    {
        /// <summary>
        /// Component is a part
        /// </summary>
        Part = 1,
        
        /// <summary>
        /// Component is a sub-assembly
        /// </summary>
        Assembly =2,
    }
}
