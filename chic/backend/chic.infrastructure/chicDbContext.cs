using Microsoft.EntityFrameworkCore;
using HabitForge.Domain.Entities;

namespace chic.infrastructure.data;

public class ChicDbContext : DbContext
{
    public ChicDbContext(DbContextOptions<ChicDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }
}
