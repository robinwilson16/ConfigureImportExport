using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigureImportExport.Models
{
    public class AppSettingsFTPConnectionModel
    {
        public bool? UploadFile { get; set; }
        public string? Server { get; set; }
        public string? Type { get; set; }
        public string? Port { get; set; }
        public string? Mode { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? SSHHostKeyFingerprint { get; set; }
        public string? FolderPath { get; set; }
    }
}
