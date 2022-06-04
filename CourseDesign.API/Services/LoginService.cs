using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using CourseDesign.API.Context;
using CourseDesign.API.Services.Interfaces;
using CourseDesign.Shared.DTOs;
using System;
using System.Threading.Tasks;

namespace CourseDesign.API.Services
{
    public class LoginService : ILoginService
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly IRepository<User> UserRepo;
        private readonly IMapper Mapper;

        public LoginService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            UnitOfWork = unitOfWork;
            UserRepo = UnitOfWork.GetRepository<User>();
            Mapper = mapper;
        }

        /// <summary>
        /// 用户登录的实现
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public async Task<APIResponse> LoginAsync(string account, string password)
        {
            try
            {
                var entity = await UserRepo.GetFirstOrDefaultAsync(predicate: x => (x.Account.Equals(account)) && (x.Password.Equals(password)));
                if (entity == null)
                    return new APIResponse("账号或密码错啦！检查一下呢……");
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return new APIResponse(ex.Message);
            }
        }

        /// <summary>
        /// 用户注册的实现
        /// </summary>
        /// <param name="entity">注册实体：账号、用户名、密码</param>
        /// <returns></returns>
        public async Task<APIResponse> Register(UserDTO entity)
        {
            try
            {
                var dbEntity = Mapper.Map<User>(entity);
                var user = await UserRepo.GetFirstOrDefaultAsync(predicate: x => x.Account.Equals(dbEntity.Account));
                if (user != null) // 账号已存在，无法注册
                    return new APIResponse($"当前账号“{dbEntity.Account}”已被注册啦，再想想别的呢……");

                // TODO:警告，ID没有变化，但好像是递增的
                dbEntity.CreateDate = DateTime.Now;
                await UserRepo.InsertAsync(dbEntity);
                if (await UnitOfWork.SaveChangesAsync() > 0)
                    return new APIResponse(dbEntity);
                return new APIResponse("注册失败……肯定不是服务器的问题！……");
            }
            catch (Exception ex)
            {
                return new APIResponse(ex.Message);
            }
        }
    }
}
