using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDesign.ViewModels.Dialogs
{
    internal class LoginViewModel : BindableBase, IDialogAware
    {
        #region Dialog部分
        public string Title { get; set; } = "登录";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }
        #endregion

        private string account;
        private string password;

        public DelegateCommand<string> ExecCommand;

        public LoginViewModel()
        {
            ExecCommand = new DelegateCommand<string>(Exec);
        }

        void Exec(string cmd)
        {
            switch (cmd)
            {
                case "登录": Login(); break;
                case "退出": Logout(); break;
            }
        }

        void Login() { }
        void Logout() { }

        public string Account { get { return account; } set { account = value; RaisePropertyChanged(); } }
        public string Password { get { return password; } set { password = value; RaisePropertyChanged(); } }
    }
}
