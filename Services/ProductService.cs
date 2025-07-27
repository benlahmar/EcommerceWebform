// EcommerceWebform/Services/ProductService.cs

using System;
using System.Collections.Generic;
using EcommerceWebform.Models; // Pour le modèle Product
using EcommerceWebform.Dao;

namespace EcommerceWebform.Services
{
    public class ProductService
    {
        private readonly ProductDao _productDAO; // Dépendance au DAO

        // Constructeur pour l'injection de dépendances (ou instanciation directe si pas d'injection)
        public ProductService()
        {
            _productDAO = new ProductDao();
        }

        // Méthode pour obtenir tous les produits
        public List<Product> GetAllProducts()
        {
            return _productDAO.GetAllProducts();
        }

        // Méthode pour obtenir un produit par son ID
        public Product GetProductById(int id)
        {
            if (id <= 0)
            {
                // Exemple de validation métier : un ID doit être positif
                throw new ArgumentException("L'ID du produit doit être un nombre positif.");
            }
            return _productDAO.GetProductById(id);
        }

        // Méthode pour ajouter un nouveau produit
        public bool AddProduct(Product product)
        {
            // Exemple de validation métier :
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product), "Le produit ne peut pas être nul.");
            }
            if (string.IsNullOrWhiteSpace(product.Name))
            {
                // Vous pouvez avoir des messages d'erreur plus spécifiques ici
                // Ou utiliser les Data Annotations du modèle qui seront vérifiées par ASP.NET
                return false; // Ou lancer une exception
            }
            if (product.Price <= 0)
            {
                return false;
            }
            if (product.StockQuantity < 0)
            {
                return false;
            }

            // Ici, vous pourriez ajouter d'autres logiques métier, par exemple:
            // - Vérifier l'unicité du nom du produit
            // - Appliquer des règles de prix spécifiques
            // - Enregistrer un historique des modifications

            _productDAO.AddProduct(product);
            return true; // Indique le succès de l'opération
        }

        // Méthode pour mettre à jour un produit existant
        public bool UpdateProduct(Product product)
        {
            // Validation de base similaire à AddProduct
            if (product == null || product.Id <= 0 || string.IsNullOrWhiteSpace(product.Name) || product.Price <= 0 || product.StockQuantity < 0)
            {
                return false; // Ou lancer une exception avec un message détaillé
            }

            // Vous pourriez vérifier si le produit existe avant de tenter de le mettre à jour
            var existingProduct = _productDAO.GetProductById(product.Id);
            if (existingProduct == null)
            {
                return false; // Produit non trouvé
            }

            // Copier les propriétés modifiables pour éviter de modifier des propriétés non prévues
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.StockQuantity = product.StockQuantity;
            existingProduct.ImageUrl = product.ImageUrl;
            // LastUpdatedDate sera mis à jour dans le DAO

            _productDAO.UpdateProduct(existingProduct);
            return true;
        }

        // Méthode pour supprimer un produit
        public bool DeleteProduct(int id)
        {
            if (id <= 0)
            {
                return false; // ID invalide
            }

            var productToDelete = _productDAO.GetProductById(id);
            if (productToDelete == null)
            {
                return false; // Produit non trouvé
            }

            // Vous pourriez ajouter des règles métier ici, par exemple:
            // - Ne pas supprimer un produit s'il est dans des commandes actives
            // - Archiver le produit au lieu de le supprimer physiquement

            _productDAO.DeleteProduct(id);
            return true;
        }
    }
}