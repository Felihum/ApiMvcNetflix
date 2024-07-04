using ApiMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiMvc.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
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
                entity.HasOne(e => e.subscription)
                      .WithMany(s => s.Usuarios)
                      .HasForeignKey(e => e.idSubscription)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Subscription>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.name).IsRequired().HasMaxLength(30);
                entity.Property(e => e.value).IsRequired();
                entity.Property(e => e.period).IsRequired().HasMaxLength(20);
                entity.HasMany(s => s.Usuarios)
                      .WithOne(u => u.subscription)
                      .HasForeignKey(u => u.idSubscription);
            });
        }
    }
}
