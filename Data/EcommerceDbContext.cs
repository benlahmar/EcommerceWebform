using EcommerceWebform.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EcommerceWebform.Data
{
    public class EcommerceDbContext : DbContext
    {
       
           
            public EcommerceDbContext() : base("DefaultConnection")
            {
                
                Configuration.LazyLoadingEnabled = false;
            }

            // DbSets pour vos entités : C'est ici que vous exposez vos modèles à Entity Framework.
            public DbSet<User> Users { get; set; }
            public DbSet<Role> Roles { get; set; }
            public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                modelBuilder.Entity<User>()
                    .HasIndex(u => u.Username) 
                    .IsUnique();             

                modelBuilder.Entity<User>()
                    .HasIndex(u => u.Email)   
                    .IsUnique();             

                
                modelBuilder.Entity<User>()
                    .HasRequired(u => u.Role) // Un utilisateur doit avoir un rôle
                    .WithMany(r => r.Users)   // Un rôle peut avoir plusieurs utilisateurs
                    .HasForeignKey(u => u.RoleId); // Clé étrangère dans la table User

               
               

                // N'oubliez pas d'appeler la méthode de base en dernier
                base.OnModelCreating(modelBuilder);
            }
        }
    
}