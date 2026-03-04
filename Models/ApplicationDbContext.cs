using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AvaloniaApplication6.Models;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Orderstatus> Orderstatuses { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Typelogin> Typelogins { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=DEMKA2;Username=postgres;Password=admin");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("clients_pkey");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("employees_pkey");

            entity.HasOne(d => d.Post).WithMany(p => p.Employees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("employees_postid_fkey");

            entity.HasOne(d => d.Typelogin).WithMany(p => p.Employees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("employees_typeloginid_fkey");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("orders_pkey");

            entity.HasOne(d => d.CodeclientNavigation).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orders_codeclient_fkey");

            entity.HasOne(d => d.Employee).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orders_employeeid_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orders_statusid_fkey");

            entity.HasMany(d => d.Services).WithMany(p => p.Orders)
                .UsingEntity<Dictionary<string, object>>(
                    "Orderservice",
                    r => r.HasOne<Service>().WithMany()
                        .HasForeignKey("Serviceid")
                        .HasConstraintName("orderservices_serviceid_fkey"),
                    l => l.HasOne<Order>().WithMany()
                        .HasForeignKey("Orderid")
                        .HasConstraintName("orderservices_orderid_fkey"),
                    j =>
                    {
                        j.HasKey("Orderid", "Serviceid").HasName("orderservices_pkey");
                        j.ToTable("orderservices");
                        j.IndexerProperty<int>("Orderid").HasColumnName("orderid");
                        j.IndexerProperty<string>("Serviceid")
                            .HasMaxLength(5)
                            .HasColumnName("serviceid");
                    });
        });

        modelBuilder.Entity<Orderstatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("orderstatus_pkey");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("posts_pkey");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("services_pkey");
        });

        modelBuilder.Entity<Typelogin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("typelogin_pkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
