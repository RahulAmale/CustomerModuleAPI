using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CustomerModuleAPI.Models
{
    public partial class CustomerModuleContext : DbContext
    {
        public CustomerModuleContext()
        {
        }

        public CustomerModuleContext(DbContextOptions<CustomerModuleContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CustomerInfo> CustomerInfos { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }
 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerInfo>(entity =>
            {
                entity.ToTable("Customer_Info");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.EmailID).HasMaxLength(50);

                entity.Property(e => e.MobileNo1).HasMaxLength(50);

                entity.Property(e => e.MobileNo2).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
