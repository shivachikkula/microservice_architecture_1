using Microsoft.EntityFrameworkCore;
using StudentService.Models;

namespace StudentService.Data;

public class StudentDbContext : DbContext
{
    public StudentDbContext(DbContextOptions<StudentDbContext> options)
        : base(options)
    {
    }

    public DbSet<StudentDetails> Students { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<StudentDetails>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            entity.HasIndex(e => new { e.FirstName, e.LastName });
        });
    }
}
