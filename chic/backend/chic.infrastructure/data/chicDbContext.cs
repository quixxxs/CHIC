// chic.infrastructure/data/chicDbContext.cs
using Microsoft.EntityFrameworkCore;
using chic.domain;

namespace chic.infrastructure.data;

public class ChicDbContext : DbContext
{
    public ChicDbContext(DbContextOptions<ChicDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Empresa> Empresas { get; set; }
    public DbSet<Ubicacion> Ubicaciones { get; set; }
    public DbSet<Asistencia> Asistencias { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Empresa>(e =>
        {
            e.Property(x => x.Nombre).IsRequired().HasMaxLength(150);
            e.Property(x => x.Direccion).IsRequired().HasMaxLength(250);
            e.Property(x => x.Telefono).HasMaxLength(20);
        });

        modelBuilder.Entity<Ubicacion>(e =>
        {
            e.Property(x => x.Nombre).IsRequired().HasMaxLength(100);
            e.HasOne(x => x.Empresa)
             .WithMany(x => x.Ubicaciones)
             .HasForeignKey(x => x.EmpresaId)
             .OnDelete(DeleteBehavior.Restrict);   // no borra en cascada
        });

        modelBuilder.Entity<Usuario>(e =>
        {
            e.Property(x => x.Nombre).IsRequired().HasMaxLength(100);
            e.Property(x => x.Apellido).IsRequired().HasMaxLength(100);
            e.Property(x => x.Correo).IsRequired().HasMaxLength(150);
            e.Property(x => x.Telefono).HasMaxLength(20);
            e.Property(x => x.Cargo).HasMaxLength(100);
            e.HasIndex(x => x.Correo).IsUnique();
            e.HasOne(x => x.Empresa)
             .WithMany(x => x.Usuarios)
             .HasForeignKey(x => x.EmpresaId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Asistencia>(e =>
        {
            e.Property(x => x.Tipo).HasConversion<string>().HasMaxLength(20);   // guarda "Entrada"/"Salida"
            e.Property(x => x.Observacion).HasMaxLength(250);
            e.HasIndex(x => new { x.UsuarioId, x.FechaHora });
        
            e.HasOne(x => x.Usuario).WithMany()
             .HasForeignKey(x => x.UsuarioId)
             .OnDelete(DeleteBehavior.Restrict);
        
            e.HasOne(x => x.Ubicacion).WithMany()
             .HasForeignKey(x => x.UbicacionId)
             .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
