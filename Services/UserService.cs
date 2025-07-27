using EcommerceWebform.Dao;
using EcommerceWebform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace EcommerceWebform.Services
{
    public class UserService
    {
        private readonly UserDao _userDAO;

        public UserService()
        {
            _userDAO = new UserDao();
        }

        // --- Politiques de Sécurité (à lire depuis AppSettings.config en production) ---
        // Pour cet exemple, nous utilisons des constantes.
        // En réalité, ces valeurs proviendraient de configurations externes (Web.config/AppSettings).
        private const int MAX_FAILED_LOGIN_ATTEMPTS = 5;
        private const int ACCOUNT_LOCKOUT_MINUTES = 15;
        private const int PASSWORD_EXPIRATION_DAYS = 90; // Expiration du mot de passe tous les 90 jours

        public User GetUserByUsername(string username)
        {
            // You might add validation or business logic here if needed before calling the DAO,
            // but for a simple retrieval, direct delegation is fine.
            return _userDAO.GetUserByUsername(username);
        }
        public bool RegisterUser(User newUser, out string errorMessage)
        {
            errorMessage = string.Empty;

            // Validation métier : unicité du nom d'utilisateur et de l'email
            if (_userDAO.UsernameExists(newUser.Username))
            {
                errorMessage = "Ce nom d'utilisateur est déjà pris.";
                return false;
            }
            if (_userDAO.EmailExists(newUser.Email))
            {
                errorMessage = "Cette adresse email est déjà utilisée.";
                return false;
            }

            // Générer un salt aléatoire pour le hachage
            newUser.PasswordSalt = GenerateSalt();
            // Hacher le mot de passe avant de le stocker
            newUser.PasswordHash = HashPassword(newUser.Password, newUser.PasswordSalt);

            // Définir les valeurs par défaut pour un nouvel utilisateur
            newUser.IsEnabled = true; // Compte actif par défaut
            newUser.AccessFailedCount = 0; // Aucune tentative échouée
            newUser.LockoutEnd = null; // Pas verrouillé
            newUser.PasswordLastChangedDate = DateTime.UtcNow; // Le mot de passe vient d'être défini
            newUser.LastLoginDate = null; // Pas encore connecté

            // Assigner un rôle par défaut (ex: 'User')
            // Il faudrait récupérer le Role 'User' de la DB pour s'assurer d'avoir le bon RoleId
            var defaultRole = _userDAO.GetAllRoles().FirstOrDefault(r => r.Name == "User");
            if (defaultRole == null)
            {
                errorMessage = "Le rôle par défaut 'User' n'existe pas. Veuillez configurer les rôles.";
                return false;
            }
            newUser.RoleId = defaultRole.Id;
           // newUser.Role = defaultRole; // Attache l'objet Role pour EF

            try
            {
                _userDAO.AddUser(newUser);
                return true;
            }
            catch (Exception ex)
            {
                // Journaliser l'exception réelle ici (ex: NLog, log4net)
                errorMessage = "Une erreur est survenue lors de l'inscription. Veuillez réessayer.  "+ ex.Message;
                 Console.WriteLine(ex.Message); // Pour le débogage
                return false;
            }
        }

        // Connexion d'un utilisateur
        public User AuthenticateUser(string username, string password, out string errorMessage)
        {
            errorMessage = string.Empty;
            var user = _userDAO.GetUserByUsername(username);

            if (user == null)
            {
                errorMessage = "Nom d'utilisateur ou mot de passe incorrect.";
                return null;
            }

            // Vérifications des politiques de sécurité avant de vérifier le mot de passe
            if (!user.IsEnabled)
            {
                errorMessage = "Votre compte est désactivé. Veuillez contacter l'administrateur.";
                return null;
            }

            if (IsAccountLocked(user)) // Utilise la logique de verrouillage
            {
                errorMessage = $"Votre compte est temporairement verrouillé. Veuillez réessayer après {ACCOUNT_LOCKOUT_MINUTES} minutes.";
                return null;
            }

            // Vérifier le mot de passe
            if (VerifyPassword(password, user.PasswordHash, user.PasswordSalt ?? string.Empty)) // Gérer le cas où salt est null
            {
                // Connexion réussie : réinitialiser les tentatives échouées et mettre à jour la date de dernière connexion
                user.AccessFailedCount = 0;
                user.LockoutEnd = null;
                user.LastLoginDate = DateTime.UtcNow;
                try
                {
                    _userDAO.ResetLoginAttempts(user); // Met à jour l'utilisateur en DB
                }
                catch (Exception ex)
                {
                    // Journaliser l'exception : échec de mise à jour du compteur
                    Console.WriteLine($"Erreur lors de la réinitialisation des tentatives de connexion pour {user.Username}: {ex.Message}");
                }

                // Vérifier l'expiration du mot de passe
                if (!IsCredentialsNonExpired(user))
                {
                    errorMessage = "Votre mot de passe a expiré et doit être changé.";
                    // L'utilisateur est connecté mais sera redirigé vers la page de changement de mot de passe.
                    return user;
                }

                return user;
            }
            else // Mot de passe incorrect
            {
                // Enregistrer l'échec de connexion et potentiellement verrouiller le compte
                user.AccessFailedCount++;
                if (user.AccessFailedCount >= MAX_FAILED_LOGIN_ATTEMPTS)
                {
                    user.LockoutEnd = DateTime.UtcNow.AddMinutes(ACCOUNT_LOCKOUT_MINUTES);
                }
                try
                {
                    _userDAO.RecordFailedLoginAttempt(user); // Met à jour l'utilisateur en DB
                }
                catch (Exception ex)
                {
                    // Journaliser l'exception
                    Console.WriteLine($"Erreur lors de l'enregistrement de l'échec de connexion pour {user.Username}: {ex.Message}");
                }

                errorMessage = "Nom d'utilisateur ou mot de passe incorrect.";
                return null;
            }
        }

        // --- Opérations de Gestion de Compte ---

        // Changer le mot de passe d'un utilisateur
        public bool ChangePassword(int userId, string oldPassword, string newPassword, out string errorMessage)
        {
            errorMessage = string.Empty;
            var user = _userDAO.GetUserById(userId);

            if (user == null)
            {
                errorMessage = "Utilisateur introuvable.";
                return false;
            }

            // Vérifier l'ancien mot de passe
            if (!VerifyPassword(oldPassword, user.PasswordHash, user.PasswordSalt ?? string.Empty))
            {
                errorMessage = "L'ancien mot de passe est incorrect.";
                return false;
            }

            // Hacher le nouveau mot de passe
            string newSalt = GenerateSalt();
            user.PasswordHash = HashPassword(newPassword, newSalt);
            user.PasswordSalt = newSalt; // Mettre à jour le salt si un nouveau est généré
            user.PasswordLastChangedDate = DateTime.UtcNow; // Mettre à jour la date de changement

            try
            {
                _userDAO.UpdatePasswordChangedDate(user); // Mettre à jour en DB
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = "Une erreur est survenue lors du changement de mot de passe.";
                // Journaliser l'exception
                return false;
            }
        }

        // Mettre à jour les informations de profil de l'utilisateur (email, par exemple)
        public bool UpdateUserProfile(User userToUpdate, out string errorMessage)
        {
            errorMessage = string.Empty;

            // Vérifier l'unicité de l'email si l'email est changé
            var existingUserWithEmail = _userDAO.GetAllUsers().FirstOrDefault(u => u.Email == userToUpdate.Email && u.Id != userToUpdate.Id);
            if (existingUserWithEmail != null)
            {
                errorMessage = "Cette adresse email est déjà utilisée par un autre compte.";
                return false;
            }

            try
            {
                _userDAO.UpdateUser(userToUpdate); // Le DAO gère la mise à jour
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = "Une erreur est survenue lors de la mise à jour du profil.";
                // Journaliser l'exception
                return false;
            }
        }


        // --- Fonctions de Hachage et Vérification de Mot de Passe (Utilise PBKDF2) ---

        // Génère un salt aléatoire sécurisé
        private string GenerateSalt()
        {
            byte[] salt = new byte[16]; // 128 bits de salt
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        // Hache un mot de passe avec un salt et PBKDF2
        private string HashPassword(string password, string salt)
        {
            // PBKDF2 est recommandé pour le hachage de mot de passe
            // Paramètres recommandés : au moins 100 000 itérations, taille de clé de 256 bits
            int iterations = 100000;
            byte[] saltBytes = Convert.FromBase64String(salt);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, iterations, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(32); // 32 octets = 256 bits
                // Concaténer les itérations, le salt et le hash pour le stockage
                return $"{iterations}.{Convert.ToBase64String(saltBytes)}.{Convert.ToBase64String(hash)}";
            }
        }

        // Vérifie un mot de passe soumis avec le hash stocké
        private bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            try
            {
                // Extraire les composants du hash stocké
                string[] parts = storedHash.Split('.');
                if (parts.Length != 3) return false; // Format invalide

                int iterations = int.Parse(parts[0]);
                byte[] saltBytes = Convert.FromBase64String(parts[1]);
                byte[] hashBytes = Convert.FromBase64String(parts[2]);

                using (var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, saltBytes, iterations, HashAlgorithmName.SHA256))
                {
                    byte[] enteredHash = pbkdf2.GetBytes(32); // Hacher le mot de passe entré
                    // Comparer les hachages octet par octet pour éviter les attaques de timing
                    for (int i = 0; i < 32; i++)
                    {
                        if (hashBytes[i] != enteredHash[i])
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            catch (FormatException) // Si le format Base64 est incorrect
            {
                return false;
            }
            catch (OverflowException) // Si les itérations sont trop grandes
            {
                return false;
            }
            catch (Exception ex)
            {
                // Loguer l'erreur de vérification (potentiellement une tentative d'attaque)
                Console.WriteLine($"Erreur lors de la vérification du mot de passe : {ex.Message}");
                return false;
            }
        }


        // --- Méthodes de Vérification des Politiques ---
        // Elles encapsulent la logique de la classe User et les constantes de politique.

        public bool IsAccountLocked(User user)
        {
            // Un compte est verrouillé si LockoutEnd n'est pas null ET la date de fin de verrouillage est future.
            return user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.UtcNow;
        }

        public bool IsCredentialsNonExpired(User user)
        {
            // La logique est déjà dans le modèle User, nous l'appelons avec la politique configurée
            return user.IsCredentialsNonExpired(PASSWORD_EXPIRATION_DAYS);
        }

        // Simplement pour être complet avec les vérifications de User.cs
        public bool IsAccountEnabled(User user) => user.IsEnabled;
        public bool IsAccountNonExpired(User user) => user.IsAccountNonExpired();
    }
}