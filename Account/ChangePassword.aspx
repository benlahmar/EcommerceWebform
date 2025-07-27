<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="EcommerceWebform.Account.ChangePassword" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Changer le mot de passe</title>
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
                <h2>Changer le mot de passe</h2>
                <hr />

                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="alert alert-danger" EnableClientScript="true" HeaderText="Veuillez corriger les erreurs suivantes :" />

                <asp:Literal ID="litMessage" runat="server" EnableViewState="false" />

                <div class="form-group">
                    <label for="<%= txtOldPassword.ClientID %>">Ancien mot de passe</label>
                    <asp:TextBox ID="txtOldPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvOldPassword" runat="server" ControlToValidate="txtOldPassword" ErrorMessage="L'ancien mot de passe est obligatoire." Display="Dynamic" CssClass="text-danger"></asp:RequiredFieldValidator>
                </div>

                <div class="form-group">
                    <label for="<%= txtNewPassword.ClientID %>">Nouveau mot de passe</label>
                    <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvNewPassword" runat="server" ControlToValidate="txtNewPassword" ErrorMessage="Le nouveau mot de passe est obligatoire." Display="Dynamic" CssClass="text-danger"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revNewPassword" runat="server" ControlToValidate="txtNewPassword" ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$" ErrorMessage="Le mot de passe doit avoir au moins 8 caractères, inclure une majuscule, une minuscule, un chiffre et un caractère spécial." Display="Dynamic" CssClass="text-danger"></asp:RegularExpressionValidator>
                </div>

                <div class="form-group">
                    <label for="<%= txtConfirmNewPassword.ClientID %>">Confirmer le nouveau mot de passe</label>
                    <asp:TextBox ID="txtConfirmNewPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvConfirmNewPassword" runat="server" ControlToValidate="txtConfirmNewPassword" ErrorMessage="La confirmation du nouveau mot de passe est obligatoire." Display="Dynamic" CssClass="text-danger"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="cmpNewPassword" runat="server" ControlToValidate="txtConfirmNewPassword" ControlToCompare="txtNewPassword" ErrorMessage="Le nouveau mot de passe et sa confirmation ne correspondent pas." Display="Dynamic" CssClass="text-danger"></asp:CompareValidator>
                </div>

                <div class="form-group">
                    <asp:Button ID="btnChangePassword" runat="server" Text="Changer le mot de passe" CssClass="btn btn-primary" OnClick="btnChangePassword_Click" />
                </div>
            </div>
        </div>
    </form>
    <script src="~/Scripts/jquery-3.7.1.min.js"></script>
    <script src="~/Scripts/bootstrap.bundle.min.js"></script>
</body>
</html>