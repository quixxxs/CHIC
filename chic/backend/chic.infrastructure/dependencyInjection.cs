// chic.infrastructure/dependencyInjection.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using chic.application.interfaces;
using chic.infrastructure.data;
using chic.infrastructure.repositories;

namespace chic.infrastructure;

public static class DependencyInjection {
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString) {
        
        // 1. Configuramos el acceso a PostgreSQL
        services.AddDbContext<ChicDbContext>(options =>
            options.UseNpgsql(connectionString));

        // 2. LA INYECCIÓN CLAVE
        services.AddScoped<IChicRepository, chicRepository>();
        services.AddScoped<IEmpresaRepository, EmpresaRepository>();
        services.AddScoped<IUbicacionRepository, UbicacionRepository>();
        services.AddScoped<IAsistenciaRepository, AsistenciaRepository>();

        return services;
    }
}