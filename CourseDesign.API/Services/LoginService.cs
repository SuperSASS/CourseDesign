using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using CourseDesign.API.Context;
using CourseDesign.API.Services.Interfaces;
using CourseDesign.Shared;
using CourseDesign.Shared.DTOs;
using CourseDesign.Shared.Extensions;
using CourseDesign.Shared.Parameters;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace CourseDesign.API.Services
{
    public class LoginService : ILoginService
    {
        private readonly BaseDBService<User> userDB;
        private readonly IMapper mapper;

        public LoginService(IUnitOfWork unitOfWork, IMapper mapper) { userDB = new BaseDBService<User>(unitOfWork); this.mapper = mapper; }

        // 登录
        public async Task<APIResponseInner> LoginAsync(UserDTO dtoEntity)
        {
            string account = dtoEntity.Account, passwordMD5 = dtoEntity.Password;
            Expression<Func<User, bool>> exp;
            exp = (x) => x.Account.Equals(account);
            var getUser = await userDB.GetExpressionSingalAsync(exp);
            if (getUser.Result == null || ((User)getUser.Result).Password != passwordMD5)
                return new APIResponseInner(APIStatusCode.Get_Wrong_Account_or_Password, "账号或密码错啦！检查一下呢……");
            else
                return new APIResponseInner(getUser.Result); // 成功，返回添加的用户实体
        }

        // 注册
        public async Task<APIResponseInner> RegisterAsync(UserDTO dtoEntity)
        {
            var dbEntity = mapper.Map<User>(dtoEntity);
            Expression<Func<User, bool>> exp = (x) => x.Account.Equals(dtoEntity.Account);
            // 检测账号是否存在
            var getUser = await userDB.GetExpressionSingalAsync(exp);
            if (getUser.Result != null) // 账号已存在，无法注册
                return new APIResponseInner(APIStatusCode.Get_Account_Haven, $"当前账号“{dbEntity.Account}”已被注册啦，再想想别的呢……");
            // 注册账号
            dbEntity.CreateDate = DateTime.Now;
            dbEntity.Password = dbEntity.Password;
            return await userDB.AddAsync(dbEntity); // 这里数据库的Add，ID是自增类型
        }

        // 修改用户信息
        public async Task<APIResponseInner> ChangeUserInfoAsync(UserDTO dtoEntity)
        {
            var dbEntity = mapper.Map<User>(dtoEntity);
            User getUserResult = (User)(await userDB.GetIDAsync(dbEntity.ID)).Result;
            // 对传过来的DTO的处理（只修改用户名密码是空的，只修改密码用户名是空的，账号一定是空的）
            dbEntity.Account = getUserResult.Account; // Account一定传过来是空的，因为客户端那里没存Account信息
            dbEntity.UserName ??= getUserResult.UserName;
            dbEntity.Password ??= getUserResult.Password;
            return await userDB.UpdateAsync(dbEntity);
        }
    }
}
