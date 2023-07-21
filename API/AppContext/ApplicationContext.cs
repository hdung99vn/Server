using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.AppContext
{
    public class ApplicationContext:DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
           : base(options)
        {
        }

        public DbSet<Todo> Todos { get; set; }
    }
}
