using EcommerceWebform.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EcommerceWebform.Account
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        private readonly UserService _userService;

        public ChangePassword()
        {
            _userService = new UserService();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Redirect if not authenticated (though Forms Authentication usually handles this)
            if (!Context.User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Account/Login.aspx"); // Redirect to login
                return;
            }

            // Display message if redirected from login due to expired password
            if (!IsPostBack && Request.QueryString["expired"] == "true")
            {
                litMessage.Text = "<div class='alert alert-warning' role='alert'>Votre mot de passe a expiré. Veuillez le changer pour continuer.</div>";
            }
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                // Ensure user is authenticated before attempting to change password
                if (!Context.User.Identity.IsAuthenticated)
                {
                    Response.Redirect("~/Account/Login.aspx");
                    return;
                }

                // Get current user's ID
                // In a real application, you would load the User object from the DB
                // using the authenticated username or ID from the FormsAuthenticationTicket/User.Identity.
                // For simplicity, we assume you have access to a User ID here.
                // You might need a helper method or retrieve the User object from your UserService
                // using Context.User.Identity.Name.
                var currentUser = _userService.GetUserByUsername(Context.User.Identity.Name); // Add this method to UserService if you don't have it

                if (currentUser == null)
                {
                    litMessage.Text = "<div class='alert alert-danger' role='alert'>Erreur: Utilisateur non trouvé.</div>";
                    return;
                }

                string oldPassword = txtOldPassword.Text;
                string newPassword = txtNewPassword.Text;
                string errorMessage;

                if (_userService.ChangePassword(currentUser.Id, oldPassword, newPassword, out errorMessage))
                {
                    
                    litMessage.Text = "<div class='alert alert-success' role='alert'>Mot de passe changé avec succès !</div>";
                   
                    txtOldPassword.Text = string.Empty;
                    txtNewPassword.Text = string.Empty;
                    txtConfirmNewPassword.Text = string.Empty;

                    
                }
                else
                {
                    
                    litMessage.Text = $"<div class='alert alert-danger' role='alert'>{errorMessage}</div>";
                    ValidationSummary1.Controls.Add(new LiteralControl(errorMessage));
                }
            }
        }
    }
}