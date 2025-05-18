using ConfigureImportExport.Data;
using ConfigureImportExport.Models;
using ConfigureImportExport.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System.Diagnostics;
using System.Text.Json;
using WinSCP;

namespace ConfigureImportExport.Pages;

public partial class FTPSettings : ContentPage
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<FTPSettings> _logger;
    private readonly AppSettingsService _appSettingsService;

    public AppSettingsModel? AppSettings;

    public FTPSettings(ApplicationDbContext dbContext, ILogger<FTPSettings> logger, AppSettingsService appSettingsService)
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

        UploadCheckboxChanged();
    }

    private async void OnUploadCheckboxClicked(object sender, CheckedChangedEventArgs e)
    {
        UploadCheckboxChanged();
    }

    private async void OnTestButtonClicked(object sender, EventArgs e)
    {
        if (AppSettings != null)
            AppSettings.IsLoading = true;

        bool? IsError = false;
        string? TestFilePath = string.Empty;

        // Setup WinSCP Settings
        int PortNumber = Int32.TryParse(AppSettings?.FTPConnection?.Port, out int result) ? result : 21;
        SessionOptions sessionOptions = new SessionOptions
        {
            HostName = AppSettings?.FTPConnection?.Server,
            PortNumber = PortNumber,
            UserName = AppSettings?.FTPConnection?.Username,
            Password = AppSettings?.FTPConnection?.Password
        };

        switch (AppSettings?.FTPConnection?.Type)
        {
            case "FTP":
                sessionOptions.Protocol = Protocol.Ftp;
                break;
            case "FTPS":
                sessionOptions.Protocol = Protocol.Ftp;
                sessionOptions.FtpSecure = FtpSecure.Explicit;
                sessionOptions.GiveUpSecurityAndAcceptAnyTlsHostCertificate = true;
                break;
            case "SFTP":
                sessionOptions.Protocol = Protocol.Sftp;
                sessionOptions.GiveUpSecurityAndAcceptAnyTlsHostCertificate = true;
                break;
            case "SCP":
                sessionOptions.Protocol = Protocol.Scp;
                sessionOptions.GiveUpSecurityAndAcceptAnyTlsHostCertificate = true;
                break;
            default:
                sessionOptions.Protocol = Protocol.Ftp;
                break;
        }

        if (AppSettings?.FTPConnection?.SSHHostKeyFingerprint?.Length > 0)
        {
            sessionOptions.SshHostKeyFingerprint = AppSettings?.FTPConnection?.SSHHostKeyFingerprint;
            sessionOptions.GiveUpSecurityAndAcceptAnyTlsHostCertificate = false;
        }

        switch (AppSettings?.FTPConnection?.Mode)
        {
            case "Active":
                sessionOptions.FtpMode = FtpMode.Active;
                break;
            case "Passive":
                sessionOptions.FtpMode = FtpMode.Passive;
                break;
            default:
                sessionOptions.FtpMode = FtpMode.Passive;
                break;
        }

        string uploadPath = Path.Combine("/", AppSettings?.FTPConnection?.FolderPath ?? "");

        if (uploadPath.Substring(uploadPath.Length - 1) != "/")
        {
            uploadPath = uploadPath + "/";
        }

        try
        {
            TestFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FTPTestFile.txt");
            using FileStream outputStream = File.OpenWrite(TestFilePath);
            using StreamWriter streamWriter = new StreamWriter(outputStream);
            await streamWriter.WriteAsync("Test file for checking FTP access");
            await streamWriter.DisposeAsync();
            await outputStream.DisposeAsync();

            Trace.WriteLine("Saved test file to: " + TestFilePath);
            //Debug.WriteLine("Saved test file to: " + TestFilePath);
        }
        catch (Exception ex)
        {
            IsError = true;

            Console.WriteLine($"Unexpected error generating test file to upload to FTP: {ex.Message}");
            await DisplayAlert("Error", $"An unexpected error occurred generating test file to upload to FTP. Please ensure you are running this program from a folder you have full access to.\r\nUnexpected error:\r\n{ex.Message}", "OK");
        }

        bool? uploadSuccessful = false;
        bool? removalSuccessful = false;
        bool? deleteSuccessful = false;
        try
        {
            using (Session session = new Session())
            {
                //When publishing to a self-contained exe file need to specify the location of WinSCP.exe
                session.ExecutablePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WinSCP.exe");

                // Connect
                session.Open(sessionOptions);

                // Upload files
                TransferOptions transferOptions = new TransferOptions();
                transferOptions.TransferMode = TransferMode.Binary;

                TransferOperationResult transferResult;
                transferResult =
                    session.PutFiles(TestFilePath, uploadPath, false, transferOptions);

                // Throw on any error
                transferResult.Check();

                // Print results
                foreach (TransferEventArgs transfer in transferResult.Transfers)
                {
                    Console.WriteLine("Upload of {0} succeeded", transfer.FileName);
                }
            }

            Console.WriteLine($"File Uploaded to {sessionOptions.HostName} to {TestFilePath}");
            uploadSuccessful = true;

            //Now attempt to delete the test file from the FTP server
            using (Session session = new Session())
            {
                //When publishing to a self-contained exe file need to specify the location of WinSCP.exe
                session.ExecutablePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WinSCP.exe");
                // Connect
                session.Open(sessionOptions);
                // Delete files
                RemovalOperationResult transferResult;
                transferResult =
                    session.RemoveFiles(uploadPath + "FTPTestFile.txt");
                // Throw on any error
                transferResult.Check();
                // Print results
                foreach (RemovalEventArgs transfer in transferResult.Removals)
                {
                    Console.WriteLine("Delete of {0} succeeded", transfer.FileName);
                }
            }

            removalSuccessful = true;

            File.Delete(TestFilePath); // Delete the test file after checking
            deleteSuccessful = true;

            await DisplayAlert("Success", "FTP connection successful!", "OK");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            if (uploadSuccessful == true && removalSuccessful == false)
                await DisplayAlert("Success", $"FTP connection successful!.\r\nHowever the test file uploaded to the FTP could not be removed again\r\nYou may remove \"FTPTestFile.txt\" manually\r\nUnexpected error:\r\n{ex.Message}", "OK");
            else if (uploadSuccessful == true && removalSuccessful == true && deleteSuccessful == false)
                await DisplayAlert("Success", $"FTP connection successful!.\r\nHowever the test file uploaded to the FTP could not be deleted locally from the folder\r\nYou may remove \"FTPTestFile.txt\" manually\r\nUnexpected error:\r\n{ex.Message}", "OK");
            else
                await DisplayAlert("Error", $"An unexpected error occurred uploading the test file to FTP. Please check your FTP settings.\r\nUnexpected error:\r\n{ex.Message}", "OK");
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

    private void UploadCheckboxChanged()
    {
        if (AppSettings?.FTPConnection != null)
        {
            // If upload file set to false then disable controls otherwise enable them
            if (AppSettings.FTPConnection.UploadFile == true)
            {
                Server.IsEnabled = true;
                Type.IsEnabled = true;
                Port.IsEnabled = true;
                Mode.IsEnabled = true;
                Username.IsEnabled = true;
                Password.IsEnabled = true;
                SSHHostKeyFingerprint.IsEnabled = true;
                FolderPath.IsEnabled = true;
                TestButton.IsEnabled = true;
            }
            else
            {
                Server.IsEnabled = false;
                Type.IsEnabled = false;
                Port.IsEnabled = false;
                Mode.IsEnabled = false;
                Username.IsEnabled = false;
                Password.IsEnabled = false;
                SSHHostKeyFingerprint.IsEnabled = false;
                FolderPath.IsEnabled = false;
                TestButton.IsEnabled = false;
            }
        }
    }

    public void DatabaseConnectionValid()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            EnableControls();
            FTPSettingsMessage.Text = "✅ Database settings are valid. Please specify where to upload the file to below";
            FTPSettingsMessageBox.BackgroundColor = Color.FromArgb("#198754");
        });
    }

    public void DatabaseConnectionInvalid()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            DisableControls();
            FTPSettingsMessage.Text = "⚠️ Please specify valid settings on the Database Settings screen first";
            FTPSettingsMessageBox.BackgroundColor = Color.FromArgb("#fd7e14");
        });
    }

    public void EnableControls()
    {
        UploadFile.IsEnabled = true;
        SaveButton.IsEnabled = true;
    }

    public void DisableControls()
    {
        UploadFile.IsEnabled = false;
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