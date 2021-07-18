using CADBooster.SolidDna;
using System.IO;

namespace SolidDna.WpfAddIn
{
    /// <summary>
    /// Register as a SolidWorks Add-in
    /// </summary>
    public class MyAddinIntegration : SolidAddIn
    {
        /// <summary>
        /// Specific application start-up code
        /// </summary>
        public override void ApplicationStartup()
        {

        }

        /// <summary>
        /// Steps to take before any add-ins load
        /// </summary>
        /// <returns></returns>
        public override void PreLoadPlugIns()
        {

        }

        public override void PreConnectToSolidWorks()
        {

        }
    }

    /// <summary>
    /// My first SolidDna Plug-in
    /// </summary>
    public class MySolidDnaPlugin : SolidPlugIn
    {
        #region Private Members

        /// <summary>
        /// The Taskpane UI for our plug-in
        /// </summary>
        private TaskpaneIntegration<MyTaskpaneUI, MyAddinIntegration> mTaskpane;

        #endregion

        #region Public Properties

        /// <summary>
        /// My Add-in description
        /// </summary>
        public override string AddInDescription => "My Addin Description";

        /// <summary>
        /// My Add-in title
        /// </summary>
        public override string AddInTitle => "My Addin Title";

        #endregion

        #region Connect To SolidWorks

        public override void ConnectedToSolidWorks()
        {
            // Create our taskpane
            mTaskpane = new TaskpaneIntegration<MyTaskpaneUI, MyAddinIntegration>
            {
                Icon = Path.Combine(this.AssemblyPath(), "logo-small.bmp"),
                WpfControl = new MyAddinControl()
            };

            mTaskpane.AddToTaskpaneAsync();
        }

        public override void DisconnectedFromSolidWorks()
        {

        }

        #endregion
    }
}
