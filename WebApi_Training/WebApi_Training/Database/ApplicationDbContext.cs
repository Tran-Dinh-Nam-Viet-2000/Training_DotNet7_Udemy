using Microsoft.EntityFrameworkCore;
using WebApi_Training.Models;

namespace WebApi_Training.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        //Tạo các bảng
        public DbSet<Coupon> coupons { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Coupon>();
        }
    }
}
