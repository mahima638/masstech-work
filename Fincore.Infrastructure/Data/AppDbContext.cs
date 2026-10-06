using Fincore.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fincore.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
       
        public DbSet<User> user { get; set; }
        public DbSet<Role> role { get; set; }
        public DbSet<RFQ> RFQs { get; set; }

        public DbSet<RFQItem> RFQItems { get; set; }

        public DbSet<VendorRFQMapping> VendorRFQMappings { get; set; }

        public DbSet<RFQQuotation> RFQQuotations { get; set; }

        public DbSet<FinalizedQuotation> FinalizedQuotations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
           

            builder.Entity<User>()
                .HasOne(u => u.role)
                .WithMany()
                .HasForeignKey(u => u.rid)
                 .OnDelete(DeleteBehavior.NoAction);


            builder.Entity<RFQItem>()
                .HasOne(x => x.RFQ)
                .WithMany(x => x.RFQItems)
                .HasForeignKey(x => x.RFQId)
                 .OnDelete(DeleteBehavior.NoAction);



            builder.Entity<VendorRFQMapping>()
                .HasOne(x => x.RFQ)
                .WithMany(x => x.RFQVendors)
                .HasForeignKey(x => x.RFQId)
                 .OnDelete(DeleteBehavior.NoAction);



            builder.Entity<RFQQuotation>()
                .HasOne(x => x.RFQ)
                .WithMany(x => x.RFQQuotations)
                .HasForeignKey(x => x.RFQId)
                 .OnDelete(DeleteBehavior.NoAction);



            builder.Entity<RFQQuotation>()
            .HasOne(x => x.Vendor)
            .WithMany()
            .HasForeignKey(x => x.VendorId)
            .OnDelete(DeleteBehavior.NoAction)
             .OnDelete(DeleteBehavior.NoAction);


            builder.Entity<VendorRFQMapping>()
                .HasOne(x => x.Vendor)
                .WithMany()
                .HasForeignKey(x => x.VendorId)
                .OnDelete(DeleteBehavior.NoAction)
                 .OnDelete(DeleteBehavior.NoAction);


            builder.Entity<FinalizedQuotation>()
                .HasOne(x => x.RFQ)
                .WithOne(x => x.FinalizedQuotation)
                .HasForeignKey<FinalizedQuotation>(x => x.RFQId)
                 .OnDelete(DeleteBehavior.NoAction);



            builder.Entity<FinalizedQuotation>()
                .HasOne(x => x.RFQQuotation)
                .WithOne(x => x.FinalizedQuotation)
                .HasForeignKey<FinalizedQuotation>(x => x.QuotationId)
                 .OnDelete(DeleteBehavior.NoAction);
        }

    }

}