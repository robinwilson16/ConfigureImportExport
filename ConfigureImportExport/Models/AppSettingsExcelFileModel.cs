using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigureImportExport.Models
{
    public class AppSettingsExcelFileModel : BaseModel
    {
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

        private string? _SheetName;
        public string? SheetName
        {
            get => _SheetName;
            set
            {
                if (_SheetName != value)
                {
                    _SheetName = value;
                    TriggerPropertyChanged(nameof(SheetName));
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

        private string? _DateFormat;
        public string? DateFormat
        {
            get => _DateFormat;
            set
            {
                if (_DateFormat != value)
                {
                    _DateFormat = value;
                    TriggerPropertyChanged(nameof(DateFormat));
                }
            }
        }

        private string? _TimeFormat;
        public string? TimeFormat
        {
            get => _TimeFormat;
            set
            {
                if (_TimeFormat != value)
                {
                    _TimeFormat = value;
                    TriggerPropertyChanged(nameof(TimeFormat));
                }
            }
        }

        private string? _CurrencyFormat;
        public string? CurrencyFormat
        {
            get => _CurrencyFormat;
            set
            {
                if (_CurrencyFormat != value)
                {
                    _CurrencyFormat = value;
                    TriggerPropertyChanged(nameof(CurrencyFormat));
                }
            }
        }
    }
}
