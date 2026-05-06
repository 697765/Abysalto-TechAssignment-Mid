using AbySalto.Mid.WebApi.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;

namespace AbySalto.Mid
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddOpenApi();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AbySalto", Version = "v1" });
            });
            services.AddValidatorsFromAssemblyContaining<RegistrationRequestValidator>();
            services.AddFluentValidationAutoValidation();

            return services;
        }
    }
}
