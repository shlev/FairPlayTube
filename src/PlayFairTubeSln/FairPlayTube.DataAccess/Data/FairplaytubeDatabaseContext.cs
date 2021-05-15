﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using FairPlayTube.DataAccess.Models;

#nullable disable

namespace FairPlayTube.DataAccess.Data
{
    public partial class FairplaytubeDatabaseContext : DbContext
    {
        public FairplaytubeDatabaseContext()
        {
        }

        public FairplaytubeDatabaseContext(DbContextOptions<FairplaytubeDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ApplicationRole> ApplicationRole { get; set; }
        public virtual DbSet<ApplicationUser> ApplicationUser { get; set; }
        public virtual DbSet<ApplicationUserRole> ApplicationUserRole { get; set; }
        public virtual DbSet<ErrorLog> ErrorLog { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Scaffolding:ConnectionString", "Data Source=(local);Initial Catalog=FairPlayTube.Database;Integrated Security=true");

            modelBuilder.Entity<ApplicationUserRole>(entity =>
            {
                entity.HasOne(d => d.ApplicationRole)
                    .WithMany(p => p.ApplicationUserRole)
                    .HasForeignKey(d => d.ApplicationRoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApplicationUserRole_ApplicationRole");

                entity.HasOne(d => d.ApplicationUser)
                    .WithOne(p => p.ApplicationUserRole)
                    .HasForeignKey<ApplicationUserRole>(d => d.ApplicationUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApplicationUserRole_ApplicationUser");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}