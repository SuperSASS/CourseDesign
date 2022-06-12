using CourseDesign.Services.API.Interfaces;
using CourseDesign.Shared;
using CourseDesign.Shared.DTOs;
using CourseDesign.Shared.Extensions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using static CourseDesign.Context.LoginUserContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseDesign.Common.Classes;
using Prism.Events;
using CourseDesign.Extensions;
using CourseDesign.ViewModels.Bases;
using Prism.Ioc;
using Prism.Regions;

namespace CourseDesign.ViewModels.Dialogs
{
    internal class LoginViewModel : DialogNavigationViewModel, IDialogAware
    {
        #region Dialog部分
        public string Title { get; set; } = "登录";

        public event Action<IDialogResult> RequestClose;

        /// <summary>
        /// 当前对话可以关闭的哦（然后就退出了！……
        /// </summary>
        /// <returns></returns>
        public bool CanCloseDialog() { return true; }

        public void OnDialogOpened(IDialogParameters parameters) { }

        /// <summary>
        /// 点击关闭按钮（右上角）时的行为
        /// </summary>
        /// <param name="parameters"></param>
        public void OnDialogClosed() { ExitAPP(); }
        #endregion

        #region 字段
        // API服务
        private readonly ILoginService LoginService;
        // 属性内部字段
        private string account;
        private string userName;
        private string password;
        private string repeatPassword;
        private int selectIndex; // 登陆界面所选择的页面（0为登录，1为注册）
        #endregion

        #region 属性

        /// <summary>
        /// 输入的账号
        /// </summary>
        public string Account { get { return account; } set { account = value; RaisePropertyChanged(); } }
        /// <summary>
        /// 输入的用户名（注册用）
        /// </summary>
        public string UserName { get { return userName; } set { userName = value; RaisePropertyChanged(); } }
        /// <summary>
        /// 输入的密码
        /// </summary>
        public string Password { get { return password; } set { password = value; RaisePropertyChanged(); } }
        /// <summary>
        /// 重复输入的密码
        /// </summary>
        public string RepeatPassword { get { return repeatPassword; } set { repeatPassword = value; RaisePropertyChanged(); } }
        /// <summary>
        /// 登陆界面所选择的页面（0为登录，1为注册）
        /// </summary>
        public int SelectIndex { get { return selectIndex; } set { selectIndex = value; RaisePropertyChanged(); } }

        /// <summary>
        /// 页面总命令
        /// </summary>
        public DelegateCommand<string> ExecCommand { get; private set; }
        #endregion

        public LoginViewModel(IContainerProvider containerProvider) : base(containerProvider)
        {
            LoginService = containerProvider.Resolve<ILoginService>();
            ExecCommand = new DelegateCommand<string>(Exec);
        }

        void Exec(string cmd)
        {
            switch (cmd)
            {
                case "登录": Login(); break;
                case "注册": Register(); break;
                case "转到注册页面": SelectIndex = 1; break;
                case "返回登陆页面": SelectIndex = 0; break;
            }
        }

        /// <summary>
        /// 登录用的方法。成功登录后callback会返回一个参数parameter，里面有Key="UserID"的值，代表用户ID
        /// </summary>
        private async void Login()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Account) || string.IsNullOrWhiteSpace(Password))
                    throw new Exception("账户和密码没有输入完整！……"); // 返回错误提示
                ShowLoadingDialog(true);
                APIResponse<UserDTO> loginResponse = await LoginService.Login(new UserDTO()
                {
                    Account = Account,
                    Password = Password.GetMD5() // 要传递MD5hash后的密码过去
                });
                if (loginResponse.Status != APIStatusCode.Success)
                    throw new Exception(loginResponse.Message);
                DialogParameters parametere = new DialogParameters();
                UserDTO userDTO = loginResponse.Result;
                UserClass user = new UserClass(userDTO.ID, userDTO.UserName);
                parametere.Add("User", user);
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK, parametere)); // 返回OK状态以及用户数据给callback（可见App.xaml.cs）里所用
            }
            catch (Exception ex)
            {
                ShowMessageDialog(ex.Message, "Login");
            }
            finally
            {
                ShowLoadingDialog(false);
            }
        }

        /// <summary>
        /// 退出整个APP的方法，在登陆界面点击右上角关闭时调用（否则不登录关闭后仍会加载主页面）
        /// </summary>
        private void ExitAPP()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Abort)); // 返回OK状态给callback（可见App.xaml.cs）里所用
        }
        /// <summary>
        /// 注册方法
        /// </summary>
        private async void Register()
        {
            try
            {
                ShowLoadingDialog(true);
                if (string.IsNullOrWhiteSpace(Account) || string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password))
                    throw new Exception("账户、用户名和密码还没有输入完整！……"); // 返回错误提示
                if (Password != RepeatPassword)
                    throw new Exception("两次密码输入不一致！……");
                APIResponse loginResponse = await LoginService.Register(new UserDTO()
                {
                    Account = Account,
                    UserName = UserName,
                    Password = Password.GetMD5() // 要传递MD5hash后的密码过去
                });
                if (loginResponse.Status != APIStatusCode.Success)
                    throw new Exception(loginResponse.Message);
                ShowMessageDialog("注册成功！直接登录吧……", "Login");
                SelectIndex = 0; // 返回登陆界面
            }
            catch (Exception ex)
            {
                ShowMessageDialog(ex.Message, "Login");
            }
            finally
            {
                ShowLoadingDialog(false);
            }
        }

    }
}
