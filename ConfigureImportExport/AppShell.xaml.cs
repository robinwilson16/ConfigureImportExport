namespace ConfigureImportExport
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            //Register Routes to Pages for Navigation
            Routing.RegisterRoute(nameof(Pages.About), typeof(Pages.About));
            Routing.RegisterRoute(nameof(Pages.DatabaseSettings), typeof(Pages.DatabaseSettings));
            Routing.RegisterRoute(nameof(Pages.ExportSettings), typeof(Pages.ExportSettings));
            Routing.RegisterRoute(nameof(Pages.FileSettings), typeof(Pages.FileSettings));
            Routing.RegisterRoute(nameof(Pages.FTPSettings), typeof(Pages.FTPSettings));
            Routing.RegisterRoute(nameof(Pages.Welcome), typeof(Pages.Welcome));
        }
    }
}
