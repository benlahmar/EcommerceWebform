// EcommerceWebform/DAO/ProductDAO.cs

using System;
using System.Collections.Generic;
using System.Linq;
using EcommerceWebform.Models; // N'oubliez pas d'inclure le namespace de votre modèle Product
using System.Data.Entity; // Nécessaire pour les opérations comme Include, Entry
using EcommerceWebform.Data;

namespace EcommerceWebform.Dao
{
    public class ProductDao
    {
        // Récupère tous les produits
        public List<Product> GetAllProducts()
        {
            using (var context = new EcommerceDbContext())
            {
                // Incluez les propriétés de navigation si le Product en avait (ex: Category)
                // return context.Products.Include(p => p.Category).ToList();
                return context.Products.ToList();
            }
        }

        // Récupère un produit par son ID
        public Product GetProductById(int id)
        {
            using (var context = new EcommerceDbContext())
            {
                // Find() est efficace pour la recherche par clé primaire
                return context.Products.Find(id);
                // Ou si vous avez besoin d'inclure des propriétés de navigation:
                // return context.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
            }
        }

        // Ajoute un nouveau produit
        public void AddProduct(Product product)
        {
            using (var context = new EcommerceDbContext())
            {
                // Assurez-vous que la date de création est définie si elle n'a pas été initialisée
                if (product.CreatedDate == default(DateTime))
                {
                    product.CreatedDate = DateTime.UtcNow;
                }
                context.Products.Add(product);
                context.SaveChanges();
            }
        }

        // Met à jour un produit existant
        public void UpdateProduct(Product product)
        {
            using (var context = new EcommerceDbContext())
            {
                // Attache l'entité au contexte et la marque comme modifiée
                context.Entry(product).State = EntityState.Modified;
                // Met à jour la date de dernière modification
                product.LastUpdatedDate = DateTime.UtcNow;

                context.SaveChanges();
            }
        }

        // Supprime un produit par son ID
        public void DeleteProduct(int id)
        {
            using (var context = new EcommerceDbContext())
            {
                var productToDelete = context.Products.Find(id);
                if (productToDelete != null)
                {
                    context.Products.Remove(productToDelete);
                    context.SaveChanges();
                }
            }
        }
    }
}