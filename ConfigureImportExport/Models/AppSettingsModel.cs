using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ConfigureImportExport.Models
{
    public class AppSettingsModel: BaseModel
    {
        public string? Version { get; set; }
        public DateTime? SettingsDate { get; set; }

        public virtual AppSettingsLoggingModel? Logging { get; set; } = new AppSettingsLoggingModel();

        public string? Locale { get; set; }
        public virtual AppSettingsDatabaseConnectionModel? DatabaseConnection { get; set; } = new AppSettingsDatabaseConnectionModel();
        public virtual AppSettingsDatabaseTableModel? DatabaseTable { get; set; } = new AppSettingsDatabaseTableModel();
        public virtual AppSettingsExcelFileModel? ExcelFile { get; set; } = new AppSettingsExcelFileModel();
        public virtual AppSettingsFTPConnectionModel? FTPConnection { get; set; } = new AppSettingsFTPConnectionModel();
        public virtual AppSettingsStoredProcedureModel? StoredProcedure { get; set; } = new AppSettingsStoredProcedureModel();

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
