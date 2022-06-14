using CourseDesign.Common.Classes;
using CourseDesign.Services.API.Interfaces;
using CourseDesign.Shared;
using CourseDesign.Shared.DTOs;
using CourseDesign.ViewModels.Bases;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Threading.Tasks;
using static CourseDesign.Context.LoginUserContext;

namespace CourseDesign.ViewModels.Settings
{
    /// <summary>
    /// 
    /// </summary>
    public class UserSettingViewModel : DialogNavigationViewModel
    {
        #region 字段
        // API服务
        private readonly ILoginService LoginService;
        // 属性内部字段
        private string userName;
        #endregion

        #region 属性
        // 命令绑定
        public DelegateCommand<string> ExecCommand { get; private set; }

        // 外部访问属性
        /// <summary>
        /// 用户名，初始会赋值为当前的用户名，后面用来更改
        /// </summary>
        public string UserName { get { return userName; } set { userName = value; RaisePropertyChanged(); } }
        #endregion

        #region 初始化
        public UserSettingViewModel(IContainerProvider containerProvider) : base(containerProvider)
        {
            LoginService = containerProvider.Resolve<ILoginService>();
            ExecCommand = new DelegateCommand<string>(Exec);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            UserName = LoginUser.UserName;
        }
        #endregion

        #region 方法
        public async void Exec(string cmd)
        {
            switch (cmd)
            {
                case "修改用户名": await ChangeUserNameAsync(); break;
                case "修改密码": ChangePassword(); break;
                default: ShowMessageDialog("内部错误 - 调用了不存在的命令，快让程序员来修！……", "Main"); break;
            }
        }
        #endregion

        #region 内部方法
        /// <summary>
        /// 修改用户名
        /// </summary>
        private async Task ChangeUserNameAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(UserName))
                    throw new Exception("新改的用户名不能为空哦！……"); // 返回错误提示
                ShowLoadingDialog(true);
                APIResponse<UserDTO> changeResponse = await LoginService.ChangeUserInfo(new UserDTO()
                {
                    ID = LoginUser.UserID,
                    UserName = UserName
                });
                if (changeResponse.Status != APIStatusCode.Success)
                    throw new Exception(changeResponse.Message);
                // 更改本地用户上下文
                UserClass changedUser = LoginUser;
                changedUser.UserName = changeResponse.Result.UserName;
                ChangeUserInfo(changedUser);
                ShowMessageDialog($"成功修改用户名！你现在叫“{UserName}”啦……", "Main");
                // 通知MainWindow更新用户上下文
                App.UpdateUser(changedUser);
            }
            catch (Exception ex)
            {
                ShowMessageDialog(ex.Message, "Main");
            }
            finally
            {
                ShowLoadingDialog(false);
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        private void ChangePassword()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
