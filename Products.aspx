<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="EcommerceWebform.Products" MasterPageFile="~/Site.Master" ValidateRequest="false" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

      <hr />

        <asp:Literal ID="Literal1" runat="server" EnableViewState="false" />

        <%-- AFFICHAGE DU TERME DE RECHERCHE  --%>
        <asp:Literal ID="litSearchTerm" runat="server" EnableViewState="false" />
        <%-- FIN AFFICHAGE DU TERME DE RECHERCHE --%>


        <div class="row">
            <%-- Utilisation d'un Repeater pour afficher les produits sous forme de cartes Bootstrap --%>
            <asp:Repeater ID="rptProducts" runat="server">
                <ItemTemplate>
                    <div class="col-md-4 mb-4">
                        <div class="card h-100 shadow-sm">
                            <img src='ProductImages/<%# Eval("ImageUrl") %>' class="card-img-top" alt='<%# Eval("Name") %>' style="height: 200px; object-fit: cover;">
                            <div class="card-body d-flex flex-column">
                                <h5 class="card-title"><%# Eval("Name") %></h5>
                                <p class="card-text flex-grow-1"><%# Eval("Description") %></p>
                                <p class="card-text">
                                    <strong>Prix: <%# string.Format("{0:C}", Eval("Price")) %></strong>
                                </p>
                                <p class="card-text">
                                    <small class="text-muted">Stock: <%# Eval("StockQuantity") %></small>
                                </p>
                                <%-- Bouton "Ajouter au panier" - Logique d'ajout au panier non implémentée ici mais le bouton est là --%>
                                <div class="mt-auto">
                                    <asp:Button ID="btnAddtoCart" runat="server" Text="Ajouter au panier" CommandName="AddToCart" CommandArgument='<%# Eval("Id") %>'
                                        CssClass="btn btn-primary w-100" />
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
                <FooterTemplate>
                    <%-- Ce contenu s'affiche après tous les éléments. Utile pour fermer des balises si nécessaire --%>
                    <%-- Ici, il n'y a rien de spécifique à faire --%>
                </FooterTemplate>
                <SeparatorTemplate>
                    <%-- Vide pour ne pas ajouter de séparateur entre les cartes --%>
                </SeparatorTemplate>
                <HeaderTemplate>
                    <%-- Ce contenu s'affiche avant le premier élément. Utile pour ouvrir des balises --%>
                </HeaderTemplate>
            </asp:Repeater>
        </div>

        <%# Eval("Name") %>
        <asp:Panel ID="pnlNoProducts" runat="server" Visible="false" CssClass="alert alert-info text-center mt-5">
            <h3>Aucun produit disponible pour le moment. Revenez bientôt !</h3>
            <p>Nous travaillons à ajouter de nouveaux articles passionnants.</p>
        </asp:Panel>
            <asp:Label ID="litMessage" runat="server" Text="Label"></asp:Label>
</asp:Content>
