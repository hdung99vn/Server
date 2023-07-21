using API.AppContext;
using API.IServices;
using API.Services;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace API.ServiceRegister
{
    public static class ApplicationServices
    {
        public static void AddServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<ApplicationContext>(options =>
               options.UseSqlServer(
                   configuration.GetConnectionString("DefaultConnection")));
            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddTransient<ITodoService,TodoService>();
        }
    }
}
