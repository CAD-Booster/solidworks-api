using System;
using System.Diagnostics;
using System.IO;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// A basic implementation of the AddIn Integration class used when registering the dll for COM.
    /// Mainly used for setting up DI so when loading the plugins they have the expected services
    /// </summary>
    public class ComRegisterSolidAddIn : SolidAddIn
    {
        public ComRegisterSolidAddIn()
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Debugger.Break();

                // Fall-back just write a static log directly
                File.AppendAllText(Path.ChangeExtension(this.AssemblyFilePath(), "fatal.log.txt"), $"\r\nUnexpected error: {ex}");
            }
        }

        public override void ApplicationStartup()
        {

        }

        public override void PreConnectToSolidWorks()
        {

        }

        public override void PreLoadPlugIns()
        {

        }
    }
}
