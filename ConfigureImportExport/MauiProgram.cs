using ConfigureImportExport.Data;
using ConfigureImportExport.Models;
using ConfigureImportExport.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;

namespace ConfigureImportExport
{
    public static class MauiProgram
    {
        public static AppSettingsService? AppSettingsService;
        public static AppSettingsModel? AppSettingsCurrent;

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Register other services
            builder.Services.AddSingleton<AppSettingsService>();
            builder.Services.AddSingleton<DbContextFactory>();

            // Register ApplicationDbContext with a valid connection string
            AppSettingsService = new AppSettingsService();
            AppSettingsCurrent = AppSettingsService?.Get();

            var conStrBuilder = new SqlConnectionStringBuilder
            {
                DataSource = AppSettingsCurrent?.DatabaseConnection?.Server,
                InitialCatalog = AppSettingsCurrent?.DatabaseConnection?.Database,
                UserID = AppSettingsCurrent?.DatabaseConnection?.Username,
                Password = AppSettingsCurrent?.DatabaseConnection?.Password,
                IntegratedSecurity = AppSettingsCurrent?.DatabaseConnection?.UseWindowsAuth ?? false,
                TrustServerCertificate = true,
                Encrypt = true,
                ConnectTimeout = 30,
                MultipleActiveResultSets = true,
                ApplicationName = "ConfigureImportExport"
            };
            var connectionString = conStrBuilder.ConnectionString;
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                //options.UseSqlServer($"Server={AppSettingsCurrent?.DatabaseConnection?.Server};Database={AppSettingsCurrent?.DatabaseConnection?.Database};User Id={AppSettingsCurrent?.DatabaseConnection?.Username};Password={AppSettingsCurrent?.DatabaseConnection?.Password};");
                options.UseSqlServer(connectionString);
            });

            builder.ConfigureLifecycleEvents(events =>
            {
#if WINDOWS
                events.AddWindows(windowsLifecycleBuilder =>
                {
                    windowsLifecycleBuilder.OnWindowCreated(window =>
                    {
                        if (window.Title== "Configuration Utility for File Import and Export Tools")
                        {
                            //get the Primary window title named "Configuration Utility for File Import and Export Tools"
                            var windows = window;
                            //use Microsoft.UI.Windowing functions for window
                            var handle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                            var id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
                            var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(id);

                           //When user execute the closing method, we can push a display alert. If user click Yes, close this application, if click the cancel, display alert will dismiss.
                            appWindow.Closing += async (s, e) =>
                            {
                                if (RuntimeSettingsService.HasUnsavedChanges == true)
                                {
                                    e.Cancel = true;
                                    //get all of Microsoft.Maui.Controls.windows.
                                    var windows1 = Application.Current.Windows.ToList<Microsoft.Maui.Controls.Window>();
                                    foreach (Window win in windows1) {
                                        //get the Primary window title named "Configuration Utility for File Import and Export Tools"
                                        if (win.Title== "Configuration Utility for File Import and Export Tools")
                                        {                    
                                            bool result = await win.Page.DisplayAlert(
                                            "Unsaved Changes",
                                            "You have unsaved changes.\r\n Are you sure you want to close the Configuration Utility for File Import and Export Tools without saving your changes?",
                                            "Yes",
                                            "Cancel");
                                           if (result)
                                            {
                                                //Application.Current.CloseWindow(win);
                                                //Close all windows
                                                Application.Current.Quit();
                                            }
                                        }
                                    }
                                }
                           };
                        }
      
                    });
                });
#endif

            });
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
