using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigureImportExport.Models
{
    public class AppSettingsDatabaseTableModel
    {
        public string? Database { get; set; }
        public string? Schema { get; set; }
        public string? TableOrView { get; set; }
        public string? StoredProcedureCommand { get; set; }
        public string? StoredProcedureParam1IntegerName { get; set; }
        public int? StoredProcedureParam1IntegerValue { get; set; }
        public string? StoredProcedureParam2IntegerName { get; set; }
        public int? StoredProcedureParam2IntegerValue { get; set; }
        public string? StoredProcedureParam1StringName { get; set; }
        public string? StoredProcedureParam1StringValue { get; set; }
        public string? StoredProcedureParam2StringName { get; set; }
        public string? StoredProcedureParam2StringValue { get; set; }
    }
}
