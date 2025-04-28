using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigureImportExport.Models
{
    public class AppSettingsExcelFileModel
    {
        public string? Folder { get; set; }
        public string? FileName { get; set; }
        public string? SheetName { get; set; }
        public string? ColumnNameAsFileName { get; set; }
        public string? DateFormat { get; set; }
        public string? TimeFormat { get; set; }
        public string? CurrencyFormat { get; set; }
    }
}
