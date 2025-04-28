using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigureImportExport.Models
{
    public class AppSettingsLoggingModel
    {
        public virtual AppSettingsLoggingLogLevelModel? LogLevel { get; set; } = new AppSettingsLoggingLogLevelModel();
    }
}
