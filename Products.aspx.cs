using EcommerceWebform.Models;
using EcommerceWebform.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Linq;


namespace EcommerceWebform
{
    public partial class Products : System.Web.UI.Page
    {
     
            private readonly ProductService _productService;

            public Products()
            {
                _productService = new ProductService();
            }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string searchTerm = Request.QueryString["q"];

                if (!string.IsNullOrEmpty(searchTerm))
                {

                    litSearchTerm.Text = $"<div class='alert alert-warning'>Résultats de recherche pour : <strong>{searchTerm}</strong></div>";


                    LoadProducts();
                }
                else
                {
                    LoadProducts();
                }
            }
        }

            private void LoadProducts()
            {
                try
                {
                    List<Product> products = _productService.GetAllProducts();

                    if (products != null && products.Any())
                    {
                        rptProducts.DataSource = products;
                        rptProducts.DataBind();
                        pnlNoProducts.Visible = false; // Cache le message "aucun produit"
                    }
                    else
                    {
                        rptProducts.DataSource = null; // S'assurer que le repeater est vide
                        rptProducts.DataBind();
                        pnlNoProducts.Visible = true; // Affiche le message "aucun produit"
                        litMessage.Text = string.Empty; // Efface les messages précédents s'il y en a
                    }
                }
                catch (Exception ex)
                {
                    // En cas d'erreur lors du chargement des produits (ex: DB non accessible)
                    litMessage.Text = $"<div class='alert alert-danger' role='alert'>Erreur lors du chargement des produits : {ex.Message}</div>";
                    pnlNoProducts.Visible = true; // Afficher le panneau vide si erreur pour éviter un affichage étrange
                    rptProducts.DataSource = null;
                    rptProducts.DataBind();
                    // En production, vous devriez logger l'exception complète (ex.ToString())
                }
            }

            // --- Pour le bouton "Ajouter au panier" (implémentation future) ---
            protected void rptProducts_ItemCommand(object source, RepeaterCommandEventArgs e)
            {
                if (e.CommandName == "AddToCart")
                {
                    int productId = Convert.ToInt32(e.CommandArgument);
                    // Ici, vous ajouteriez la logique pour ajouter le produit au panier
                    // Cela impliquerait généralement:
                    // 1. Récupérer le produit par son ID (via _productService.GetProductById(productId))
                    // 2. Gérer le panier de l'utilisateur (souvent stocké en session ou en DB)
                    // 3. Ajouter le produit (et la quantité) au panier
                    // 4. Afficher un message de succès (ex: "Produit ajouté au panier !")

                    litMessage.Text = $"<div class='alert alert-success' role='alert'>Le produit avec l'ID {productId} a été ajouté au panier (fonctionnalité en cours de développement).</div>";
                }
            }
        }
    }