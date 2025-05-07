using ConfigureImportExport.Data;
using ConfigureImportExport.Models;
using ConfigureImportExport.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;

namespace ConfigureImportExport
{
    public partial class App : Application
    {
        public static IServiceProvider Services;
        public static ApplicationDbContext DBContext;
        public static IConfiguration Configuration;
        private readonly AppSettingsService AppSettingsService;
        public static DbContextFactory DBContextFactory;
        public static AppSettingsModel? AppSettingsCurrent;

        public App(ApplicationDbContext context, IServiceProvider provider, IConfiguration configuration, AppSettingsService appSettingsService, DbContextFactory dbContextFactory)
        {
            InitializeComponent();
            Services = provider;
            Configuration = configuration;
            AppSettingsService = appSettingsService;
            DBContextFactory = dbContextFactory;
            //Context = context;
            AppSettingsCurrent = AppSettingsService?.Get();
            var conStrBuilder = new SqlConnectionStringBuilder
            {
                DataSource = AppSettingsCurrent?.DatabaseConnection?.Server,
                InitialCatalog = AppSettingsCurrent?.DatabaseConnection?.Database,
                UserID = AppSettingsCurrent?.DatabaseConnection?.Username,
                Password = AppSettingsCurrent?.DatabaseConnection?.Password,
                IntegratedSecurity = AppSettingsCurrent?.DatabaseConnection?.UseWindowsAuth ?? false,
                TrustServerCertificate = true,
                Encrypt = true,
                ConnectTimeout = 30,
                MultipleActiveResultSets = true,
                ApplicationName = "ConfigureImportExport"
            };
            var connectionString = conStrBuilder.ConnectionString;
            DBContext = DBContextFactory.CreateDbContext(connectionString);
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            //return new Window(new AppShell());
            Window window = new Window(new AppShell());
            window.Title = "Configuration Utility for File Import and Export Tools";

            return window;
        }

        public void ChangeConnectionString(string newConnectionString)
        {
            // Dispose of the old DbContext if necessary
            DBContext?.Dispose();

            // Create a new DbContext with the updated connection string
            DBContext = DBContextFactory.CreateDbContext(newConnectionString);

            Console.WriteLine("Connection string updated successfully.");
        }
    }
}