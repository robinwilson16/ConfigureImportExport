namespace ConfigureImportExport.Pages;

public partial class Welcome : ContentPage
{
	public Welcome()
	{
		InitializeComponent();
	}

    private async void DatabaseSettingsButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(Pages.DatabaseSettings));
    }

    private async void ExportSettingsButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(Pages.ExportSettings));
    }

    private async void FileSettingsButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(Pages.FileSettings));
    }

    private async void FTPSettingsButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(Pages.FTPSettings));
    }
}