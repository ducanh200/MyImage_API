using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MyImage_API.Entities;

public partial class MyimageContext : DbContext
{
    public MyimageContext()
    {
    }

    public MyimageContext(DbContextOptions<MyimageContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Frame> Frames { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Size> Sizes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=MYIMAGE;Integrated Security=True;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__admin__3213E83F4E9EF0EA");

            entity.ToTable("admin");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__feedback__3213E83FDFCCC6AC");

            entity.ToTable("feedback");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Message)
                .HasMaxLength(255)
                .HasColumnName("message");
            entity.Property(e => e.Rate).HasColumnName("rate");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__feedback__user_i__498EEC8D");
        });

        modelBuilder.Entity<Frame>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__frame__3213E83F65B4AE72");

            entity.ToTable("frame");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FrameAmount).HasColumnName("frame_amount");
            entity.Property(e => e.FrameColorInsite)
                .HasMaxLength(255)
                .HasColumnName("frame_color_insite");
            entity.Property(e => e.FrameColorOutsite)
                .HasMaxLength(255)
                .HasColumnName("frame_color_outsite");
            entity.Property(e => e.FrameName)
                .HasMaxLength(255)
                .HasColumnName("frame_name");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__images__3213E83FA6A54C7E");

            entity.ToTable("images");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FrameId).HasColumnName("frame_id");
            entity.Property(e => e.MaterialId).HasColumnName("material_id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.SizeId).HasColumnName("size_id");
            entity.Property(e => e.Thumbnail)
                .HasMaxLength(255)
                .HasColumnName("thumbnail");

            entity.HasOne(d => d.Frame).WithMany(p => p.Images)
                .HasForeignKey(d => d.FrameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__images__frame_id__4C6B5938");

            entity.HasOne(d => d.Material).WithMany(p => p.Images)
                .HasForeignKey(d => d.MaterialId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__images__material__4F47C5E3");

            entity.HasOne(d => d.Order).WithMany(p => p.Images)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__images__order_id__4E53A1AA");

            entity.HasOne(d => d.Size).WithMany(p => p.Images)
                .HasForeignKey(d => d.SizeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__images__size_id__4D5F7D71");
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__material__3213E83FC946799E");

            entity.ToTable("material");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MaterialAmount).HasColumnName("material_amount");
            entity.Property(e => e.MaterialName)
                .HasMaxLength(255)
                .HasColumnName("material_name");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__order__3213E83FAF81CF06");

            entity.ToTable("order");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.FeedbackId).HasColumnName("feedback_id");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TotalAmount).HasColumnName("total_amount");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Feedback).WithMany(p => p.Orders)
                .HasForeignKey(d => d.FeedbackId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__order__feedback___4B7734FF");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__order__user_id__4A8310C6");
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__size__3213E83FE812D76F");

            entity.ToTable("size");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.SizeAmount).HasColumnName("size_amount");
            entity.Property(e => e.SizeHeight)
                .HasMaxLength(255)
                .HasColumnName("size_height");
            entity.Property(e => e.SizeName)
                .HasMaxLength(255)
                .HasColumnName("size_name");
            entity.Property(e => e.SizeWidth)
                .HasMaxLength(255)
                .HasColumnName("size_width");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83F8D241D7F");

            entity.ToTable("users");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.City)
                .HasMaxLength(255)
                .HasColumnName("city");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasColumnName("role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
