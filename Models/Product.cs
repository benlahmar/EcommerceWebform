using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EcommerceWebform.Models
{
    [Table("Products")] // Mappe cette classe à une table nommée "Products" dans la base de données
    public class Product
    {
        [Key] 
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom du produit est obligatoire.")]
        [StringLength(200, ErrorMessage = "Le nom du produit ne peut pas dépasser 200 caractères.")]
        public string Name { get; set; }

        [StringLength(1000, ErrorMessage = "La description ne peut pas dépasser 1000 caractères.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Le prix est obligatoire.")]
        [Range(0.01, 1000000.00, ErrorMessage = "Le prix doit être compris entre 0.01 et 1 000 000.")]
       
        public decimal Price { get; set; }

        [Required(ErrorMessage = "La quantité en stock est obligatoire.")]
        [Range(0, 100000, ErrorMessage = "La quantité en stock doit être comprise entre 0 et 100 000.")]
        public int StockQuantity { get; set; }

        [StringLength(255, ErrorMessage = "Le chemin de l'image ne peut pas dépasser 255 caractères.")]
        public string ImageUrl { get; set; }

        // Date de création du produit
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // Initialise à la date/heure actuelle UTC par défaut

        // Date de la dernière mise à jour
        public DateTime? LastUpdatedDate { get; set; } // Nullable, car il peut ne pas être mis à jour immédiatement
    }
}