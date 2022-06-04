using Arch.EntityFrameworkCore.UnitOfWork;

namespace CourseDesign.API.Context.Repository
{
    public class TDollRepository : Repository<TDoll>, IRepository<TDoll>
    {
        public TDollRepository(CourseDesignContext dbContext) : base(dbContext) { }
    }
}
