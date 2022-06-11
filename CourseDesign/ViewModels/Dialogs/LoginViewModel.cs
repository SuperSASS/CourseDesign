using CourseDesign.Services.API.Interfaces;
using CourseDesign.Shared;
using CourseDesign.Shared.DTOs;
using CourseDesign.Shared.Extensions;
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

        /// <summary>
        /// 点击关闭按钮（右上角）时的行为
        /// </summary>
        /// <param name="parameters"></param>
        public void OnDialogOpened(IDialogParameters parameters)
        {
        }

        public void OnDialogClosed()
        {
            Logout();
        }

        #endregion

        // API服务
        private readonly ILoginService LoginService;
        // 属性内部字段
        private string account;
        private string password;

        public DelegateCommand<string> ExecCommand;

        public LoginViewModel(ILoginService loginService)
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

        /// <summary>
        /// 登录方法
        /// </summary>
        /// <exception cref="Exception"></exception>
        async void Login()
        {
            if (string.IsNullOrWhiteSpace(Account) || string.IsNullOrWhiteSpace(Password))
                throw new Exception("账户和密码没有输入完整！……"); // 返回错误提示
            APIStatusCode loginResponse = (await LoginService.Login(new UserDTO()
            {
                Account = Account,
                Password = Password.GetMD5() // 要传递MD5hash后的密码过去
            })).Status;
            if (loginResponse == APIStatusCode.Success)
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK)); // 返回OK状态给callback（可见App.xaml.cs）里所用

            // 提示登陆失败的信息
        }
        /// <summary>
        /// 退出方法
        /// </summary>
        void Logout()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Abort)); // 返回OK状态给callback（可见App.xaml.cs）里所用
        }

        public string Account { get { return account; } set { account = value; RaisePropertyChanged(); } }
        public string Password { get { return password; } set { password = value; RaisePropertyChanged(); } }
    }
}
