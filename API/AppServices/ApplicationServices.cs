using API.AppContext;
using API.IServices;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

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
            services.AddSwaggerGen(options =>
            {
                // Scheme Definition 
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri("https://localhost:5001/connect/authorize"),
                            TokenUrl = new Uri("https://localhost:5001/connect/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                {"SampleAPI", "Sample API - full access"}
                            }
                        },
                    }
                });

                // Apply Scheme globally
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                        },
                        new[] { "SampleAPI" }
                    }
                });
            });
            services.AddAuthentication("Bearer")
                  .AddJwtBearer("Bearer", options =>
                  {
                      options.Authority = "https://localhost:5001";
                      options.TokenValidationParameters = new TokenValidationParameters()
                      {
                          ValidateAudience = false, // Validate 
                      };
                  });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "SampleAPI");
                });
            });
        }
    }
}
