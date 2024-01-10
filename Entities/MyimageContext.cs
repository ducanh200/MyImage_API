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

    public virtual DbSet<Hanger> Hangers { get; set; }

    public virtual DbSet<ImageUrl> ImageUrls { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderImage> OrderImages { get; set; }

    public virtual DbSet<Size> Sizes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=MYIMAGE;Integrated Security=True;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__admin__3213E83F4A3694B0");

            entity.ToTable("admin");

            entity.HasIndex(e => e.Email, "UQ__admin__AB6E616408A3AE8B").IsUnique();

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
            entity.HasKey(e => e.Id).HasName("PK__feedback__3213E83FE3905E9A");

            entity.ToTable("feedbacks");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Message)
                .HasMaxLength(255)
                .HasColumnName("message");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__feedbacks__user___1E3A7A34");
        });

        modelBuilder.Entity<Frame>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__frames__3213E83FD45C4204");

            entity.ToTable("frames");

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

        modelBuilder.Entity<Hanger>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__hangers__3213E83F01ADF5B5");

            entity.ToTable("hangers");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.HangerAmount).HasColumnName("hanger_amount");
            entity.Property(e => e.HangerName)
                .HasMaxLength(255)
                .HasColumnName("hanger_name");
        });

        modelBuilder.Entity<ImageUrl>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__image_ur__3213E83FBE5CBBD5");

            entity.ToTable("image_url");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Thumbnail)
                .HasMaxLength(255)
                .HasColumnName("thumbnail");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__orders__3213E83FB2C6A5F8");

            entity.ToTable("orders");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.City)
                .HasMaxLength(255)
                .HasColumnName("city");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FeedbackId).HasColumnName("feedback_id");
            entity.Property(e => e.Phone)
                .HasMaxLength(25)
                .HasColumnName("phone");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TotalAmount).HasColumnName("total_amount");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Feedback).WithMany(p => p.Orders)
                .HasForeignKey(d => d.FeedbackId)
                .HasConstraintName("FK__orders__feedback__2022C2A6");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__orders__user_id__1F2E9E6D");
        });

        modelBuilder.Entity<OrderImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__order_im__3213E83FBC69E0AB");

            entity.ToTable("order_images");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.FrameId).HasColumnName("frame_id");
            entity.Property(e => e.HangerId).HasColumnName("hanger_id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.SizeId).HasColumnName("size_id");
            entity.Property(e => e.Thumbnail)
                .HasMaxLength(255)
                .HasColumnName("thumbnail");

            entity.HasOne(d => d.Frame).WithMany(p => p.OrderImages)
                .HasForeignKey(d => d.FrameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__order_ima__frame__2116E6DF");

            entity.HasOne(d => d.Hanger).WithMany(p => p.OrderImages)
                .HasForeignKey(d => d.HangerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__order_ima__hange__220B0B18");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderImages)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__order_ima__order__23F3538A");

            entity.HasOne(d => d.Size).WithMany(p => p.OrderImages)
                .HasForeignKey(d => d.SizeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__order_ima__size___22FF2F51");
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__sizes__3213E83F1F7FC81E");

            entity.ToTable("sizes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.SizeAmount).HasColumnName("size_amount");
            entity.Property(e => e.SizeName)
                .HasMaxLength(255)
                .HasColumnName("size_name");
            entity.Property(e => e.SizeWidth)
                .HasMaxLength(255)
                .HasColumnName("size_width");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83F2A185507");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "UQ__users__AB6E61641FBC6ED6").IsUnique();

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
                .HasMaxLength(25)
                .HasColumnName("phone");
            entity.Property(e => e.Role)
                .HasMaxLength(255)
                .HasColumnName("role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
