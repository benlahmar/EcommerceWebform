
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddProduct.aspx.cs" Inherits="EcommerceWebform.AddProduct" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2>Ajouter un nouveau produit</h2>
        <hr />

        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="alert alert-danger" EnableClientScript="true" HeaderText="Veuillez corriger les erreurs suivantes :" />
        <asp:Literal ID="litMessage" runat="server" EnableViewState="false" />

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
                <asp:Label ID="lblImage" runat="server" AssociatedControlID="fuProductImage" CssClass="col-md-2 col-form-label">Image du produit</asp:Label>
                <div class="col-md-10">
                    <asp:FileUpload ID="fuProductImage" runat="server" CssClass="form-control" />
                    <asp:RegularExpressionValidator ID="revProductImage" runat="server" ControlToValidate="fuProductImage"
                        ValidationExpression="^.*\.(jpg|JPG|jpeg|JPEG|png|PNG|gif|GIF)$"
                        ErrorMessage="Veuillez sélectionner un fichier image (jpg, jpeg, png, gif)." CssClass="text-danger" Display="Dynamic"></asp:RegularExpressionValidator>
                </div>
            </div>

            <div class="form-group row mb-3">
                <div class="offset-md-2 col-md-10">
                    <asp:Button ID="btnSave" runat="server" Text="Ajouter le produit" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                    <a href="Products.aspx" class="btn btn-secondary">Annuler</a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>