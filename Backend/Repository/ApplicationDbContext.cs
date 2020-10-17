using Microsoft.EntityFrameworkCore;
using PublicTimeAPI.Models;
using System;

namespace PublicTimeAPI.Repository
{
  public class ApplicationDbContext : DbContext
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    { }
    public DbSet<Story> Stories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      if (modelBuilder == null)
      {
        throw new ArgumentNullException(nameof(modelBuilder));
      }

      modelBuilder.Entity<Story>(e =>
      {
        e.HasKey(o => o.Id);
        e.Property(o => o.Id).ValueGeneratedOnAdd();
      });
    }
  }
}
