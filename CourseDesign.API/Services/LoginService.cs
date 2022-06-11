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
        public async Task<APIResponseInner> LoginAsync(string account, string passwordMD5)
        {
            Expression<Func<User, bool>> exp;
            exp = (x) => x.Account.Equals(account);
            var getUser = await userDB.GetExpressionSingalAsync(exp);
            if (getUser.Result == null || ((User)getUser.Result).Password != passwordMD5)
                return new APIResponseInner(APIStatusCode.Get_Wrong_Account_or_Password, "账号或密码错啦！检查一下呢……");
            else
                return new APIResponseInner();
        }

        // 注册
        public async Task<APIResponseInner> RegisterAsync(UserDTO entity)
        {
            var dbEntity = mapper.Map<User>(entity);
            Expression<Func<User, bool>> exp = (x) => x.Account.Equals(entity.Account);
            // 检测账号是否存在
            var getUser = await userDB.GetExpressionSingalAsync(exp);
            if (getUser.Result != null) // 账号已存在，无法注册
                return new APIResponseInner(APIStatusCode.Get_Account_Haven, $"当前账号“{dbEntity.Account}”已被注册啦，再想想别的呢……");
            // 注册账号
            dbEntity.CreateDate = DateTime.Now;
            dbEntity.Password = dbEntity.Password.GetMD5();
            return await userDB.AddAsync(dbEntity); // 这里数据库的Add，ID是自增类型
        }
    }
}
