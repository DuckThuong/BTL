using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BTL.Data;

public partial class WebNcContext : DbContext
{
    public WebNcContext()
    {
    }

    public WebNcContext(DbContextOptions<WebNcContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Lecture> Lectures { get; set; }

    public virtual DbSet<LectureReview> LectureReviews { get; set; }

    public virtual DbSet<SearchResult> SearchResults { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Courses__C92D7187C443C778");

            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.CourseName).HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Lecture>(entity =>
        {
            entity.HasKey(e => e.LectureId).HasName("PK__Lectures__B739F69F5667CC10");

            entity.Property(e => e.LectureId).HasColumnName("LectureID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Image).HasMaxLength(255);
            entity.Property(e => e.LectureName).HasMaxLength(255);
            entity.Property(e => e.Rating)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(3, 2)");
            entity.Property(e => e.ReviewCount).HasDefaultValue(0);
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.VideoUrl)
                .HasMaxLength(255)
                .HasColumnName("VideoURL");
            entity.Property(e => e.Views).HasDefaultValue(0);

            entity.HasOne(d => d.Course).WithMany(p => p.Lectures)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__Lectures__Course__17036CC0");
        });

        modelBuilder.Entity<LectureReview>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__LectureR__74BC79AE8A60A6FF");

            entity.Property(e => e.ReviewId).HasColumnName("ReviewID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LectureId).HasColumnName("LectureID");
            entity.Property(e => e.Rating).HasColumnType("decimal(3, 2)");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Lecture).WithMany(p => p.LectureReviews)
                .HasForeignKey(d => d.LectureId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LectureRe__Lectu__1CBC4616");

            entity.HasOne(d => d.User).WithMany(p => p.LectureReviews)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LectureRe__UserI__1DB06A4F");
        });

        modelBuilder.Entity<SearchResult>(entity =>
        {
            entity.HasKey(e => e.SearchId).HasName("PK__SearchRe__21C5351422050CE6");

            entity.Property(e => e.SearchId).HasColumnName("SearchID");
            entity.Property(e => e.SearchDateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SearchKeyword).HasMaxLength(255);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.SearchResults)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__SearchRes__UserI__76969D2E");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC519ACD88");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Username).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
