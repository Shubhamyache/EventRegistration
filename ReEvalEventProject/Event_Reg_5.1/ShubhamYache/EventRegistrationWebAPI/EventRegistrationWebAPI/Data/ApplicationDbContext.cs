using EventRegistrationWebAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Net.Sockets;
using EventRegistrationWebAPI.DTOs.RegistrationDto;

namespace EventRegistrationWebAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Venue> Venues { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Payment> Payments { get; set; }    

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuring many-to-many relationship for EventArtists

            modelBuilder.Entity<Event>()
               .HasOne(e => e.Organizer)
               .WithMany(o => o.Events)
               .HasForeignKey(e => e.OrganizerId);

            modelBuilder.Entity<Event>()
                .HasIndex(e => e.EventId)
                .HasDatabaseName("IX_Event_EventId");

            // Index on RegistrationId in Registration model
            modelBuilder.Entity<Registration>()
                .HasIndex(r => r.RegistrationId)
                .HasDatabaseName("IX_Registration_RegistrationId");

            // Explicitly configure the foreign key
            modelBuilder.Entity<Registration>()
                .HasOne(r => r.User)
                .WithMany(u => u.Registrations)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Payment>()
              .Property(p => p.PaymentAmount)
              .HasColumnType("decimal(10, 2)");

            modelBuilder.Entity<Registration>()
              .HasOne(r => r.Payment)
              .WithOne(p => p.Registration)
              .HasForeignKey<Registration>(r => r.PaymentId); 
        }
    }
}
