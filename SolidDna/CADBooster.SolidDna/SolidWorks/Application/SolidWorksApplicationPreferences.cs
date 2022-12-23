using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace CADBooster.SolidDna
{
    public partial class SolidWorksApplication : SharedSolidDnaObject<SldWorks>
    {
        public class SolidWorksPreferences
        {
            /// <summary>
            /// Get the default assembly template path.
            /// </summary>
            public string DefaultAssemblyTemplate
            {
                get => SolidWorksEnvironment.Application.GetUserPreferencesString(swUserPreferenceStringValue_e.swDefaultTemplateAssembly);
                set => SolidWorksEnvironment.Application.SetUserPreferencesString(swUserPreferenceStringValue_e.swDefaultTemplateAssembly, value);
            }

            /// <summary>
            /// Get the default drawing template path.
            /// </summary>
            public string DefaultDrawingTemplate
            {
                get => SolidWorksEnvironment.Application.GetUserPreferencesString(swUserPreferenceStringValue_e.swDefaultTemplateDrawing);
                set => SolidWorksEnvironment.Application.SetUserPreferencesString(swUserPreferenceStringValue_e.swDefaultTemplateDrawing, value);
            }

            /// <summary>
            /// Get the default part template path.
            /// </summary>
            public string DefaultPartTemplate
            {
                get => SolidWorksEnvironment.Application.GetUserPreferencesString(swUserPreferenceStringValue_e.swDefaultTemplatePart);
                set => SolidWorksEnvironment.Application.SetUserPreferencesString(swUserPreferenceStringValue_e.swDefaultTemplatePart, value);
            }

            /// <summary>
            /// The scaling factor used when exporting as DXF
            /// </summary>
            public double DxfOutputScaleFactor
            {
                get => SolidWorksEnvironment.Application.GetUserPreferencesDouble(swUserPreferenceDoubleValue_e.swDxfOutputScaleFactor);
                set => SolidWorksEnvironment.Application.SetUserPreferencesDouble(swUserPreferenceDoubleValue_e.swDxfOutputScaleFactor, value);
            }

            /// <summary>
            /// The scaling factor used when exporting as DXF
            /// </summary>
            public int DxfMultiSheetOption
            {
                get => SolidWorksEnvironment.Application.GetUserPreferencesInteger(swUserPreferenceIntegerValue_e.swDxfMultiSheetOption);
                set => SolidWorksEnvironment.Application.SetUserPreferencesInteger(swUserPreferenceIntegerValue_e.swDxfMultiSheetOption, value);
            }

            /// <summary>
            /// The scaling of DXF output. If true no scaling will be done
            /// </summary>
            public bool DxfOutputNoScale
            {
                get => SolidWorksEnvironment.Application.GetUserPreferencesInteger(swUserPreferenceIntegerValue_e.swDxfOutputNoScale) == 1;
                set => SolidWorksEnvironment.Application.SetUserPreferencesInteger(swUserPreferenceIntegerValue_e.swDxfOutputNoScale, value ? 1 : 0);
            }
        }
    }
}
