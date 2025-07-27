using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema; // Pour [NotMapped]

namespace EcommerceWebform.Models
{


    public class User
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom d'utilisateur est obligatoire.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Le nom d'utilisateur doit contenir entre 3 et 50 caractères.")]
        [Display(Name = "Nom d'utilisateur")]
        public string Username { get; set; } = string.Empty;


        [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
        [MaxLength(256)]
        [Display(Name = "Hash du mot de passe")]
        public string PasswordHash { get; set; } = string.Empty;


        [MaxLength(128)]
        [Display(Name = "Salt du mot de passe")]
        public string PasswordSalt { get; set; } // Nullable si l'algorithme de hachage l'intègre

        [Required(ErrorMessage = "L'adresse email est obligatoire.")]
        [EmailAddress(ErrorMessage = "Format d'email invalide.")]
        [StringLength(100, ErrorMessage = "L'email ne peut pas dépasser 100 caractères.")]
        [Display(Name = "Adresse email")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Compte activé")]
        public bool IsEnabled { get; set; } = true;

        [Display(Name = "Dernière connexion")]
        public DateTime? LastLoginDate { get; set; }


        [Display(Name = "Date de changement du mot de passe")]
        public DateTime? PasswordLastChangedDate { get; set; } = DateTime.UtcNow; // Initialiser à la création

        // Compteur d'échecs de connexion : pour le verrouillage de compte.
        [Display(Name = "Tentatives de connexion échouées")]
        public int AccessFailedCount { get; set; } = 0;

        // Date et heure de fin de verrouillage. Null si non verrouillé.
        [Display(Name = "Fin du verrouillage")]
        public DateTime? LockoutEnd { get; set; }

        // Clé étrangère vers le rôle de l'utilisateur
        public int RoleId { get; set; }

        // Propriété de navigation pour la relation avec le rôle
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; } // Rend le rôle nullable pour la création initiale

        [NotMapped]
        [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Le mot de passe doit contenir au moins {2} caractères.")]
        [DataType(DataType.Password)] // Pour les contrôles UI (Web Forms) et la sécurité d'entrée
        [Display(Name = "Mot de passe")]
        public string Password { get; set; } = string.Empty;

        [NotMapped]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Le mot de passe et la confirmation ne correspondent pas.")]
        [Display(Name = "Confirmer le mot de passe")]
        public string ConfirmPassword { get; set; } = string.Empty;

        

        public bool IsAccountEnabled() => IsEnabled;
        public bool IsAccountNonLocked() => LockoutEnd == null || LockoutEnd <= DateTime.UtcNow; 
        public bool IsCredentialsNonExpired(int passwordExpirationDays)
        {
            if (PasswordLastChangedDate == null) return true; 
            return PasswordLastChangedDate.Value.AddDays(passwordExpirationDays) > DateTime.UtcNow;
        }
        public bool IsAccountNonExpired() => true; 
    }
}