using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ReservasAPI.Models;

namespace ReservasAPI.Data;

public partial class DBReservasContext : DbContext
{
    public DBReservasContext()
    {
    }

    public DBReservasContext(DbContextOptions<DBReservasContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Reserva> Reservas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasKey(e => e.IdReserva).HasName("PK__Reserva__0E49C69DF3F8B2FA");

            entity.ToTable("Reserva");

            entity.Property(e => e.Cantidad).HasDefaultValue(1);
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Pendiente");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaFin).HasColumnType("datetime");
            entity.Property(e => e.FechaInicio).HasColumnType("datetime");
            entity.Property(e => e.FechaReserva).HasColumnType("datetime");
            entity.Property(e => e.Observaciones).HasMaxLength(250);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
