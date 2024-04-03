using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;
using quest_web.Models ;

namespace quest_web
{
    
    public class APIDbContext : DbContext
    {
        public APIDbContext()
        {
        }
        
        public APIDbContext(DbContextOptions<APIDbContext> options) : base(options) { }
        
        public virtual DbSet<User> Users { get; set; }

            
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "server=127.0.0.1;database=quest_web;user=application;password=password";
            //var serverVersion = ServerVersion.AutoDetect(connectionString) ; // new MySqlServerVersion(new Version(8, 0, 28));
    
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.HasIndex(e => e.Username, "username")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreationDate).HasColumnName("creation_date").HasColumnType("datetime");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("password");

                entity.Property(e => e.Role)
                    .HasConversion<string>()
                    .HasMaxLength(255)
                    .HasColumnName("role");

                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date").HasColumnType("datetime");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("username");

            });
        }
    }   
}
