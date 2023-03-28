using Administrador.ViewModels;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Administrador.ViewModels
{
    class AdministradorViewModels
    {
        #region Attributes
        private string userName;

        #endregion
        
        #region Properties
        public string UserName
        {
            get { return this.userName; }
        }
        #endregion
        
        #region Commands
        public ICommand LogOutCommand
        {
            get
            {
                return new RelayCommand(LogOut);
            }
        }
        #endregion
        
        #region Methods

        public void LogOut()
        {
            UserProvider obj = new UserProvider();
            obj.LogOut();
        }
        #endregion
    }
}
