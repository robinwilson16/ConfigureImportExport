using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ConfigureImportExport.Models
{
    public class AppSettings: INotifyPropertyChanged
    {
        public string? Version { get; set; }
        public DateTime? SettingsDate { get; set; }
        private string? _serverName;

        public string? ServerName
        {
            get => _serverName;
            set
            {
                if (_serverName != value)
                {
                    _serverName = value;
                    OnPropertyChanged(nameof(ServerName));
                }
            }
        }
        public string? Username { get; set; }
        public string? Password { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
