using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace ConfigureImportExport.Models
{
    public class AppSettingsFTPConnectionModel : BaseModel
    {
        public AppSettingsFTPConnectionModel()
        {
            Init();
        }
        public override void Init()
        {
            base.Init();
            UploadFile = false;
            Server = string.Empty;
            Type = "SFTP";
            Port = "22";
            Mode = "Passive";
            Username = string.Empty;
            Password = string.Empty;
            SSHHostKeyFingerprint = string.Empty;
            FolderPath = string.Empty;
        }

        private bool? _UploadFile;
        public bool? UploadFile
        {
            get => _UploadFile;
            set
            {
                if (_UploadFile != value)
                {
                    _UploadFile = value;
                    TriggerPropertyChanged(nameof(UploadFile));
                }
            }
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

        private string? _Type;
        public string? Type
        {
            get => _Type;
            set
            {
                if (_Type != value)
                {
                    _Type = value;
                    TriggerPropertyChanged(nameof(Type));
                }
            }
        }

        private string? _Port;
        public string? Port
        {
            get => _Port;
            set
            {
                if (_Port != value)
                {
                    _Port = value;
                    TriggerPropertyChanged(nameof(Port));
                }
            }
        }

        private string? _Mode;
        public string? Mode
        {
            get => _Mode;
            set
            {
                if (_Mode != value)
                {
                    _Mode = value;
                    TriggerPropertyChanged(nameof(Mode));
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

        private string? _SSHHostKeyFingerprint;
        public string? SSHHostKeyFingerprint
        {
            get => _SSHHostKeyFingerprint;
            set
            {
                if (_SSHHostKeyFingerprint != value)
                {
                    _SSHHostKeyFingerprint = value;
                    TriggerPropertyChanged(nameof(SSHHostKeyFingerprint));
                }
            }
        }

        private string? _FolderPath;
        public string? FolderPath
        {
            get => _FolderPath;
            set
            {
                if (_FolderPath != value)
                {
                    _FolderPath = value;
                    TriggerPropertyChanged(nameof(FolderPath));
                }
            }
        }

        public ObservableCollection<FTPConnectionTypeOption> FTPConnectionType { get; set; } = new ObservableCollection<FTPConnectionTypeOption>
        {
            new FTPConnectionTypeOption { DisplayName = "File Transfer Protocol (FTP)", Value = "FTP" },
            new FTPConnectionTypeOption { DisplayName = "FTP Secure (FTPS)", Value = "FTPS" },
            new FTPConnectionTypeOption { DisplayName = "SSH File Transfer Protocol (SFTP)", Value = "SFTP" },
            new FTPConnectionTypeOption { DisplayName = "Secure Copy Protocol (SCP)", Value = "SCP" }
        };

        public ObservableCollection<string> FTPConnectionMode { get; set; } = new ObservableCollection<string>
        {
            "Passive",
            "Port"
        };
    }
}
