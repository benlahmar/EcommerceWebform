using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EcommerceWebform.Data;
using EcommerceWebform.Models;

namespace EcommerceWebform.Dao
{
        public class UserDao
        {
            public User GetUserByUsername(string username)
            {
                using (var context = new EcommerceDbContext())
                {
                return context.Users.Include("Role")
                                 .FirstOrDefault(u => u.Username == username);
            }
            }

            // Méthode pour trouver un utilisateur par son ID
            public User GetUserById(int id)
            {
                using (var context = new EcommerceDbContext())
                {
                return context.Users.Include("Role")
                                .FirstOrDefault(u => u.Id == id);
            }
            }

            public bool UsernameExists(string username)
            {
                using (var context = new EcommerceDbContext())
                {
                    return context.Users.Any(u => u.Username == username);
                }
            }

            public bool EmailExists(string email)
            {
                using (var context = new EcommerceDbContext())
                {
                    return context.Users.Any(u => u.Email == email);
                }
            }

            public void AddUser(User user)
            {
                using (var context = new EcommerceDbContext())
                {
                    
                 


                    context.Users.Add(user);
                    context.SaveChanges();
                }
            }

            // Méthode pour mettre à jour un utilisateur existant
            public void UpdateUser(User user)
            {
                using (var context = new EcommerceDbContext())
                {
                    // Attache l'entité et marque-la comme modifiée
                    context.Entry(user).State = System.Data.Entity.EntityState.Modified;

                    
                    context.SaveChanges();
                }
            }

            // Méthode pour supprimer un utilisateur
            public void DeleteUser(int userId)
            {
                using (var context = new EcommerceDbContext())
                {
                    var userToDelete = context.Users.Find(userId);
                    if (userToDelete != null)
                    {
                        context.Users.Remove(userToDelete);
                        context.SaveChanges();
                    }
                }
            }

            // Méthode pour obtenir tous les utilisateurs (utile pour l'administration)
            public IQueryable<User> GetAllUsers()
            {
                var context = new EcommerceDbContext(); 
            return context.Users.Include("Role").AsQueryable();
        }

            // Méthode pour mettre à jour les tentatives de connexion échouées (souvent appelée après échec de login)
            public void RecordFailedLoginAttempt(User user)
            {
                using (var context = new EcommerceDbContext())
                {
                    // Attacher l'utilisateur au contexte si ce n'est pas déjà le cas
                    context.Users.Attach(user);
                    // Marquer les propriétés spécifiques comme modifiées pour éviter de charger l'entité entière
                    context.Entry(user).Property(u => u.AccessFailedCount).IsModified = true;
                    context.Entry(user).Property(u => u.LockoutEnd).IsModified = true;
                    context.SaveChanges();
                }
            }

            // Méthode pour réinitialiser les tentatives de connexion échouées (après un login réussi)
            public void ResetLoginAttempts(User user)
            {
                using (var context = new EcommerceDbContext())
                {
                    context.Users.Attach(user);
                    context.Entry(user).Property(u => u.AccessFailedCount).IsModified = true;
                    context.Entry(user).Property(u => u.LockoutEnd).IsModified = true;
                    context.Entry(user).Property(u => u.LastLoginDate).IsModified = true; // Mettre à jour aussi la date de login
                    context.SaveChanges();
                }
            }

            // Méthode pour mettre à jour la date de dernier changement de mot de passe
            public void UpdatePasswordChangedDate(User user)
            {
                using (var context = new EcommerceDbContext())
                {
                    context.Users.Attach(user);
                    context.Entry(user).Property(u => u.PasswordHash).IsModified = true; // Si le hash est mis à jour
                    context.Entry(user).Property(u => u.PasswordSalt).IsModified = true; // Si le salt est mis à jour
                    context.Entry(user).Property(u => u.PasswordLastChangedDate).IsModified = true;
                    context.SaveChanges();
                }
            }

            // Méthode pour obtenir tous les rôles (utile pour l'assignation de rôles)
            public IEnumerable<Role> GetAllRoles()
            {
               
            using (var context = new EcommerceDbContext()) 
            {
                return context.Roles.ToList();
            }
        }

            // Méthode pour obtenir un rôle par son ID
            public Role GetRoleById(int roleId)
            {
                using (var context = new EcommerceDbContext())
                {
                    return context.Roles.Find(roleId);
                }
            }
        }
    
}