using Microsoft.EntityFrameworkCore;
using SmartMeterAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SmartMeterAPI.Infrastracture.Data
{
    public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
    {

        public DbSet<MeterReading> MeterReadings { get; set; }
        public DbSet<Customer> Customers { get; set; }

        // Injecting this: DbContextOptions with ctor. This way the funcs and properties of DbContextOptions are passed to ApplicationDbContext
        // Also, need it to pass DataBase Provider and default Connection String from Program.cs
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


        // Create and add contraint for your db
        protected override void OnModelCreating (ModelBuilder modelBuilder){ 
            base.OnModelCreating (modelBuilder);        // Making sure we do not edit base (DbContext) class configuration for db

            modelBuilder.Entity<Customer>()
                .HasKey(c => c.AccountId);  // Sets AccountId as the primary key

            modelBuilder.Entity<Customer>()
                .Property(c => c.AccountId)
                .ValueGeneratedNever();  // This ensures EF does not expect this to be an identity column

            // Configure Meter Reading (relationship mostly)
            modelBuilder.Entity<MeterReading>()
                .HasOne(m => m.Customer)
                .WithMany()
                .HasForeignKey(m => m.AccountId);
        }
    }
}
