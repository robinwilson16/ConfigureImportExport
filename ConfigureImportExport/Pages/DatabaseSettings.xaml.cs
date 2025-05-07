using ConfigureImportExport.Data;
using ConfigureImportExport.Models;
using ConfigureImportExport.Services;
using Microsoft.Maui.Controls;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System.Diagnostics;

namespace ConfigureImportExport.Pages;

public partial class DatabaseSettings : ContentPage
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<DatabaseSettings> _logger;
    private readonly AppSettingsService _appSettingsService;

    public AppSettingsModel? AppSettings;

    public DatabaseSettings(ApplicationDbContext dbContext, ILogger<DatabaseSettings> logger, AppSettingsService appSettingsService)
    {
        InitializeComponent();
        _dbContext = dbContext;
        _logger = logger;
        _appSettingsService = appSettingsService;

        AppSettings = _appSettingsService?.Get();

        BindingContext = AppSettings;

        if (AppSettings != null)
        {
            AppSettings.IsLoading = false;
            AppSettings.DBConnectionValid = false;
        }

        RuntimeSettingsService.HasUnsavedChanges = false;
        DisableControls();

        UseWindowsAuthChanged();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        //Code here
    }

    private void OnUseWindowsAuthCheckboxClicked(object sender, CheckedChangedEventArgs e)
    {
        UseWindowsAuthChanged();
    }

    private async void OnTestButtonClicked(object sender, EventArgs e)
    {
        if (AppSettings != null)
            AppSettings.IsLoading = true;

        var conStrBuilder = new SqlConnectionStringBuilder
        {
            DataSource = AppSettings?.DatabaseConnection?.Server,
            InitialCatalog = AppSettings?.DatabaseConnection?.Database,
            UserID = AppSettings?.DatabaseConnection?.Username,
            Password = AppSettings?.DatabaseConnection?.Password,
            IntegratedSecurity = AppSettings?.DatabaseConnection?.UseWindowsAuth ?? false,
            TrustServerCertificate = true,
            Encrypt = true,
            ConnectTimeout = 10,
            MultipleActiveResultSets = true,
            ApplicationName = "ConfigureImportExport"
        };
        var connectionString = conStrBuilder.ConnectionString;
        App.DBContext.Dispose();
        App.DBContext = App.DBContextFactory.CreateDbContext(connectionString); // Create a new DbContext with the updated connection string

        try
        {
            await App.DBContext.Database.ExecuteSqlRawAsync("SELECT 1");
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

        if (AppSettings?.DBConnectionValid == true)
        {
            await LoadDatabaseObjects();
            await LoadStoredProcedures();
            DatabaseConnectionValid();
        }
        else
        {
            DatabaseConnectionInvalid();
        }
        //try
        //{
        //    if (await App.DBContext.Database.CanConnectAsync())
        //    {
        //        await DisplayAlert("Success", "Database connection successful!", "OK");
        //    }
        //    else
        //    {
        //        await DisplayAlert("Error", "Database connection failed. Please check your settings.", "OK");
        //    }
        //}
        //catch (SqlException ex)
        //{
        //    // Handle SQL-specific errors
        //    Console.WriteLine($"SQL error: {ex.Message}");
        //    await DisplayAlert("Error", $"Database connection failed. SQL error: {ex.Message}", "OK");
        //}
        //catch (Exception ex)
        //{
        //    // Handle general errors
        //    Console.WriteLine($"Unexpected error: {ex.Message}");
        //    await DisplayAlert("Error", $"Database connection failed. Unexpected error: {ex.Message}", "OK");
        //}

        //if (await _dbContext.Database.CanConnectAsync())
        //{
        //    await DisplayAlert("Success", "Database connection successful!", "OK");
        //}
        //else
        //{
        //    await DisplayAlert("Error", "Database connection failed. Please check your settings.", "OK");
        //}
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

    private async void OnCloseButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private void UseWindowsAuthChanged()
    {
        if (AppSettings?.DatabaseConnection != null)
        {
            // If upload file set to false then disable controls otherwise enable them
            if (AppSettings.DatabaseConnection.UseWindowsAuth == true)
            {
                Username.IsEnabled = false;
                Password.IsEnabled = false;
            }
            else
            {
                Username.IsEnabled = true;
                Password.IsEnabled = true;
            }
        }
    }

    private async Task LoadDatabaseObjects()
    {
        try
        {
            if (AppSettings?.DatabaseTable != null)
            {
                AppSettings?.DatabaseTable?.DatabaseObject.Clear();

                var allObjects = await App.DBContext.Database
                    .SqlQueryRaw<string>("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE IN ('BASE TABLE', 'VIEW') ORDER BY TABLE_NAME")
                    .ToListAsync();

                if (allObjects != null)
                {
                    AppSettings?.DatabaseTable?.DatabaseObject.Add("");

                    foreach (var databaseObject in allObjects)
                    {
                        AppSettings?.DatabaseTable?.DatabaseObject.Add(databaseObject);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }

    private async Task LoadStoredProcedures()
    {
        try
        {
            if (AppSettings?.DatabaseTable != null)
            {
                AppSettings?.DatabaseTable?.DatabaseStoredProcedure.Clear();

                var allProcedures = await App.DBContext.Database
                    .SqlQueryRaw<string>("SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_TYPE = 'PROCEDURE' ORDER BY ROUTINE_NAME")
                    .ToListAsync();

                if (allProcedures != null)
                {
                    AppSettings?.DatabaseTable?.DatabaseStoredProcedure.Add("");

                    foreach (var procedure in allProcedures)
                    {
                        AppSettings?.DatabaseTable?.DatabaseStoredProcedure.Add(procedure);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }

    public void DatabaseConnectionValid()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            EnableControls();
            DatabaseSettingsMessage.Text = "✅ Database settings are valid. Please move to the Export Settings screen.";
            DatabaseSettingsMessageBox.BackgroundColor = Color.FromArgb("#198754");
        });
    }

    public void DatabaseConnectionInvalid()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            DisableControls();
            DatabaseSettingsMessage.Text = "⚠️ Database settings are not valid. Please amend and test again.";
            DatabaseSettingsMessageBox.BackgroundColor = Color.FromArgb("#fd7e14");
        });
    }

    public void EnableControls()
    {
        SaveButton.IsEnabled = true;
    }

    public void DisableControls()
    {
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