using ConfigureImportExport.Data;
using ConfigureImportExport.Models;
using ConfigureImportExport.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (AppSettings != null)
            if (AppSettings.DBConnectionValid == true)
                DatabaseConnectionValid();
            else
                DatabaseConnectionInvalid();

        StoredProcedureNameChanged();
    }

    private void OnStoredProcedureNameChanged(object sender, EventArgs e)
    {
        StoredProcedureNameChanged();
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

        try
        {
            if (AppSettings != null)
            {
                if (!string.IsNullOrEmpty(AppSettings.DatabaseTable?.TableOrView))
                {
                    await App.DBContext.Database.ExecuteSqlAsync($"SELECT 1 FROM [{AppSettings?.DatabaseTable?.TableOrView}]");
                }
                else if (!string.IsNullOrEmpty(AppSettings.DatabaseTable?.StoredProcedureCommand))
                {
                    //await App.DBContext.Database.ExecuteSqlAsync(@$"
                    //    SELECT 1
                    //    FROM [{AppSettings?.DatabaseTable?.Database}].INFORMATION_SCHEMA.ROUTINES R
                    //    WHERE 
                    //     R.ROUTINE_TYPE = 'PROCEDURE'
                    //     AND R.ROUTINE_SCHEMA = '{AppSettings?.DatabaseTable?.Schema ?? "dbo"}'
                    //     AND R.ROUTINE_NAME = '{AppSettings?.DatabaseTable?.StoredProcedureCommand}'");

                    var sql = $@"
                        SELECT 1 AS Result
                        FROM [{AppSettings.DatabaseTable.Database}].INFORMATION_SCHEMA.ROUTINES
                        WHERE ROUTINE_TYPE = 'PROCEDURE'
                          AND ROUTINE_SCHEMA = {{0}}
                          AND ROUTINE_NAME = {{1}}";

                    var exists = await _dbContext.Set<DatabaseObject>()
                        .FromSqlRaw(sql, AppSettings.DatabaseTable.Schema ?? "dbo", AppSettings.DatabaseTable.StoredProcedureCommand)
                        .AnyAsync();

                    if (!exists)
                    {
                        await DisplayAlert("Error", "Stored procedure not found. Please check the name and try again.", "OK");
                        return;
                    }
                }
            }
            
            await DisplayAlert("Success", "Database connection successful!", "OK");

            if (AppSettings != null)
                AppSettings.DBConnectionValid = true;
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"SQL error: {ex.Message}");
            await DisplayAlert("Error", $"A SQL error occurred. Please check your database settings.\r\nSQL error:\r\n{ex.Message}", "OK");
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Database update error: {ex.Message}");
            await DisplayAlert("Error", $"Failed to update the database. Please try again.\r\nDatabase update error:\r\n{ex.Message}", "OK");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
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

        RuntimeSettingsService.HasUnsavedChanges = false;
    }

    private async void OnCancelButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnCloseButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private void StoredProcedureNameChanged()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            if (AppSettings?.DatabaseTable?.StoredProcedureCommand?.Length > 0)
            {
                ExportSettingsGrid.RowDefinitions[9].Height = new GridLength(1, GridUnitType.Auto);
                ExportSettingsGrid.RowDefinitions[10].Height = new GridLength(1, GridUnitType.Auto);
                ExportSettingsGrid.RowDefinitions[11].Height = new GridLength(1, GridUnitType.Auto);
                ExportSettingsGrid.RowDefinitions[12].Height = new GridLength(1, GridUnitType.Auto);
            }
            else
            {
                ExportSettingsGrid.RowDefinitions[9].Height = 0;
                ExportSettingsGrid.RowDefinitions[10].Height = 0;
                ExportSettingsGrid.RowDefinitions[11].Height = 0;
                ExportSettingsGrid.RowDefinitions[12].Height = 0;
            }

            //Used to refresh the heights after changing them
            ExportSettingsGrid.InvalidateMeasure();
        });
    }

    private async Task<bool?> CheckInputs()
    {
        //Check fields have values first
        string? ErrorMessage = "";

        if (AppSettings != null)
        {
            if (string.IsNullOrEmpty(AppSettings?.DatabaseTable?.Database))
            {
                ErrorMessage += "Please enter a database name.\r\n";
            }
            if (string.IsNullOrEmpty(AppSettings?.DatabaseTable?.Schema))
            {
                ErrorMessage += "Please enter a schema name (enter dbo if unsure).\r\n";
            }
            if (string.IsNullOrEmpty(AppSettings?.DatabaseTable?.TableOrView) && string.IsNullOrEmpty(AppSettings?.DatabaseTable?.StoredProcedureCommand))
            {
                ErrorMessage += "Please select either a database table/view or a stored procedure.\r\n";
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
            ExportSettingsMessage.Text = "✅ Database settings are valid. Please specify details of the database object (table/view/procedure) to use";
            ExportSettingsMessageBox.BackgroundColor = Color.FromArgb("#198754");
        });
    }

    public void DatabaseConnectionInvalid()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            DisableControls();
            ExportSettingsMessage.Text = "⚠️ Please specify valid settings on the Database Settings screen first";
            ExportSettingsMessageBox.BackgroundColor = Color.FromArgb("#fd7e14");
        });
    }

    public void EnableControls()
    {
        Database.IsEnabled = true;
        Schema.IsEnabled = true;
        DatabaseObject.IsEnabled = true;
        StoredProcedureCommand.IsEnabled = true;
        SaveButton.IsEnabled = true;
    }

    public void DisableControls()
    {
        Database.IsEnabled = false;
        Schema.IsEnabled = false;
        DatabaseObject.IsEnabled = false;
        StoredProcedureCommand.IsEnabled = false;
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