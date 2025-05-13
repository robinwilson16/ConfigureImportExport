using System.Diagnostics;

namespace ConfigureImportExport.Pages;

public partial class RunExportTask : ContentPage
{
	public RunExportTask()
	{
		InitializeComponent();
	}

    private async void RunButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Show the loading overlay
            LoadingOverlay.IsVisible = true;

            //string exePath = Path.Combine(AppContext.BaseDirectory, "CSVSQLExporter.exe");
            string exePath = Path.Combine(Path.GetDirectoryName(Environment.ProcessPath), "CSVSQLExporter.exe");

            // Ensure the file exists
            if (!File.Exists(exePath))
            {
                await DisplayAlert("Error", "CSVSQLExporter.exe not found. Please ensure it is in the application directory.", "OK");
                return;
            }

            // Set up the process start information
            var processStartInfo = new ProcessStartInfo
            {
                FileName = exePath,
                Arguments = "", // Add any arguments here if needed
                UseShellExecute = false, // Set to true if you want to open it in a shell
                RedirectStandardOutput = true, // Redirect output if you want to capture it
                RedirectStandardError = true, // Redirect error output
                CreateNoWindow = true // Do not create a visible window
            };

            // Start the process
            using var process = new Process { StartInfo = processStartInfo };
            process.Start();

            // Optionally, read the output
            string output = await process.StandardOutput.ReadToEndAsync();
            string error = await process.StandardError.ReadToEndAsync();

            // Wait for the process to exit
            process.WaitForExit();

            // Check the exit code
            if (process.ExitCode == 0)
            {
                await DisplayAlert("Success", "CSVSQLExporter executed successfully.", "OK");
            }
            else
            {
                await DisplayAlert("Error", $"CSVSQLExporter failed with exit code {process.ExitCode}.\nError: {error}", "OK");
            }
        }
        catch (Exception ex)
        {
            // Handle any exceptions
            await DisplayAlert("Error", $"An error occurred while executing CSVSQLExporter.exe:\n{ex.Message}", "OK");
        }
        finally
        {
            // Hide the loading overlay
            LoadingOverlay.IsVisible = false;
        }
    }
}