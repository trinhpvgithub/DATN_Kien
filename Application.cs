using DATN_Kien.Commands;
using Nice3point.Revit.Toolkit.External;

namespace DATN_Kien
{
    /// <summary>
    ///     Application entry point
    /// </summary>
    [UsedImplicitly]
    public class Application : ExternalApplication
    {
        public override void OnStartup()
        {
            CreateRibbon();
        }

        private void CreateRibbon()
        {
            var panel = Application.CreatePanel("Commands", "DATN_Kien");

            panel.AddPushButton<StartupCommand>("Execute")
                .SetImage("/DATN_Kien;component/Resources/Icons/RibbonIcon16.png")
                .SetLargeImage("/DATN_Kien;component/Resources/Icons/RibbonIcon32.png");
        }
    }
}