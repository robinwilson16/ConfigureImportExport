using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigureImportExport.Models
{
    public class AppSettingsDatabaseTableModel : BaseModel
    {
        private string? _Database;
        public string? Database
        {
            get => _Database;
            set
            {
                if (_Database != value)
                {
                    _Database = value;
                    TriggerPropertyChanged(nameof(Database));
                }
            }
        }

        private string? _Schema;
        public string? Schema
        {
            get => _Schema;
            set
            {
                if (_Schema != value)
                {
                    _Schema = value;
                    TriggerPropertyChanged(nameof(Schema));
                }
            }
        }

        private string? _TableOrView;
        public string? TableOrView
        {
            get => _TableOrView;
            set
            {
                if (_TableOrView != value)
                {
                    _TableOrView = value;
                    TriggerPropertyChanged(nameof(TableOrView));
                }
            }
        }

        private string? _StoredProcedureCommand;
        public string? StoredProcedureCommand
        {
            get => _StoredProcedureCommand;
            set
            {
                if (_StoredProcedureCommand != value)
                {
                    _StoredProcedureCommand = value;
                    TriggerPropertyChanged(nameof(StoredProcedureCommand));
                }
            }
        }

        private string? _StoredProcedureParam1IntegerName;
        public string? StoredProcedureParam1IntegerName
        {
            get => _StoredProcedureParam1IntegerName;
            set
            {
                if (_StoredProcedureParam1IntegerName != value)
                {
                    _StoredProcedureParam1IntegerName = value;
                    TriggerPropertyChanged(nameof(StoredProcedureParam1IntegerName));
                }
            }
        }

        private int? _StoredProcedureParam1IntegerValue;
        public int? StoredProcedureParam1IntegerValue
        {
            get => _StoredProcedureParam1IntegerValue;
            set
            {
                if (_StoredProcedureParam1IntegerValue != value)
                {
                    _StoredProcedureParam1IntegerValue = value;
                    TriggerPropertyChanged(nameof(StoredProcedureParam1IntegerValue));
                }
            }
        }

        private string? _StoredProcedureParam2IntegerName;
        public string? StoredProcedureParam2IntegerName
        {
            get => _StoredProcedureParam2IntegerName;
            set
            {
                if (_StoredProcedureParam2IntegerName != value)
                {
                    _StoredProcedureParam2IntegerName = value;
                    TriggerPropertyChanged(nameof(StoredProcedureParam2IntegerName));
                }
            }
        }

        private int? _StoredProcedureParam2IntegerValue;
        public int? StoredProcedureParam2IntegerValue
        {
            get => _StoredProcedureParam2IntegerValue;
            set
            {
                if (_StoredProcedureParam2IntegerValue != value)
                {
                    _StoredProcedureParam2IntegerValue = value;
                    TriggerPropertyChanged(nameof(StoredProcedureParam2IntegerValue));
                }
            }
        }

        private string? _StoredProcedureParam1StringName;
        public string? StoredProcedureParam1StringName
        {
            get => _StoredProcedureParam1StringName;
            set
            {
                if (_StoredProcedureParam1StringName != value)
                {
                    _StoredProcedureParam1StringName = value;
                    TriggerPropertyChanged(nameof(StoredProcedureParam1StringName));
                }
            }
        }

        private string? _StoredProcedureParam1StringValue;
        public string? StoredProcedureParam1StringValue
        {
            get => _StoredProcedureParam1StringValue;
            set
            {
                if (_StoredProcedureParam1StringValue != value)
                {
                    _StoredProcedureParam1StringValue = value;
                    TriggerPropertyChanged(nameof(StoredProcedureParam1StringValue));
                }
            }
        }

        private string? _StoredProcedureParam2StringName;
        public string? StoredProcedureParam2StringName
        {
            get => _StoredProcedureParam2StringName;
            set
            {
                if (_StoredProcedureParam2StringName != value)
                {
                    _StoredProcedureParam2StringName = value;
                    TriggerPropertyChanged(nameof(StoredProcedureParam2StringName));
                }
            }
        }

        private string? _StoredProcedureParam2StringValue;
        public string? StoredProcedureParam2StringValue
        {
            get => _StoredProcedureParam2StringValue;
            set
            {
                if (_StoredProcedureParam2StringValue != value)
                {
                    _StoredProcedureParam2StringValue = value;
                    TriggerPropertyChanged(nameof(StoredProcedureParam2StringValue));
                }
            }
        }
    }
}
