using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigureImportExport.Models;
using ConfigureImportExport.Services;
using Microsoft.EntityFrameworkCore;

namespace ConfigureImportExport.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<AppSettingsModel> AppSettings { get; set; }
    }
}
