using Microsoft.EntityFrameworkCore;
using NuxibaApi.Models;

namespace NuxibaApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Area> Areas { get; set; }
    public DbSet<LoginRecord> LoginRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        

    }
}
