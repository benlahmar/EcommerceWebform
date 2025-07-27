using EcommerceWebform.Models;
using EcommerceWebform.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;


namespace EcommerceWebform
{
    public partial class AdminProducts : Page
    {
        private readonly ProductService _productService;
        public User us;
        public AdminProducts()
        {
            _productService = new ProductService();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            us = (User)Session["user"];

            if (us == null || !us.Role.Name.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                // Rediriger vers une page d'accès refusé ou la page d'accueil
                Response.Redirect("~/Default.aspx?msg=accessdenied");
                return; // Arrêter l'exécution de la page
            }

            if (!IsPostBack)
            {
                LoadAdminProducts();
            }
        }

        private void LoadAdminProducts()
        {
            try
            {
                List<Product> products = _productService.GetAllProducts();
                gvAdminProducts.DataSource = products;
                gvAdminProducts.DataBind();

                if (!products.Any())
                {
                    litMessage.Text = "<div class='alert alert-info' role='alert'>Aucun produit n'est enregistré dans le système.</div>";
                }
                else
                {
                    litMessage.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = $"<div class='alert alert-danger' role='alert'>Erreur lors du chargement des produits : {ex.Message}</div>";
                // En production, loggez l'exception complète (ex.ToString())
            }
        }

        protected void gvAdminProducts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAdminProducts.PageIndex = e.NewPageIndex;
            LoadAdminProducts(); // Recharge les données pour la nouvelle page
        }

        protected void gvAdminProducts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int productId;
            if (!int.TryParse(e.CommandArgument.ToString(), out productId))
            {
                litMessage.Text = "<div class='alert alert-danger'>ID de produit invalide.</div>";
                return;
            }

            if (e.CommandName == "EditProduct")
            {
                // Redirige vers la page de modification avec l'ID du produit
                Response.Redirect($"~/EditProduct.aspx?id={productId}");
            }
            else if (e.CommandName == "DeleteProduct")
            {
                try
                {
                    // Récupérer le produit pour obtenir l'URL de l'image avant de le supprimer de la DB
                    Product productToDelete = _productService.GetProductById(productId);

                    if (_productService.DeleteProduct(productId))
                    {
                        // Si la suppression en base de données est réussie, tenter de supprimer l'image physique
                        if (productToDelete != null && !string.IsNullOrEmpty(productToDelete.ImageUrl))
                        {
                            string imagePath = Server.MapPath(productToDelete.ImageUrl);
                            if (File.Exists(imagePath))
                            {
                                // Éviter de supprimer les images par défaut si vous en utilisez
                                if (!productToDelete.ImageUrl.Contains("placeholder.png") && !productToDelete.ImageUrl.Contains("default.png"))
                                {
                                    File.Delete(imagePath);
                                    litMessage.Text += "<br/><small>Image associée supprimée du serveur.</small>";
                                }
                            }
                        }

                        litMessage.Text = "<div class='alert alert-success' role='alert'>Produit supprimé avec succès.</div>";
                        LoadAdminProducts(); // Recharge la liste des produits après la suppression
                    }
                    else
                    {
                        litMessage.Text = "<div class='alert alert-warning' role='alert'>Échec de la suppression du produit. Il n'existe peut-être plus.</div>";
                    }
                }
                catch (Exception ex)
                {
                    litMessage.Text = $"<div class='alert alert-danger' role='alert'>Erreur lors de la suppression du produit : {ex.Message}</div>";
                    // En production, loggez l'exception complète
                }
            }
        }
    }
}