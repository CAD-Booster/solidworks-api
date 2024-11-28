using SolidWorks.Interop.swconst;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// The connection status of a 3DExperience connection.
    /// Same values as <see cref="sw3DExperienceState_e"/>.
    /// </summary>
    public enum ConnectionStatus3DExperience
    {
        /// <summary>
        /// Program is Desktop SolidWorks without a 3DExperience connection.
        /// </summary>
        None = 0,

        /// <summary>
        /// Program is connected to 3DExperience.
        /// </summary>
        Online = 1,

        /// <summary>
        /// Program is not connected to 3DExperience.
        /// </summary>
        Offline = 2,
    }
}
