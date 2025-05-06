using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Microsoft.Maui.Controls;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ConfigureImportExport.Models
{
    [NotMapped]
    [Keyless]
    public class AppSettingsModel: BaseModel
    {
        public AppSettingsModel()
        {
            Init();
        }
        public override void Init()
        {
            base.Init();
            Version = string.Empty;
            SettingsDate = null;
            Logging = new AppSettingsLoggingModel();
            Locale = "en-GB";
            DatabaseConnection = new AppSettingsDatabaseConnectionModel();
            DatabaseTable = new AppSettingsDatabaseTableModel();
            CSVFile = new AppSettingsCSVFileModel();
            FTPConnection = new AppSettingsFTPConnectionModel();
        }

        public string? Version { get; set; }
        public DateTime? SettingsDate { get; set; }

        public virtual AppSettingsLoggingModel? Logging { get; set; }

        public string? Locale { get; set; }
        public virtual AppSettingsDatabaseConnectionModel? DatabaseConnection { get; set; }
        public virtual AppSettingsDatabaseTableModel? DatabaseTable { get; set; }
        public virtual AppSettingsCSVFileModel? CSVFile { get; set; }
        public virtual AppSettingsFTPConnectionModel? FTPConnection { get; set; }

        [JsonIgnore]
        private bool _IsLoading;
        public bool IsLoading
        {
            get => _IsLoading;
            set
            {
                if (_IsLoading != value)
                {
                    _IsLoading = value;
                    TriggerPropertyChanged(nameof(IsLoading));
                }
            }
        }

        [JsonIgnore]
        public string? VersionAndDate
        {
            get
            {
                if (SettingsDate != null)
                {
                    return $"{Version} - {SettingsDate.Value.ToShortDateString()}";
                }
                else
                {
                    return Version;
                }
            }
        }
    }
}
