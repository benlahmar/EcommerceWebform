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
    public partial class AddProduct : System.Web.UI.Page
    {
        private readonly ProductService _productService;

        public AddProduct()
        {
            _productService = new ProductService();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Peut-être vérifier ici l'autorisation de l'utilisateur (par session) si seul un admin peut ajouter
            // if (Session["CurrentUserRole"] == null || Session["CurrentUserRole"].ToString() != "Admin")
            // {
            //     Response.Redirect("~/Default.aspx"); // Ou une page d'accès refusé
            // }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string imageUrl = string.Empty; 
                string uploadDirectory = Server.MapPath("~/ProductImages/"); // Chemin physique du dossier ProductImages

                // S'assurer que le dossier d'upload existe
                if (!Directory.Exists(uploadDirectory))
                {
                    Directory.CreateDirectory(uploadDirectory);
                }

                // Vérifier si un fichier a été sélectionné et est valide
                if (fuProductImage.HasFile)
                {
                    try
                    {
                        // Valider le type de fichier (double vérification côté serveur)
                        string fileExtension = Path.GetExtension(fuProductImage.FileName).ToLower();
                        string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };

                        if (!allowedExtensions.Contains(fileExtension))
                        {
                            litMessage.Text = "<div class='alert alert-danger'>Seuls les fichiers JPG, JPEG, PNG et GIF sont autorisés.</div>";
                            return;
                        }

                        // Générer un nom de fichier unique pour éviter les conflits
                        string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                        string filePath = Path.Combine(uploadDirectory, uniqueFileName);

                        // Enregistrer l'image sur le serveur
                        fuProductImage.SaveAs(filePath);

                        // Stocker le chemin relatif de l'image dans la base de données
                        imageUrl =  uniqueFileName;
                    }
                    catch (Exception ex)
                    {
                        litMessage.Text = $"<div class='alert alert-danger'>Erreur lors de l'upload de l'image : {ex.Message}</div>";
                        // Log de l'exception complète en production
                        return; // Arrêter le processus si l'upload échoue
                    }
                }
                else
                {
                    // Si aucune image n'est fournie, utilisez une image par défaut ou laissez vide
                    imageUrl = "~/Content/p1.png";
                    // ou imageUrl = string.Empty;
                }

                Product newProduct = new Product
                {
                    Name = txtName.Text.Trim(),
                    Description = txtDescription.Text.Trim(),
                    Price = decimal.Parse(txtPrice.Text.Trim()),
                    StockQuantity = int.Parse(txtStockQuantity.Text.Trim()),
                    ImageUrl = imageUrl,
                    CreatedDate = DateTime.UtcNow // Sera mis à jour ou non par le DAO selon votre logique
                };

                try
                {
                    if (_productService.AddProduct(newProduct))
                    {
                        litMessage.Text = "<div class='alert alert-success'>Produit ajouté avec succès !</div>";
                        ClearForm(); // Efface le formulaire après succès
                    }
                    else
                    {
                        litMessage.Text = "<div class='alert alert-warning'>Échec de l'ajout du produit. Veuillez vérifier les données.</div>";
                    }
                }
                catch (Exception ex)
                {
                    litMessage.Text = $"<div class='alert alert-danger'>Erreur lors de l'enregistrement du produit : {ex.Message}</div>";
                    // Log de l'exception complète en production
                }
            }
        }

        private void ClearForm()
        {
            txtName.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtStockQuantity.Text = string.Empty;
            // fuProductImage ne peut pas être réinitialisé directement, il faudra que l'utilisateur le re-sélectionne
        }
    
}
}