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
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<Realisateur> Realisateurs { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Commande> Commandes { get; set; }

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

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("username");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("password");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("firstname");               
                    
                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("lastname");

                entity.Property(e => e.Role)
                    .HasConversion<string>()
                    .HasMaxLength(255)
                    .HasColumnName("role");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("creation_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnName("updated_date")
                    .HasColumnType("datetime");

            });
            
            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("address");

                entity.Property(e => e.Id)
                    .HasColumnName("id"); 
                        
                entity.Property(e => e.Street)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("street");
                    
                entity.Property(e => e.PostalCode)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("postal_code");
                    
                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("city");
                    
                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("country");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("creation_date")
                    .HasColumnType("datetime");
                
                entity.Property(e => e.UpdatedDate)
                    .HasColumnName("updated_date")
                    .HasColumnType("datetime");

            });

            modelBuilder.Entity<Article>(entity =>
            {
                entity.ToTable("article");

                //entity.HasIndex(e => e.RealisateurId, "realisateur_id");

                entity.Property(e => e.Id)                    
                    .HasColumnName("id");
/*
                 entity.HasOne( e => e.Realisateur)
                    .WithMany( u => u.Article )
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasForeignKey("realisateur_id") ;
*/

                entity.Property(e => e.Titre)
                    .HasMaxLength(200)
                    .HasColumnName("titre");
                    
                entity.Property(e => e.Duree)
                    .HasColumnName("duree");

                entity.Property(e => e.Categorie)
                    .HasMaxLength(200)
                    .HasColumnName("categorie");

                entity.Property(e => e.Synopsis)
                    .HasMaxLength(1000)
                    .HasColumnName("synopsis");

                entity.Property(e => e.TypeArticle)
                    .HasConversion<string>()
                    .HasColumnName("type_article");

                entity.Property(e => e.DateSortie)
                    .HasColumnType("datetime")
                    .HasColumnName("date_sortie");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("creation_date")
                    .HasDefaultValueSql("getdate()");;

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_date");
            });

            modelBuilder.Entity<Commande>(entity =>
            {
                entity.ToTable("commande");

                entity.HasIndex(e => e.ArticleId, "article_id");

                entity.HasIndex(e => e.UserId, "user_id");

                entity.Property(e => e.Id)
                    .HasColumnName("id");

                entity.HasOne(e => e.Article)
                .WithMany(u => u.Commande)
                    .HasForeignKey("article_id")
                    .OnDelete(DeleteBehavior.Cascade);

               entity.HasOne(e => e.User)
                .WithMany(u => u.Commande)
                    .HasForeignKey("user_id")
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("creation_date")
                    .HasDefaultValueSql("getdate()");;
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("comment");

                entity.HasIndex(e => e.ArticleId, "article_id");

                entity.HasIndex(e => e.UserId, "user_id");

                entity.Property(e => e.Id)
                    .HasColumnName("id");

                     entity.HasOne(e => e.Article)
                .WithMany(u => u.Comment)
                    .HasForeignKey("article_id")
                    .OnDelete(DeleteBehavior.Cascade);

               entity.HasOne(e => e.User)
                .WithMany(u => u.Comment)
                    .HasForeignKey("user_id")
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.Contenu)
                    .HasMaxLength(1000)
                    .HasColumnName("contenu");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("creation_date")
                    .HasDefaultValueSql("getdate()");;

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_date");
            });

            modelBuilder.Entity<Realisateur>(entity =>
            {
                entity.ToTable("realisateur");

                entity.Property(e => e.Id)
                    .HasColumnName("id");

                entity.Property(e => e.NomComplet)
                    .HasMaxLength(200)
                    .HasColumnName("nom_complet");

                entity.Property(e => e.Nationalite)
                    .HasMaxLength(200)
                    .HasColumnName("nationalite");

                entity.Property(e => e.Photo)
                    .HasMaxLength(200)
                    .HasColumnName("photo");

                entity.Property(e => e.Biographie)
                    .HasMaxLength(200)
                    .HasColumnName("biographie");

                entity.Property(e => e.Naissance)
                    .HasColumnType("datetime")
                    .HasColumnName("naissance");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("creation_date")
                    .HasDefaultValueSql("getdate()");;

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("update_date");
            });

           
        }
    }   
}
