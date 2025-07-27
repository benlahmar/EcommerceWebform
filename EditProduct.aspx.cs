using EcommerceWebform.Models;
using EcommerceWebform.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EcommerceWebform
{
    public partial class EditProduct : Page
    {
        private readonly ProductService _productService;

        public EditProduct()
        {
            _productService = new ProductService();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // --- Gérer l'autorisation basée sur la session ici ---
            // Si vous avez un système de rôles basé sur la session, assurez-vous que seul un Admin y accède.
            // Exemple :
            // if (Session["CurrentUserRole"] == null || Session["CurrentUserRole"].ToString() != "Admin")
            // {
            //     Response.Redirect("~/Default.aspx"); // Ou une page d'accès refusé
            // }

            if (!IsPostBack)
            {
                // Tenter de charger les données du produit si un ID est fourni dans l'URL
                if (Request.QueryString["id"] != null)
                {
                    int productId;
                    if (int.TryParse(Request.QueryString["id"], out productId))
                    {
                        hdnProductId.Value = productId.ToString(); // Stocke l'ID dans le champ caché
                        LoadProductData(productId);
                    }
                    else
                    {
                        litMessage.Text = "<div class='alert alert-danger'>ID de produit invalide.</div>";
                        // Vous pouvez rediriger l'utilisateur vers la liste des produits ou une page d'erreur
                        // Response.Redirect("~/Products.aspx");
                    }
                }
                else
                {
                    litMessage.Text = "<div class='alert alert-warning'>Aucun ID de produit spécifié pour la modification.</div>";
                    // Rediriger ou cacher le formulaire
                    // Response.Redirect("~/Products.aspx");
                }
            }
        }

        private void LoadProductData(int productId)
        {
            try
            {
                Product product = _productService.GetProductById(productId);

                if (product != null)
                {
                    txtName.Text = product.Name;
                    txtDescription.Text = product.Description;
                    txtPrice.Text = product.Price.ToString();
                    txtStockQuantity.Text = product.StockQuantity.ToString();

                    if (!string.IsNullOrEmpty(product.ImageUrl))
                    {
                        imgProduct.ImageUrl = product.ImageUrl;
                        imgProduct.Visible = true; // Assure que l'image est visible
                        hdnExistingImageUrl.Value = product.ImageUrl; // Stocke l'URL existante
                    }
                    else
                    {
                        imgProduct.Visible = false; // Cache l'image si aucune n'est présente
                        hdnExistingImageUrl.Value = string.Empty;
                    }
                }
                else
                {
                    litMessage.Text = "<div class='alert alert-danger'>Produit introuvable.</div>";
                    // Peut-être rediriger vers la liste des produits
                    // Response.Redirect("~/Products.aspx");
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = $"<div class='alert alert-danger'>Erreur lors du chargement des données du produit : {ex.Message}</div>";
                // En production, loggez l'exception complète
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                int productId;
                if (!int.TryParse(hdnProductId.Value, out productId) || productId <= 0)
                {
                    litMessage.Text = "<div class='alert alert-danger'>ID de produit invalide pour la mise à jour.</div>";
                    return;
                }

                // Récupérer le produit existant pour la mise à jour
                Product productToUpdate = _productService.GetProductById(productId);
                if (productToUpdate == null)
                {
                    litMessage.Text = "<div class='alert alert-danger'>Le produit que vous essayez de modifier n'existe plus.</div>";
                    return;
                }

                string newImageUrl = hdnExistingImageUrl.Value; // Commence avec l'image existante par défaut

                // Logique de gestion de l'upload d'image
                if (fuProductImage.HasFile)
                {
                    try
                    {
                        string uploadDirectory = Server.MapPath("~/ProductImages/");
                        if (!Directory.Exists(uploadDirectory))
                        {
                            Directory.CreateDirectory(uploadDirectory);
                        }

                        string fileExtension = Path.GetExtension(fuProductImage.FileName).ToLower();
                        string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };

                        if (!allowedExtensions.Contains(fileExtension))
                        {
                            litMessage.Text = "<div class='alert alert-danger'>Seuls les fichiers JPG, JPEG, PNG et GIF sont autorisés.</div>";
                            return;
                        }

                        // Générer un nom de fichier unique
                        string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                        string filePath = Path.Combine(uploadDirectory, uniqueFileName);

                        // Enregistrer la nouvelle image
                        fuProductImage.SaveAs(filePath);

                        // Mettre à jour le chemin de l'image
                        newImageUrl = uniqueFileName;

                        // OPTIONNEL : Supprimer l'ancienne image si elle existe et n'est pas une image par défaut
                        if (!string.IsNullOrEmpty(hdnExistingImageUrl.Value) &&
                            !hdnExistingImageUrl.Value.Contains("placeholder.png") && // Éviter de supprimer une image par défaut
                            File.Exists(Server.MapPath(hdnExistingImageUrl.Value)))
                        {
                            File.Delete(Server.MapPath(hdnExistingImageUrl.Value));
                        }
                    }
                    catch (Exception ex)
                    {
                        litMessage.Text = $"<div class='alert alert-danger'>Erreur lors de l'upload de l'image : {ex.Message}</div>";
                        // Log de l'exception complète en production
                        return;
                    }
                }
                // Si fuProductImage.HasFile est false, newImageUrl conserve la valeur de hdnExistingImageUrl.Value
                // Si l'utilisateur efface le champ de fichier, vous devriez avoir un bouton "Supprimer l'image" séparé
                // ou une logique pour gérer un "upload" vide comme suppression de l'image.
                // Pour l'instant, si aucune nouvelle image n'est choisie, l'ancienne est conservée.

                // Mettre à jour les propriétés du produit
                productToUpdate.Name = txtName.Text.Trim();
                productToUpdate.Description = txtDescription.Text.Trim();
                productToUpdate.Price = decimal.Parse(txtPrice.Text.Trim());
                productToUpdate.StockQuantity = int.Parse(txtStockQuantity.Text.Trim());
                productToUpdate.ImageUrl = newImageUrl;
                productToUpdate.LastUpdatedDate = DateTime.UtcNow; // Mettre à jour la date de modification

                try
                {
                    if (_productService.UpdateProduct(productToUpdate))
                    {
                        litMessage.Text = "<div class='alert alert-success'>Produit mis à jour avec succès !</div>";
                        // Optionnel : Recharger les données pour refléter les changements si l'utilisateur reste sur la page
                        LoadProductData(productId);
                    }
                    else
                    {
                        litMessage.Text = "<div class='alert alert-warning'>Échec de la mise à jour du produit. Vérifiez les données.</div>";
                    }
                }
                catch (Exception ex)
                {
                    litMessage.Text = $"<div class='alert alert-danger'>Erreur lors de l'enregistrement des modifications du produit : {ex.Message}</div>";
                    // Log de l'exception complète en production
                }
            }
        }
    }
}