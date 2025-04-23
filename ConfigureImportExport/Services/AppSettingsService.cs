using ConfigureImportExport.Models;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DashboardApp.Services
{
    public class AppSettingsService
    {
        public AppSettings? _appSettings;
        public string? CustomSettingsFileName;

        public AppSettingsService()
        {
            CustomSettingsFileName = "appsettings.json";

            var options = new JsonSerializerOptions
            {
                ReadCommentHandling = JsonCommentHandling.Skip
            };

            string customSettingsFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CustomSettingsFileName);
            if (System.IO.File.Exists(customSettingsFile))
            {
                using FileStream inputStream = System.IO.File.OpenRead(customSettingsFile);
                using StreamReader streamReader = new StreamReader(inputStream);
                _appSettings = JsonSerializer.Deserialize<AppSettings?>(streamReader.ReadToEnd(), options);
                Trace.WriteLine("Loaded settings from: " + customSettingsFile);
                return;
            }
            else
            {
                _appSettings = new AppSettings();
            }
        }
        
        public AppSettings? Get()
        {
            return _appSettings;
        }

        public async Task<AppSettings>? Set()
        {
            
            //_appSettings.Version = "1.0.1";
            
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string customSettingsFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CustomSettingsFileName);
            using FileStream outputStream = System.IO.File.OpenWrite(customSettingsFile);
            using StreamWriter streamWriter = new StreamWriter(outputStream);
            await streamWriter.WriteAsync(JsonSerializer.Serialize(_appSettings, options));
            Trace.WriteLine("Saved settings file to: " + customSettingsFile);

            return _appSettings;
        }

        public async Task<AppSettings>? Set(AppSettings appSettings)
        {

            //_appSettings.Version = "1.0.1";

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string customSettingsFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CustomSettingsFileName);
            using FileStream outputStream = System.IO.File.OpenWrite(customSettingsFile);
            using StreamWriter streamWriter = new StreamWriter(outputStream);
            await streamWriter.WriteAsync(JsonSerializer.Serialize(appSettings, options));
            Trace.WriteLine("Saved settings file to: " + customSettingsFile);

            return appSettings;
        }
    }
}
