using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigureImportExport.Models
{
    public class AppSettingsStoredProcedureModel
    {
        public bool? RunTask { get; set; }
        public string? Database { get; set; }
        public string? Schema { get; set; }
        public string? StoredProcedure { get; set; }
    }
}
