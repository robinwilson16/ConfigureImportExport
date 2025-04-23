using ConfigureImportExport.Models;
using DashboardApp.Services;

namespace ConfigureImportExport.Pages;

public partial class DatabaseSettings : ContentPage
{
    public AppSettingsService AppSettingsService;
    public AppSettings? AppSettings;
    public DatabaseSettings()
    {
        InitializeComponent();

        AppSettingsService = new AppSettingsService();
        AppSettings = AppSettingsService?.Get();

        BindingContext = AppSettings;
    }

    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        //AppSettings.ServerName = ServerName;
        //AppSettingsService appSettingsService = new AppSettingsService();
        AppSettings? appSettings = await AppSettingsService.Set(AppSettings);
    }
}