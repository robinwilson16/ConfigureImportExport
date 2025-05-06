using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ConfigureImportExport.Models
{
    [NotMapped]
    [Keyless]
    public class AppSettingsLoggingLogLevelModel : BaseModel
    {
        public AppSettingsLoggingLogLevelModel()
        {
            Init();
        }
        public override void Init()
        {
            base.Init();
            Default = "Information";
            MicrosoftAspNetCore = "Warning";
        }

        public string? Default { get; set; }

        [JsonPropertyName("Microsoft.AspNetCore")]
        public string? MicrosoftAspNetCore { get; set; }
    }
}
