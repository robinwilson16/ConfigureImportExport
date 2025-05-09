﻿using Microsoft.EntityFrameworkCore;
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
    public class AppSettingsDatabaseConnectionModel : BaseModel
    {
        public AppSettingsDatabaseConnectionModel()
        {
            Init();
        }

        public override void Init()
        {
            base.Init();
            Server = string.Empty;
            Database = string.Empty;
            UseWindowsAuth = false;
            Username = string.Empty;
            Password = string.Empty;
        }

        private string? _Server;
        public string? Server
        {
            get => _Server;
            set
            {
                if (_Server != value)
                {
                    _Server = value;
                    TriggerPropertyChanged(nameof(Server));
                }
            }
        }

        private string? _Database;
        public string? Database
        {
            get => _Database;
            set
            {
                if (_Database != value)
                {
                    _Database = value;
                    TriggerPropertyChanged(nameof(Database));
                }
            }
        }

        private bool? _UseWindowsAuth = false;
        public bool? UseWindowsAuth
        {
            get => _UseWindowsAuth;
            set
            {
                if (_UseWindowsAuth != value)
                {
                    _UseWindowsAuth = value;
                    TriggerPropertyChanged(nameof(UseWindowsAuth));
                }
            }
        }

        private string? _Username;
        public string? Username
        {
            get => _Username;
            set
            {
                if (_Username != value)
                {
                    _Username = value;
                    TriggerPropertyChanged(nameof(Username));
                }
            }
        }

        private string? _Password;
        public string? Password
        {
            get => _Password;
            set
            {
                if (_Password != value)
                {
                    _Password = value;
                    TriggerPropertyChanged(nameof(Password));
                }
            }
        }
    }
}
