<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="EcommerceWebform.Account.ForgotPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Mot de passe oublié</title>
    <link href="~/Content/bootstrap.min.css" rel="stylesheet" />
    <link href="~/Content/Site.css" rel="stylesheet" />
    <style>
        .form-container {
            max-width: 500px;
            margin: 50px auto;
            padding: 20px;
            border: 1px solid #ddd;
            border-radius: 5px;
            box-shadow: 0 0 10px rgba(0,0,0,0.1);
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="form-container">
                <h2>Réinitialisation de mot de passe</h2>
                <hr />
                <p>Veuillez entrer votre adresse email pour recevoir un lien de réinitialisation de mot de passe.</p>

                <%-- Pour l'instant, juste un champ email et un bouton --%>
                <div class="form-group">
                    <label for="<%= txtEmail.ClientID %>">Adresse Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="L'adresse email est obligatoire." Display="Dynamic" CssClass="text-danger"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ErrorMessage="Format d'email invalide." Display="Dynamic" CssClass="text-danger"></asp:RegularExpressionValidator>
                </div>

                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="alert alert-danger" EnableClientScript="true" HeaderText="Veuillez corriger les erreurs suivantes :" />
                <asp:Literal ID="litMessage" runat="server" EnableViewState="false" />

                <div class="form-group">
                    <asp:Button ID="btnSubmit" runat="server" Text="Envoyer le lien de réinitialisation" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                </div>

                <div class="mt-3 text-center">
                    <p><a href="Login.aspx">Retour à la connexion</a></p>
                </div>
            </div>
        </div>
    </form>
    <script src="~/Scripts/jquery-3.7.1.min.js"></script>
    <script src="~/Scripts/bootstrap.bundle.min.js"></script>
</body>
</html>