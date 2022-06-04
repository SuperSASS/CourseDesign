using Arch.EntityFrameworkCore.UnitOfWork;

namespace CourseDesign.API.Context.Repository
{
    public class TextTaskRepository : Repository<TextTask>, IRepository<TextTask>
    {
        public TextTaskRepository(CourseDesignContext dbContext) : base(dbContext) { }
    }
}
