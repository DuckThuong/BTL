using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BTL.Data;

public partial class BaiGiang2024Context : DbContext
{
    public BaiGiang2024Context()
    {
    }

    public BaiGiang2024Context(DbContextOptions<BaiGiang2024Context> options)
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
            entity.HasKey(e => e.CourseId).HasName("PK__Courses__C92D71878A2521E7");

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
            entity.HasKey(e => e.LectureId).HasName("PK__Lectures__B739F69FFA334928");

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
                .HasConstraintName("FK__Lectures__Course__59063A47");
        });

        modelBuilder.Entity<LectureReview>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__LectureR__74BC79AEAE922994");

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
                .HasConstraintName("FK__LectureRe__Lectu__5EBF139D");

            entity.HasOne(d => d.User).WithMany(p => p.LectureReviews)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LectureRe__UserI__5FB337D6");
        });

        modelBuilder.Entity<SearchResult>(entity =>
        {
            entity.HasKey(e => e.SearchId).HasName("PK__SearchRe__21C53514EDBD0A22");

            entity.Property(e => e.SearchId).HasColumnName("SearchID");
            entity.Property(e => e.SearchDateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SearchKeyword).HasMaxLength(255);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.SearchResults)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__SearchRes__UserI__6383C8BA");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACF9419F74");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Age).HasComputedColumnSql("(datepart(year,getdate())-[BirthYear])", false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.Interests).HasMaxLength(255);
            entity.Property(e => e.LastLogin).HasColumnType("datetime");
            entity.Property(e => e.Nationality).HasMaxLength(100);
            entity.Property(e => e.Occupation).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
            entity.Property(e => e.ProfilePicture).HasMaxLength(255);
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Username).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
