using ConfigureImportExport.Models;
using DashboardApp.Services;

namespace ConfigureImportExport.Pages;

public partial class ExportSettings : ContentPage
{
    public AppSettingsService AppSettingsService;
    public AppSettingsModel? AppSettings;

    public ExportSettings()
	{
		InitializeComponent();

        AppSettingsService = new AppSettingsService();
        AppSettings = AppSettingsService?.Get();

        BindingContext = AppSettings;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        //Code here
    }

    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        //AppSettings.ServerName = ServerName;
        //AppSettingsService appSettingsService = new AppSettingsService();
        AppSettingsModel? appSettings = await AppSettingsService.Set(AppSettings);
    }
}