<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditProduct.aspx.cs" Inherits="EcommerceWebform.EditProduct" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2>Modifier le produit</h2>
        <hr />

        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="alert alert-danger" EnableClientScript="true" HeaderText="Veuillez corriger les erreurs suivantes :" />
        <asp:Literal ID="litMessage" runat="server" EnableViewState="false" />
        
        <%-- Champ caché pour stocker l'ID du produit en cours de modification --%>
        <asp:HiddenField ID="hdnProductId" runat="server" Value="0" />
        <%-- Champ caché pour stocker le chemin de l'image existante --%>
        <asp:HiddenField ID="hdnExistingImageUrl" runat="server" Value="" />

        <div class="form-horizontal">
            <div class="form-group row mb-3">
                <asp:Label ID="lblName" runat="server" AssociatedControlID="txtName" CssClass="col-md-2 col-form-label">Nom du produit</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" ErrorMessage="Le nom est obligatoire." CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
            </div>

            <div class="form-group row mb-3">
                <asp:Label ID="lblDescription" runat="server" AssociatedControlID="txtDescription" CssClass="col-md-2 col-form-label">Description</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                </div>
            </div>

            <div class="form-group row mb-3">
                <asp:Label ID="lblPrice" runat="server" AssociatedControlID="txtPrice" CssClass="col-md-2 col-form-label">Prix (€)</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvPrice" runat="server" ControlToValidate="txtPrice" ErrorMessage="Le prix est obligatoire." CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="cvPrice" runat="server" ControlToValidate="txtPrice" Operator="GreaterThan" ValueToCompare="0" Type="Currency" ErrorMessage="Le prix doit être supérieur à zéro." CssClass="text-danger" Display="Dynamic"></asp:CompareValidator>
                </div>
            </div>

            <div class="form-group row mb-3">
                <asp:Label ID="lblStockQuantity" runat="server" AssociatedControlID="txtStockQuantity" CssClass="col-md-2 col-form-label">Quantité en stock</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox ID="txtStockQuantity" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvStockQuantity" runat="server" ControlToValidate="txtStockQuantity" ErrorMessage="La quantité en stock est obligatoire." CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="cvStockQuantity" runat="server" ControlToValidate="txtStockQuantity" Operator="GreaterThanEqual" ValueToCompare="0" Type="Integer" ErrorMessage="La quantité doit être égale ou supérieure à zéro." CssClass="text-danger" Display="Dynamic"></asp:CompareValidator>
                </div>
            </div>

            <div class="form-group row mb-3">
                <asp:Label ID="lblCurrentImage" runat="server" CssClass="col-md-2 col-form-label">Image actuelle</asp:Label>
                <div class="col-md-10">
                    <asp:Image ID="imgProduct" runat="server" Width="150px" Height="150px" AlternateText="Image du produit" CssClass="img-thumbnail mb-2" />
                    <asp:FileUpload ID="fuProductImage" runat="server" CssClass="form-control" />
                    <asp:RegularExpressionValidator ID="revProductImage" runat="server" ControlToValidate="fuProductImage"
                        ValidationExpression="^.*\.(jpg|JPG|jpeg|JPEG|png|PNG|gif|GIF)$"
                        ErrorMessage="Veuillez sélectionner un fichier image (jpg, jpeg, png, gif)." CssClass="text-danger" Display="Dynamic"></asp:RegularExpressionValidator>
                    <small class="form-text text-muted">Laissez vide pour conserver l'image actuelle. Téléchargez un nouveau fichier pour la remplacer.</small>
                </div>
            </div>

            <div class="form-group row mb-3">
                <div class="offset-md-2 col-md-10">
                    <asp:Button ID="btnSave" runat="server" Text="Enregistrer les modifications" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                    <a href="Products.aspx" class="btn btn-secondary">Annuler</a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>