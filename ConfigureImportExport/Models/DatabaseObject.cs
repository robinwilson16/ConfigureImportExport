using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigureImportExport.Models
{
    [Keyless]
    public class DatabaseObject
    {
        public int? Result { get; set; }
    }
}
