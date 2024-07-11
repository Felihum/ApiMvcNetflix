using ApiMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiMvc.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Usuario> Users { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.cpf).IsRequired().HasMaxLength(11);
                entity.Property(e => e.email).IsRequired().HasMaxLength(40);
                entity.Property(e => e.password).IsRequired().HasMaxLength(100);
                entity.Property(e => e.birthday).IsRequired();
                entity.Property(e => e.role).IsRequired();
                entity.HasOne(e => e.subscription)
                      .WithMany(s => s.users)
                      .HasForeignKey(e => e.idSubscription)
                      .OnDelete(DeleteBehavior.Cascade);
                /*entity.HasMany(e => e.profiles)
                      .WithOne(p => p.usuario)
                      .HasForeignKey(p => p.idUser)
                      .OnDelete(DeleteBehavior.Cascade);*/
            });

            modelBuilder.Entity<Subscription>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.name).IsRequired().HasMaxLength(30);
                entity.Property(e => e.value).IsRequired();
                entity.Property(e => e.period).IsRequired().HasMaxLength(20);
                /*entity.HasMany(s => s.Usuarios)
                      .WithOne(u => u.subscription)
                      .HasForeignKey(u => u.idSubscription);*/
            });

            modelBuilder.Entity<Title>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.title).IsRequired().HasMaxLength(50);
                entity.Property(e => e.releaseYear).IsRequired();
                entity.Property(e => e.gender).IsRequired().HasMaxLength(20);
                entity.Property(e => e.image).IsRequired();
                entity.Property(e => e.logo);
                entity.Property(e => e.description);
                entity.Property(e => e.type).IsRequired().HasMaxLength(10);
                entity.Property(e => e.ageRating).IsRequired();
            });

            modelBuilder.Entity<Season>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.number).IsRequired();
                entity.HasOne(e => e.title)
                      .WithMany(t => t.seasons)
                      .HasForeignKey(e => e.idTitle);
                /*entity.HasMany(s => s.episodes)
                      .WithOne(e => e.season)
                      .HasForeignKey(e => e.idSeason);*/

            });

            modelBuilder.Entity<Episode>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.title).IsRequired().HasMaxLength(100);
                entity.Property(e => e.description).IsRequired();
                entity.Property(e => e.duration).IsRequired();
                entity.Property(e => e.image).IsRequired();
                entity.Property(e => e.number).IsRequired();
                entity.HasOne(e => e.season)
                      .WithMany(s => s.episodes)
                      .HasForeignKey(e => e.idSeason);
            });

            modelBuilder.Entity<Profile>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.name).IsRequired().HasMaxLength(20);
                entity.Property(e => e.type).IsRequired().HasMaxLength(20);
                entity.Property(e => e.image).IsRequired();
                entity.HasOne(e => e.usuario)
                      .WithMany(u => u.profiles)
                      .HasForeignKey(e => e.idUser);
            });
        }
    }
}