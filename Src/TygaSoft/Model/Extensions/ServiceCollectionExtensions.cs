using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace TygaSoft.Model
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg =>
                        {
                            cfg.AddProfile(new DtoProfile());
                        });
            services.AddSingleton(sp => config.CreateMapper());

            services.AddScoped<IDtoMapper, DtoMapper>();

            return services;
        }
    }
}