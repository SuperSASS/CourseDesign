using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using CourseDesign.API.Context;
using CourseDesign.API.Services.Interfaces;
using CourseDesign.Shared.DTOs;
using CourseDesign.Shared.Parameters;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using static CourseDesign.API.Services.APIResponse;

namespace CourseDesign.API.Services
{
    public class Test : BaseEntity
    {
        public string Account { get; set; }
        public int Roll { get; set; }
    }


    public class LoginService : ILoginService
    {
        private readonly BasicSQLService<User> userDB;
        private readonly IMapper mapper;

        public LoginService(IUnitOfWork unitOfWork, IMapper mapper) { userDB = new BasicSQLService<User>(unitOfWork); this.mapper = mapper; }

        /// <summary>
        /// 用户登录的实现
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponse"/></returns>
        public async Task<APIResponse> LoginAsync(string account, string password)
        {
            Expression<Func<User, bool>> exp;
            exp = (x) => x.Account.Equals(account);
            var getUser = await userDB.GetExpressionSingalAsync(exp);
            if (getUser.Result == null || ((User)getUser.Result).Password != password)
                return new APIResponse(StatusCode.Get_Wrong_Account_or_Password, "账号或密码错啦！检查一下呢……");
            else
                return new APIResponse();
        }

        /// <summary>
        /// 用户注册的实现
        /// </summary>
        /// <param name="entity">注册实体：账号、用户名、密码</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponse"/></returns>
        public async Task<APIResponse> RegisterAsync(UserDTO entity)
        {
            var dbEntity = mapper.Map<User>(entity);
            Expression<Func<User, bool>> exp = (x) => x.Account.Equals(entity.Account);

            var getUser = await userDB.GetExpressionSingalAsync(exp);
        
            if (getUser.Result != null) // 账号已存在，无法注册
                return new APIResponse(StatusCode.Get_Account_Haven, $"当前账号“{dbEntity.Account}”已被注册啦，再想想别的呢……");

            // TODO:2 - 警告，ID没有变化，但好像是递增的
            return await userDB.AddAsync(dbEntity);
        }
    }
}
