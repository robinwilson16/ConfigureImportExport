using ConfigureImportExport.Data;
using ConfigureImportExport.Models;
using ConfigureImportExport.Services;
using Microsoft.Extensions.Logging;

namespace ConfigureImportExport.Pages;

public partial class FileSettings : ContentPage
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<FileSettings> _logger;
    private readonly AppSettingsService _appSettingsService;

    public AppSettingsModel? AppSettings;

    public FileSettings(ApplicationDbContext dbContext, ILogger<FileSettings> logger, AppSettingsService appSettingsService)
	{
		InitializeComponent();
        _dbContext = dbContext;
        _logger = logger;
        _appSettingsService = appSettingsService;

        AppSettings = _appSettingsService?.Get();

        BindingContext = AppSettings;

        if (AppSettings != null)
            AppSettings.IsLoading = false;

        RuntimeSettingsService.HasUnsavedChanges = false;

        if (AppSettings != null)
            if (AppSettings.DBConnectionValid == true)
                DatabaseConnectionValid();
            else
                DatabaseConnectionInvalid();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        //Code here
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

    private async void OnRunButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnCancelButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnCloseButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    public void DatabaseConnectionValid()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            EnableControls();
            FileSettingsMessage.Text = "✅ Database settings are valid. Please specify the database object below to use";
            FileSettingsMessageBox.BackgroundColor = Color.FromArgb("#198754");
        });
    }

    public void DatabaseConnectionInvalid()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            DisableControls();
            FileSettingsMessage.Text = "⚠️ Please specify valid settings on the Database Settings screen first";
            FileSettingsMessageBox.BackgroundColor = Color.FromArgb("#fd7e14");
        });
    }

    public void EnableControls()
    {
        Folder.IsEnabled = true;
        FileName.IsEnabled = true;
        ColumnNameAsFileName.IsEnabled = true;
        IncludeHeaders.IsEnabled = true;
        Delimiter.IsEnabled = true;
        AlwaysWrapInSpeechmarks.IsEnabled = true;
        SaveButton.IsEnabled = true;
    }

    public void DisableControls()
    {
        Folder.IsEnabled = false;
        FileName.IsEnabled = false;
        ColumnNameAsFileName.IsEnabled = false;
        IncludeHeaders.IsEnabled = false;
        Delimiter.IsEnabled = true;
        AlwaysWrapInSpeechmarks.IsEnabled = true;
        SaveButton.IsEnabled = false;
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