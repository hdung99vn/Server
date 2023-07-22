using API.AppContext;
using API.IServices;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.ServiceRegister
{
    public static class ApplicationServices
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationContext>(options =>
               options.UseSqlServer(
                   configuration.GetConnectionString("DefaultConnection")));
            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddStackExchangeRedisCache(options => { options.Configuration = configuration.GetConnectionString("RedisConnection"); });
            services.AddTransient<ITodoService, TodoService>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
