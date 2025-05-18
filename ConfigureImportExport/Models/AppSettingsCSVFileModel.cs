using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ConfigureImportExport.Models
{
    [NotMapped]
    [Keyless]
    public class AppSettingsCSVFileModel : BaseModel
    {
        public AppSettingsCSVFileModel()
        {
            Init();
        }
        public override void Init()
        {
            base.Init();
            Folder = string.Empty;
            FileName = string.Empty;
            ColumnNameAsFileName = string.Empty;
            IncludeHeaders = true;
            Delimiter = ",";
            AlwaysWrapInSpeechmarks = false;
        }

        private string? _Folder;
        public string? Folder
        {
            get => _Folder;
            set
            {
                if (_Folder != value)
                {
                    _Folder = value;
                    TriggerPropertyChanged(nameof(Folder));
                }
            }
        }

        [JsonIgnore]
        public string FolderDisplay
        {
            get => Folder?.Replace("\\\\", "\\") ?? string.Empty;
            set
            {
                // Store as double backslashes in the model
                var newValue = value?.Replace("\\", "\\\\");
                if (Folder != newValue)
                {
                    Folder = newValue;
                    TriggerPropertyChanged(nameof(FolderDisplay));
                }
            }
        }

        private string? _FileName;
        public string? FileName
        {
            get => _FileName;
            set
            {
                if (_FileName != value)
                {
                    _FileName = value;
                    TriggerPropertyChanged(nameof(FileName));
                }
            }
        }

        private string? _ColumnNameAsFileName;
        public string? ColumnNameAsFileName
        {
            get => _ColumnNameAsFileName;
            set
            {
                if (_ColumnNameAsFileName != value)
                {
                    _ColumnNameAsFileName = value;
                    TriggerPropertyChanged(nameof(ColumnNameAsFileName));
                }
            }
        }

        private bool? _IncludeHeaders;
        public bool? IncludeHeaders
        {
            get => _IncludeHeaders;
            set
            {
                if (_IncludeHeaders != value)
                {
                    _IncludeHeaders = value;
                    TriggerPropertyChanged(nameof(IncludeHeaders));
                }
            }
        }

        private string? _Delimiter;
        public string? Delimiter
        {
            get => _Delimiter;
            set
            {
                if (_Delimiter != value)
                {
                    _Delimiter = value;
                    TriggerPropertyChanged(nameof(Delimiter));
                }
            }
        }

        private bool? _AlwaysWrapInSpeechmarks;
        public bool? AlwaysWrapInSpeechmarks
        {
            get => _AlwaysWrapInSpeechmarks;
            set
            {
                if (_AlwaysWrapInSpeechmarks != value)
                {
                    _AlwaysWrapInSpeechmarks = value;
                    TriggerPropertyChanged(nameof(AlwaysWrapInSpeechmarks));
                }
            }
        }
    }
}
