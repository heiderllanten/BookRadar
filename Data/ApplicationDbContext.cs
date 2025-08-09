using Microsoft.EntityFrameworkCore;
using BookRadar.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace BookRadar.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<SearchHistory> HistorialBusquedas { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SearchHistory>(entity =>
        {
            entity.ToTable("HistorialBusquedas");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Autor).HasMaxLength(250).IsRequired();
            entity.Property(e => e.Titulo).HasMaxLength(500).IsRequired();
            entity.Property(e => e.Editorial).HasMaxLength(500);
            entity.Property(e => e.FechaConsulta).IsRequired();
        });
    }
}
