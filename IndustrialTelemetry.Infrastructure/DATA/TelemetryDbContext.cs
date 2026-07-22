using IndustrialTelemetry.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace IndustrialTelemetry.Infrastructure.DATA;

public class TelemetryDbContext : DbContext
{
    public TelemetryDbContext(DbContextOptions<TelemetryDbContext> options) : base(options)
    {
    }

    public DbSet<Equipment> Equipments { get; set; }
    public DbSet<TelemetryRecord> TelemetryRecords { get; set; }
    public DbSet<Alert> Alerts { get; set; }
    public DbSet<IdempotencyRecord> IdempotencyRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Настройка Optimistic Concurrency для Equipment
        modelBuilder.Entity<Equipment>()
            .Property(e => e.Version)
            .IsConcurrencyToken();

        // Настройка Идемпотентности: Уникальный индекс на RequestId
        modelBuilder.Entity<TelemetryRecord>()
            .HasIndex(t => t.RequestId)
            .IsUnique();

        modelBuilder.Entity<IdempotencyRecord>()
            .HasIndex(i => i.RequestId)
            .IsUnique();

        // Связи
        modelBuilder.Entity<TelemetryRecord>()
            .HasOne(t => t.Equipment)
            .WithMany(e => e.TelemetryRecords)
            .HasForeignKey(t => t.EquipmentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Alert>()
            .HasOne(a => a.Equipment)
            .WithMany(e => e.Alerts)
            .HasForeignKey(a => a.EquipmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}