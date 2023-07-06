﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PartialZ.DataAccess.PartialZDB;

public partial class PartialClaimsContext : DbContext
{
    public PartialClaimsContext()
    {
    }

    public PartialClaimsContext(DbContextOptions<PartialClaimsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<EmailTemplate> EmailTemplates { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeWorkHistory> EmployeeWorkHistories { get; set; }

    public virtual DbSet<Employer> Employers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=APPDEVDB19;Initial Catalog=PartialClaims;Persist Security Info=True;TrustServerCertificate=True;User ID=PartialEmp_User;Password=BtFItmFmnX04clyyrIGu");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmailTemplate>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.LastModifedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Subject).HasMaxLength(500);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Employee");

            entity.Property(e => e.BusinessTitle).HasMaxLength(500);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.IsVerified).HasDefaultValueSql("((0))");
            entity.Property(e => e.LastModifedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
        });

        modelBuilder.Entity<EmployeeWorkHistory>(entity =>
        {
            entity.ToTable("EmployeeWorkHistory");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.LastModifedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PayrollEndDay).HasColumnType("datetime");
        });

        modelBuilder.Entity<Employer>(entity =>
        {
            entity.ToTable("Employer");

            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Eannumber)
                .HasMaxLength(100)
                .HasColumnName("EANNumber");
            entity.Property(e => e.EmployerEmail).HasMaxLength(100);
            entity.Property(e => e.Feinnumber)
                .HasMaxLength(100)
                .HasColumnName("FEINNumber");
            entity.Property(e => e.LastModifedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(500);
            entity.Property(e => e.State).HasMaxLength(100);
            entity.Property(e => e.ZipCode).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
