using ConfigureImportExport.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigureImportExport.Models
{
    public abstract class BaseModel : INotifyPropertyChanged
    {
        #region Constructor
        /// <summary>
        /// Base constructor for all models.
        /// </summary>
        protected BaseModel()
        {
            Init();
        }
        #endregion

        #region Init Method
        /// <summary>
        /// Initialise any properties of this class
        /// </summary>

        public virtual void Init()
        {
            
        }
        #endregion

        #region RaisePropertyChanged Method
        /// <summary>
        /// Event used to trigger changes to any bound UI objects
        /// </summary>  

        public event PropertyChangedEventHandler? PropertyChanged;

        public virtual void TriggerPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
