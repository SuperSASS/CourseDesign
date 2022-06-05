using Arch.EntityFrameworkCore.UnitOfWork;

namespace CourseDesign.API.Context.Repository
{
    public class TextPlanRepository : Repository<TextPlan>, IRepository<TextPlan>
    {
        public TextPlanRepository(CourseDesignContext dbContext) : base(dbContext) { }
    }
}
