using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Helpers;
using DatingApp.API.Services;
using DatingApp.API.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DatingApp.API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DataContext>(options => options.UseNpgsql(config.GetConnectionString("DefaultConnection")));
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));

            services.AddScoped<LogUserActivity>();

            services.AddSingleton<PresenceTracker>();

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPhotoService, PhotoService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}