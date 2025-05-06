using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigureImportExport.Services
{
    public static class RuntimeSettingsService
    {
        public static bool? HasUnsavedChanges { get; set; } = false;
    }
}
