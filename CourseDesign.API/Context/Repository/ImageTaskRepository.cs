using Arch.EntityFrameworkCore.UnitOfWork;

namespace CourseDesign.API.Context.Repository
{
    public class ImageTaskRepository : Repository<ImageTask>, IRepository<ImageTask>
    {
        public ImageTaskRepository(CourseDesignContext dbContext) : base(dbContext) { }
    }
}
