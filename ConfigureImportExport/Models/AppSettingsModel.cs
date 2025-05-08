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
            IsLoading = false;
            DBConnectionValid = false;
            FileSettingsValid = false;
            FTPSettingsValid = false;
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
        [JsonIgnore]
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
        private bool _DBConnectionValid;
        [JsonIgnore]
        public bool DBConnectionValid
        {
            get => _DBConnectionValid;
            set
            {
                if (_DBConnectionValid != value)
                {
                    _DBConnectionValid = value;
                    TriggerPropertyChanged(nameof(DBConnectionValid));
                }
            }
        }

        [JsonIgnore]
        private bool _FileSettingsValid;
        [JsonIgnore]
        public bool FileSettingsValid
        {
            get => _FileSettingsValid;
            set
            {
                if (_FileSettingsValid != value)
                {
                    _FileSettingsValid = value;
                    TriggerPropertyChanged(nameof(FileSettingsValid));
                }
            }
        }

        [JsonIgnore]
        private bool _FTPSettingsValid;
        [JsonIgnore]
        public bool FTPSettingsValid
        {
            get => _FTPSettingsValid;
            set
            {
                if (_FTPSettingsValid != value)
                {
                    _FTPSettingsValid = value;
                    TriggerPropertyChanged(nameof(FTPSettingsValid));
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
