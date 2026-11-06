using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MusicaApi.Models;

namespace MusicaApi.Data;

public partial class MusicaContext : DbContext
{
    public MusicaContext(DbContextOptions<MusicaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Album> Albums { get; set; }

    public virtual DbSet<Cancion> Cancions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Album>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Album__3214EC07C39193DB");
        });

        modelBuilder.Entity<Cancion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cancion__3214EC072841E74E");

            entity.HasOne(d => d.Album).WithMany(p => p.Cancions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cancion_Album");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
