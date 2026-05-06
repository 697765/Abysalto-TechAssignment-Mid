using AbySalto.Mid.Application.Favorite;
using AbySalto.Mid.Application.User;
using Microsoft.Extensions.DependencyInjection;

namespace AbySalto.Mid.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFavoriteService, FavoriteService>();

            return services;
        }
    }
}
