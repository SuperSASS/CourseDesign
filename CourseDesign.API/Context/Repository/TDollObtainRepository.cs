using Arch.EntityFrameworkCore.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace CourseDesign.API.Context.Repository
{
    public class TDollObtainRepository : Repository<TDollObtain>, IRepository<TDollObtain>
    {
        public TDollObtainRepository(DbContext dbContext) : base(dbContext) { }
    }
}
