using ConfigureImportExport.Models;
using DashboardApp.Services;

namespace ConfigureImportExport
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            //Code here
        }

        private async void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            //Test function to save a config value
            AppSettingsService appSettingsService = new AppSettingsService();
            AppSettingsModel? appSettings = await appSettingsService.Set();

            SemanticScreenReader.Announce(CounterBtn.Text);

            System.Diagnostics.Debug.WriteLine("Clicked " + count + " times");
        }
    }

}
