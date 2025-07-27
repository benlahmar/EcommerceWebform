<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminProducts.aspx.cs" Inherits="EcommerceWebform.AdminProducts" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2>Gestion des Produits (Admin)</h2>
        <hr />

        <asp:Literal ID="litMessage" runat="server" EnableViewState="false" />

        <div class="mb-3">
            <a href="AddProduct.aspx" class="btn btn-success">
                <i class="fas fa-plus-circle"></i> Ajouter un nouveau produit
            </a>
        </div>

        <%-- Grille d'affichage des produits pour l'administration --%>
        <asp:GridView ID="gvAdminProducts" runat="server"
            AutoGenerateColumns="False"
            CssClass="table table-striped table-bordered table-hover"
            HeaderStyle-CssClass="thead-dark"
            EmptyDataText="Aucun produit n'est disponible pour le moment."
            AllowPaging="True"
            PageSize="10"
            OnPageIndexChanging="gvAdminProducts_PageIndexChanging"
            OnRowCommand="gvAdminProducts_RowCommand"
            DataKeyNames="Id"> <%-- Très important pour récupérer l'ID --%>
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="ID" ItemStyle-Width="50px" ItemStyle-CssClass="text-center" />
                <asp:BoundField DataField="Name" HeaderText="Nom" SortExpression="Name" />
                <asp:BoundField DataField="Price" HeaderText="Prix" DataFormatString="{0:C}" ItemStyle-CssClass="text-right" />
                <asp:BoundField DataField="StockQuantity" HeaderText="Stock" ItemStyle-CssClass="text-center" />
                <asp:TemplateField HeaderText="Image">
                    <ItemTemplate>
                        <asp:Image ID="imgProduct" runat="server" ImageUrl='ProductImages/<%# Eval("ImageUrl") %>' Width="75px" Height="75px" AlternateText='<%# Eval("Name") %>' CssClass="img-thumbnail" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CreatedDate" HeaderText="Créé le" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                <asp:BoundField DataField="LastUpdatedDate" HeaderText="Modifié le" DataFormatString="{0:dd/MM/yyyy HH:mm}" />

                <%-- Colonne des actions --%>
                <asp:TemplateField HeaderText="Actions" ItemStyle-Width="150px">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditProduct" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-sm btn-info me-2">
                            <i class="fas fa-edit"></i> Modifier
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnDelete" runat="server" CommandName="DeleteProduct" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-sm btn-danger" OnClientClick="return confirm('Êtes-vous sûr de vouloir supprimer ce produit ? Cette action est irréversible.');">
                            <i class="fas fa-trash-alt"></i> Supprimer
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerStyle CssClass="pagination-ys" HorizontalAlign="Center" />
            <PagerSettings Mode="NumericFirstLast" FirstPageText="Premier" LastPageText="Dernier" />
        </asp:GridView>
    </div>
</asp:Content>