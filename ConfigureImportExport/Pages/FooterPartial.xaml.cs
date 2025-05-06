using System.Reflection;
using ConfigureImportExport.Models;
using ConfigureImportExport.Services;

namespace ConfigureImportExport.Pages;

public partial class FooterPartial : ContentView
{
    public AppSettingsService AppSettingsService;
    public AppSettingsModel? AppSettings;

    public string? ProductVersion { get; set; }
	
	public FooterPartial()
	{
		InitializeComponent();

        AppSettingsService = new AppSettingsService();
        AppSettings = AppSettingsService?.Get();

        BindingContext = AppSettings;

        AppSettingsDetails.Text = "Using following appsettings.json file: " + AppSettingsService?.GetFilePath();

        ProductVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
		Version.Text = "v" + ProductVersion;

    }
}