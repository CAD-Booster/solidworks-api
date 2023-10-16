using CADBooster.SolidDna;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static CADBooster.SolidDna.SolidWorksEnvironment;

namespace SolidDna.Exporting
{
    /// <summary>
    /// Register as a SolidWorks Add-in
    /// </summary>
    [Guid("6D769D97-6103-4495-AACD-63CDD0EC396B"), ComVisible(true)]  // Replace the GUID with your own.
    public class SolidDnaAddinIntegration : SolidAddIn
    {
        /// <summary>
        /// Specific application start-up code
        /// </summary>
        public override void ApplicationStartup()
        {

        }

        public override void PreLoadPlugIns()
        {

        }

        public override void PreConnectToSolidWorks()
        {

        }
    }

    /// <summary>
    /// Register as SolidDna Plug-in
    /// </summary>
    public class MySolidDnaPlugin : SolidPlugIn
    {
        #region Public Properties

        /// <summary>
        /// My Add-in description
        /// </summary>
        public override string AddInDescription => "An example of Command Items and exporting";

        /// <summary>
        /// My Add-in title
        /// </summary>
        public override string AddInTitle => "SolidDNA Exporting";

        #endregion

        #region Connect To SolidWorks

        public override void ConnectedToSolidWorks()
        {
            // Part commands.
            // You don't need to use the return value, but it's there if you want to.
            var partGroup = Application.CommandManager.CreateCommandTab(
                title: "Export Part",
                id: 120_000,
                commandManagerItems: new List<ICommandManagerItem>{
                    new CommandManagerItem {
                        Name = "DXF",
                        Tooltip = "DXF",
                        ImageIndex = 0,
                        Hint = "Export part as DXF",
                        VisibleForDrawings = false,
                        VisibleForAssemblies = false,
                        OnClick = FileExporting.ExportPartAsDxf
                    },

                    new CommandManagerItem {
                        Name = "STEP",
                        Tooltip = "STEP",
                        ImageIndex = 2,
                        Hint = "Export part as STEP",
                        VisibleForDrawings = false,
                        VisibleForAssemblies = false,
                        OnClick = FileExporting.ExportModelAsStep
                    },
                },
                mainIconPathFormat: "icons{0}.png",
                iconListsPathFormat: "icons{0}.png");

            // Assembly commands
            var assemblyGroup = Application.CommandManager.CreateCommandTab(
                title: "Export Assembly",
                id: 120_001,
                commandManagerItems: new List<ICommandManagerItem> {
                    new CommandManagerItem {
                        Name = "STEP",
                        Tooltip = "STEP",
                        ImageIndex = 2,
                        Hint = "Export assembly as STEP",
                        VisibleForDrawings = false,
                        VisibleForParts = false,
                        OnClick = FileExporting.ExportModelAsStep
                    },
                },
                mainIconPathFormat: "icons{0}.png",
                iconListsPathFormat: "icons{0}.png");

            // Drawing commands
            var drawingGroup = Application.CommandManager.CreateCommandTab(
                title: "Export Drawing",
                id: 120_002,
                commandManagerItems: new List<ICommandManagerItem>{

                    new CommandManagerItem {
                        Name = "PDF",
                        Tooltip = "PDF",
                        Hint = "Export drawing as PDF",
                        ImageIndex = 1,
                        VisibleForParts = false,
                        VisibleForAssemblies = false,
                        OnClick = FileExporting.ExportDrawingAsPdf
                    },

                },
                mainIconPathFormat: "icons{0}.png",
                iconListsPathFormat: "icons{0}.png");
        }

        public override void DisconnectedFromSolidWorks()
        {

        }

        #endregion
    }
}
