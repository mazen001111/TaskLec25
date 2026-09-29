using Microsoft.EntityFrameworkCore;
using TaskLec25.Models;

namespace TaskLec25.Data
{
    public class ApplicationDbContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<MyTask> Tasks { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer("Data Source=MAZEN\\SQLEXPRESS;Integrated Security=True;database=ToDoList;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Application Name=\"Microsoft SQL Server Data Tools, SQL Server Object Explorer\";Command Timeout=0");
        }
    }
}
