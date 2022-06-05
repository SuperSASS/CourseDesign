using Microsoft.EntityFrameworkCore;

namespace CourseDesign.API.Context
{
    /// <summary>
    /// 数据库共享的上下文
    /// </summary>
    public class CourseDesignContext : DbContext
    {
        // 所有的表
        public DbSet<User> Users { get; set; }
        public DbSet<TextPlan> TextPlans { get; set; }
        public DbSet<ImagePlan> ImagePlans { get; set; }
        public DbSet<TDoll> TDolls { get; set; }
        public DbSet<TDollObtain> TDollsObtain { get; set; }

        // 数据库上下文
        public CourseDesignContext(DbContextOptions<CourseDesignContext> options) : base(options) { }
    }
}
