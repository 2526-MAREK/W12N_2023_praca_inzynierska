using Microsoft.EntityFrameworkCore;
using testingEnvironmentApp.Models;
using testingEnvironmentApp.Models.Alarms;

namespace testingEnvironmentApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<MsrtPoint> MsrtPoints { get; set; }
        public DbSet<Hub> Hubs { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Msrt> Msrts { get; set; }
        public DbSet<MsrtAssociation> MsrtAssociations { get; set; }
        public DbSet<Alarm> Alarms { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Msrt>()
         .HasKey(m => new { m.IdHubs, m.IdDevice, m.IdChannels, m.DataTimeMs, m.DateTimeZone });
            /*modelBuilder.Entity<Msrt>()
         .HasKey(m => m.IdMsrt); // Ustaw IdMsrt jako klucz główny*/

            modelBuilder.Entity<Msrt>()
    .HasOne<Hub>()
    .WithMany()
    .HasForeignKey(m => m.IdHubs)
    .OnDelete(DeleteBehavior.Restrict); // Zmiana z Cascade na Restrict

            modelBuilder.Entity<Msrt>()
    .HasOne<Device>()
    .WithMany()
    .HasForeignKey(m => m.IdDevice)
    .OnDelete(DeleteBehavior.Restrict); // Zmiana z Cascade na Restrict

            modelBuilder.Entity<Msrt>()
    .HasOne<Channel>()
    .WithMany()
    .HasForeignKey(m => m.IdChannels)
    .OnDelete(DeleteBehavior.Restrict); // Zmiana z Cascade na Restrict

            modelBuilder.Entity<MsrtAssociation>()
                 .HasKey(ma => new { ma.IdHubs, ma.IdDevice, ma.IdChannels, ma.DataTimeMs, ma.DateTimeZone });

            modelBuilder.Entity<MsrtAssociation>()
                .HasOne<MsrtPoint>()
                .WithMany()
                .HasForeignKey(ma => ma.IdPoint);

            modelBuilder.Entity<MsrtAssociation>()
               .HasOne<Msrt>()
               .WithMany()
               .HasForeignKey(ma => new { ma.IdHubs, ma.IdDevice, ma.IdChannels, ma.DataTimeMs, ma.DateTimeZone });


            // Konfiguracja dla tabeli device_alarms
            modelBuilder.Entity<Alarm>()
                .HasOne<Channel>()
                .WithMany()
                .HasForeignKey(da => da.IdChannel);

            // Konfiguracja dla tabeli channels
            modelBuilder.Entity<Channel>()
                .HasOne<Device>()
                .WithMany()
                .HasForeignKey(c => c.IdDevice);

            modelBuilder.Entity<Device>()
                .HasOne<Hub>()
                .WithMany()
                .HasForeignKey(c => c.IdHub);


            modelBuilder.Entity<Device>()
            .HasIndex(d => d.DeviceIdentifier)
            .IsUnique();


            modelBuilder.Entity<Channel>()
            .HasIndex(d => d.ChannelIdentifier)
            .IsUnique();

            modelBuilder.Entity<Hub>()
            .HasIndex(d => d.HubIdentifier)
            .IsUnique();

            modelBuilder.Entity<MsrtPoint>()
            .HasIndex(d => d.MsrtPointIdentifier)
            .IsUnique();
        }
    }
}
