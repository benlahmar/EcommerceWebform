<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="EcommerceWebform.Account.Register" %>

<!DOCTYPE html>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Inscription</title>
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
        .form-group {
            margin-bottom: 15px;
        }
        .btn-primary {
            width: 100%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="form-container">
                <h2>Inscription</h2>
                <hr />

                <%-- ValidationSummary affichera toutes les erreurs de validation et messages d'erreur du code-behind --%>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="alert alert-danger" EnableClientScript="true" HeaderText="Veuillez corriger les erreurs suivantes :" />

                <%-- Message de succès global (pour affichage après inscription réussie, par exemple) --%>
                <asp:Literal ID="litMessage" runat="server" EnableViewState="false" />

                <div class="form-group">
                    <label for="<%= txtUsername.ClientID %>">Nom d'utilisateur</label>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ControlToValidate="txtUsername" ErrorMessage="Le nom d'utilisateur est obligatoire." Display="Dynamic" CssClass="text-danger"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revUsername" runat="server" ControlToValidate="txtUsername" ValidationExpression="^[a-zA-Z0-9_]{3,50}$" ErrorMessage="Le nom d'utilisateur doit contenir entre 3 et 50 caractères alphanumériques ou underscore." Display="Dynamic" CssClass="text-danger"></asp:RegularExpressionValidator>
                </div>

                <div class="form-group">
                    <label for="<%= txtEmail.ClientID %>">Adresse Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" MaxLength="100"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="L'adresse email est obligatoire." Display="Dynamic" CssClass="text-danger"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ErrorMessage="Format d'email invalide." Display="Dynamic" CssClass="text-danger"></asp:RegularExpressionValidator>
                </div>

                <div class="form-group">
                    <label for="<%= txtPassword.ClientID %>">Mot de passe</label>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" MaxLength="100"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="Le mot de passe est obligatoire." Display="Dynamic" CssClass="text-danger"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revPassword" runat="server" ControlToValidate="txtPassword" ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$" ErrorMessage="Le mot de passe doit avoir au moins 8 caractères, inclure une majuscule, une minuscule, un chiffre et un caractère spécial." Display="Dynamic" CssClass="text-danger"></asp:RegularExpressionValidator>
                </div>

                <div class="form-group">
                    <label for="<%= txtConfirmPassword.ClientID %>">Confirmer le mot de passe</label>
                    <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" MaxLength="100"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" ControlToValidate="txtConfirmPassword" ErrorMessage="La confirmation du mot de passe est obligatoire." Display="Dynamic" CssClass="text-danger"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="cmpPassword" runat="server" ControlToValidate="txtConfirmPassword" ControlToCompare="txtPassword" ErrorMessage="Le mot de passe et la confirmation ne correspondent pas." Display="Dynamic" CssClass="text-danger"></asp:CompareValidator>
                </div>

                <div class="form-group">
                    <asp:Button ID="btnRegister" runat="server" Text="S'inscrire" CssClass="btn btn-primary" OnClick="btnRegister_Click" />
                </div>

                <div class="mt-3 text-center">
                    <p>Déjà un compte ? <a href="Login.aspx">Se connecter ici</a>.</p>
                </div>
            </div>
        </div>
    </form>
    <script src="~/Scripts/jquery-3.7.1.min.js"></script>
    <script src="~/Scripts/bootstrap.bundle.min.js"></script>
</body>
</html>
