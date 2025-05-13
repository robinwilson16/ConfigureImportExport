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

namespace ConfigureImportExport.Services
{
    public class AppSettingsService
    {
        public AppSettingsModel? _appSettings;
        public string? CustomSettingsFileName;
        public string? CustomSettingsFilePath;

        public AppSettingsService()
        {
            CustomSettingsFileName = "appsettings.json";

            var options = new JsonSerializerOptions
            {
                ReadCommentHandling = JsonCommentHandling.Skip
            };

            CustomSettingsFilePath = Path.Combine(Path.GetDirectoryName(Environment.ProcessPath), CustomSettingsFileName);
            if (File.Exists(CustomSettingsFilePath))
            {
                using FileStream inputStream = System.IO.File.OpenRead(CustomSettingsFilePath);
                using StreamReader streamReader = new StreamReader(inputStream);
                _appSettings = JsonSerializer.Deserialize<AppSettingsModel?>(streamReader.ReadToEnd(), options);
                Trace.WriteLine("Loaded settings from: " + CustomSettingsFilePath);
                return;
            }
            else
            {
                _appSettings = new AppSettingsModel();
            }
        }
        
        public AppSettingsModel? Get()
        {
            return _appSettings;
        }

        public async Task<AppSettingsModel?> Set()
        {
            
            //_appSettings.Version = "1.0.1";
            
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            CustomSettingsFilePath = System.IO.Path.Combine(Path.GetDirectoryName(Environment.ProcessPath), CustomSettingsFileName);
            using FileStream outputStream = System.IO.File.OpenWrite(CustomSettingsFilePath);
            using StreamWriter streamWriter = new StreamWriter(outputStream);
            await streamWriter.WriteAsync(JsonSerializer.Serialize(_appSettings, options));

            //Set variable back to false now settings have been saved
            RuntimeSettingsService.HasUnsavedChanges = false;
            Trace.WriteLine("Saved settings file to: " + CustomSettingsFilePath);

            return _appSettings;
        }

        public async Task<AppSettingsModel>? Set(AppSettingsModel appSettings)
        {

            //_appSettings.Version = "1.0.1";

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            CustomSettingsFilePath = System.IO.Path.Combine(Path.GetDirectoryName(Environment.ProcessPath), CustomSettingsFileName);
            using FileStream outputStream = System.IO.File.OpenWrite(CustomSettingsFilePath);
            using StreamWriter streamWriter = new StreamWriter(outputStream);
            await streamWriter.WriteAsync(JsonSerializer.Serialize(appSettings, options));
            Trace.WriteLine("Saved settings file to: " + CustomSettingsFilePath);
            //Debug.WriteLine("Saved settings file to: " + CustomSettingsFilePath);

            return appSettings;
        }

        public string? GetFilePath()
        {
            return CustomSettingsFilePath;
        }
    }
}
