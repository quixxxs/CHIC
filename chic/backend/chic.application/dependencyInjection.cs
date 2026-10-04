// chic.application/dependencyInjection.cs
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace chic.application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // escanea y registra todos los validadores en esta capa 
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        return services;
    }
}