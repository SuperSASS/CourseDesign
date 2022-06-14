using Arch.EntityFrameworkCore.UnitOfWork;

namespace CourseDesign.API.Context.Repository
{
    public class ImagePlanRepository : Repository<ImagePlan>, IRepository<ImagePlan>
    {
        public ImagePlanRepository(CourseDesignContext dbContext) : base(dbContext) { }
    }
}
