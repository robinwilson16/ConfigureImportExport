using ConfigureImportExport.Data;
using ConfigureImportExport.Models;
using ConfigureImportExport.Services;
using Microsoft.Extensions.Logging;

namespace ConfigureImportExport.Pages;

public partial class ExportSettings : ContentPage
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<ExportSettings> _logger;
    private readonly AppSettingsService _appSettingsService;

    public AppSettingsModel? AppSettings;

    public ExportSettings(ApplicationDbContext dbContext, ILogger<ExportSettings> logger, AppSettingsService appSettingsService)
	{
		InitializeComponent();
        _dbContext = dbContext;
        _logger = logger;
        _appSettingsService = appSettingsService;

        AppSettings = _appSettingsService?.Get();

        BindingContext = AppSettings;

        StoredProcedureNameChanged();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        //Code here
    }

    private void OnStoredProcedureNameChanged(object sender, EventArgs e)
    {
        StoredProcedureNameChanged();
    }

    private async void OnTestButtonClicked(object sender, EventArgs e)
    {
        if (AppSettings != null)
            AppSettings.IsLoading = true;

        AppSettingsModel? appSettings = await _appSettingsService.Set(AppSettings);

        if (AppSettings != null)
            AppSettings.IsLoading = false;
    }

    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        if (AppSettings != null)
            AppSettings.IsLoading = true;

        //AppSettings.ServerName = ServerName;
        //AppSettingsService appSettingsService = new AppSettingsService();
        AppSettingsModel? appSettings = await _appSettingsService.Set(AppSettings);

        if (AppSettings != null)
            AppSettings.IsLoading = false;
    }

    private async void OnCancelButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private void StoredProcedureNameChanged()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            if (AppSettings?.DatabaseTable?.StoredProcedureCommand?.Length > 0)
            {
                ExportSettingsGrid.RowDefinitions[7].Height = new GridLength(1, GridUnitType.Auto);
                ExportSettingsGrid.RowDefinitions[8].Height = new GridLength(1, GridUnitType.Auto);
                ExportSettingsGrid.RowDefinitions[9].Height = new GridLength(1, GridUnitType.Auto);
                ExportSettingsGrid.RowDefinitions[10].Height = new GridLength(1, GridUnitType.Auto);
            }
            else
            {
                ExportSettingsGrid.RowDefinitions[7].Height = 0;
                ExportSettingsGrid.RowDefinitions[8].Height = 0;
                ExportSettingsGrid.RowDefinitions[9].Height = 0;
                ExportSettingsGrid.RowDefinitions[10].Height = 0;
            }

            //Used to refresh the heights after changing them
            ExportSettingsGrid.InvalidateMeasure();
        });
    }

    public static void OnEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        if (RuntimeSettingsService.HasUnsavedChanges != true)
        {
            // Set HasUnsavedChanges to true
            RuntimeSettingsService.HasUnsavedChanges = true;

            // Optionally log or debug
            Console.WriteLine("Entry text changed. HasUnsavedChanges set to true.");
        }
    }
}