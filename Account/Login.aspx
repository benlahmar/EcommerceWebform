<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="EcommerceWebform.Account.Login" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Connexion</title>
    <link href="~/Content/bootstrap.min.css" rel="stylesheet" />
    <link href="~/Content/Site.css" rel="stylesheet" />
    <style>
        .form-container {
            max-width: 400px;
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
                <h2>Connexion</h2>
                <hr />

                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="alert alert-danger" EnableClientScript="true" HeaderText="Veuillez corriger les erreurs suivantes :" />

                <asp:Literal ID="litMessage" runat="server" EnableViewState="false" />

                <div class="form-group">
                    <label for="<%= txtUsername.ClientID %>">Nom d'utilisateur</label>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ControlToValidate="txtUsername" ErrorMessage="Le nom d'utilisateur est obligatoire." Display="Dynamic" CssClass="text-danger"></asp:RequiredFieldValidator>
                </div>

                <div class="form-group">
                    <label for="<%= txtPassword.ClientID %>">Mot de passe</label>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" MaxLength="100"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="Le mot de passe est obligatoire." Display="Dynamic" CssClass="text-danger"></asp:RequiredFieldValidator>
                </div>

                <div class="form-group">
                    <asp:CheckBox ID="chkRememberMe" runat="server" Text="Se souvenir de moi" />
                </div>

                <div class="form-group">
                    <asp:Button ID="btnLogin" runat="server" Text="Se connecter" CssClass="btn btn-primary" OnClick="btnLogin_Click" />
                </div>

                <div class="mt-3 text-center">
                    <p>Pas encore de compte ? <a href="Register.aspx">S'inscrire ici</a>.</p>
                     <p><a href="ForgotPassword.aspx">Mot de passe oublié ?</a></p>

                </div>

            </div>
        </div>
    </form>
    <script src="~/Scripts/jquery-3.7.1.min.js"></script>
    <script src="~/Scripts/bootstrap.bundle.min.js"></script>
</body>
</html>