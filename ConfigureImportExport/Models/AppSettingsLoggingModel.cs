using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigureImportExport.Models
{
    [NotMapped]
    [Keyless]
    public class AppSettingsLoggingModel
    {
        public virtual AppSettingsLoggingLogLevelModel? LogLevel { get; set; } = new AppSettingsLoggingLogLevelModel();
    }
}
