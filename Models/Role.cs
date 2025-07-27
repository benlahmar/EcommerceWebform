using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using System.ComponentModel.DataAnnotations;
using EcommerceWebform.Models;

namespace EcommerceWebform.Models
{
    public class Role
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom du rôle est obligatoire.")]
        [StringLength(50, ErrorMessage = "Le nom du rôle ne peut pas dépasser 50 caractères.")]
        public string Name { get; set; } = string.Empty;

        public virtual ICollection<User> Users { get; set; }
    }
}