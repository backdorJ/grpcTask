using GrpcHw.Task3.Server.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GrpcHw.Task3.Server.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    private AppDbContext()
    {
    }

    public DbSet<User> Users { get; set; }

    public DbSet<MessageEntity> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(typeof(Program).Assembly);
}