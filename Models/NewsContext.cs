using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace News_App.Models;

public partial class NewsContext : IdentityDbContext<User>
{


    public NewsContext(DbContextOptions<NewsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Newspage> Newspages { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    // DbSet property for RefreshToken entity - enables querying and persisting refresh tokens in the database
    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed roles into the database
        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                Id = "1",
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new IdentityRole
            {
                Id = "2",
                Name = "User",
                NormalizedName = "USER"
            },
            new IdentityRole
            {
                Id = "3",
                Name = "Publisher",
                NormalizedName = "PUBLISHER"
            },
            new IdentityRole
            {
                Id = "4",
                Name = "AdminOfPage",
                NormalizedName = "ADMINOFPAGE"
            }
        );

        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__category__3213E83F7F8A98C8");

            entity.ToTable("category");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("name");
        });
        modelBuilder.Entity<User>(b =>
        {
            b.HasIndex("NormalizedUserName")
    .IsUnique(false)
    .HasDatabaseName("UserNameIndex")
    .HasFilter("[NormalizedUserName] IS NOT NULL");
        });
        modelBuilder.Entity<Newspage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__newspage__3213E83F23451510");

            entity.ToTable("newspage");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Iscreated).HasColumnName("iscreated");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");

            // Configure Admin relationship
            entity.HasOne(n => n.Admin)
                .WithOne(u => u.Newspages)
                .HasForeignKey<Newspage>(n => n.AdminId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure many-to-many: Newspage <-> User (Publishers)
        modelBuilder.Entity<Newspage>()
            .HasMany(n => n.Publishers!)
            .WithMany(u => u.NewsPagesOfPublisher!)
            .UsingEntity<Dictionary<string, object>>(
                "NewspagePublishers",
                right => right
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey("UserId"),
                left => left
                    .HasOne<Newspage>()
                    .WithMany()
                    .HasForeignKey("NewspageId")
            );

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__post__3213E83FD562E7D8");

            entity.ToTable("post");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoryId).HasColumnName("Category_Id");
            entity.Property(e => e.NewspageId).HasColumnName("Newspage_Id");
            entity.Property(e => e.Post1)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("post");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("title");

            // Configure Image as JSON string
            entity.Property(e => e.Image)
                .HasColumnName("Image")
                .HasConversion(
                    v => v == null ? null : Newtonsoft.Json.JsonConvert.SerializeObject(v),
                    v => v == null ? null : Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(v));

            entity.HasOne(d => d.Category).WithMany(p => p.Posts)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__post__Category_I__5EBF139D");

            entity.HasOne(d => d.Newspage).WithMany(p => p.Posts)
                .HasForeignKey(d => d.NewspageId)
                .HasConstraintName("FK__post__Newspage_I__5FB337D6");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
