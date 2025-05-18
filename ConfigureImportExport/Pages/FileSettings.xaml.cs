using ConfigureImportExport.Data;
using ConfigureImportExport.Models;
using ConfigureImportExport.Services;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

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
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (AppSettings != null)
            if (AppSettings.DBConnectionValid == true)
                DatabaseConnectionValid();
            else
                DatabaseConnectionInvalid();
    }

    private async void OnTestButtonClicked(object sender, EventArgs e)
    {
        //Check fields have values first
        if (await CheckInputs() == false)
        {
            return;
        }

        if (AppSettings != null)
            AppSettings.IsLoading = true;

        bool? fileSaved = false;
        try
        {
            if (AppSettings != null)
            {
                if (AppSettings?.CSVFile?.Folder?.Length > 0)
                {
                    // Check if the folder exists and create it if not
                    if (!Directory.Exists(AppSettings.CSVFile.Folder))
                    {
                        Directory.CreateDirectory(AppSettings.CSVFile.Folder);
                    }
                }

                // Check if the file name is valid
                string? folder = AppSettings?.CSVFile?.Folder ?? Path.GetDirectoryName(Environment.ProcessPath);
                string? fileName = AppSettings?.CSVFile?.FileName ?? "TestFilePath.csv";
                string testFilePath = Path.Combine(folder ?? "", fileName);
                using FileStream outputStream = File.OpenWrite(testFilePath);
                using StreamWriter streamWriter = new StreamWriter(outputStream);
                await streamWriter.WriteAsync("Test file for checking FTP access");
                Trace.WriteLine("Saved test file to: " + testFilePath);
                await streamWriter.DisposeAsync();
                await outputStream.DisposeAsync();
                fileSaved = true;

                File.Delete(testFilePath); // Delete the test file after checking

                await DisplayAlert("Success", "File Settings Are Valid!", "OK");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");

            if (fileSaved == true)
                await DisplayAlert("Success", $"File Settings Are Valid!.\r\nHowever the test file uploaded to the FTP could not be deleted locally from the folder\r\nYou may remove \"{AppSettings?.CSVFile?.FileName ?? "TestFilePath.csv"}\" manually\r\nUnexpected error:\r\n{ex.Message}", "OK");
            else
                await DisplayAlert("Error", $"An unexpected error occurred. Please try again.\r\nUnexpected error:\r\n{ex.Message}", "OK");
        }
        finally
        {
            if (AppSettings != null)
                AppSettings.IsLoading = false;
        }
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

    private async Task<bool?> CheckInputs()
    {
        //Check fields have values first
        string? ErrorMessage = "";

        if (AppSettings != null)
        {
            if (string.IsNullOrEmpty(AppSettings?.CSVFile?.FileName) && string.IsNullOrEmpty(AppSettings?.CSVFile?.ColumnNameAsFileName))
            {
                ErrorMessage += "Please enter a name for your CSV file or type the name of a column from your database table/view or a stored procedure which will name the file.\r\n";
            }
        }

        if (ErrorMessage.Length > 0)
        {
            await DisplayAlert("Error", ErrorMessage, "OK");
            return false;
        }

        return true;
    }

    public void DatabaseConnectionValid()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            EnableControls();
            FileSettingsMessage.Text = "✅ Database settings are valid. Please specify details of where to save the file to on the local computer.";
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