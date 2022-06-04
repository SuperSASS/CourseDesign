using Arch.EntityFrameworkCore.UnitOfWork;

namespace CourseDesign.API.Context.Repository
{
    public class UserRepository : Repository<User>, IRepository<User>
    {
        public UserRepository(CourseDesignContext dbContext) : base(dbContext) { }
    }
}
