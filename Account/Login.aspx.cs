using EcommerceWebform.Helpers;
using EcommerceWebform.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EcommerceWebform.Account
{
    public partial class Login : System.Web.UI.Page
    {
        private readonly UserService _userService;

        public Login()
        {
            _userService = new UserService();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // If user is already authenticated, redirect them
            if (Context.User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Default.aspx"); // Or any other protected page
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string username = txtUsername.Text.Trim();
                string password = txtPassword.Text;
                bool rememberMe = chkRememberMe.Checked;
                string errorMessage;

                var user = _userService.AuthenticateUser(username, password, out errorMessage);

                if (user != null)
                {
                    // Authentication successful
                    // Use FormsAuthentication to set the user's identity in the browser cookie
                    Session.Add("user", user);

                    // If password needs to be changed (e.g., first login, or expired)
                    if (!user.IsCredentialsNonExpired(90)) // Re-check policy
                    {
                        Response.Redirect("~/Account/ChangePassword.aspx?expired=true");
                        return;
                    }

                    // Redirect to the original URL the user tried to access, or to a default page
                    string returnUrl = Request.QueryString["ReturnUrl"];
                    if (!string.IsNullOrEmpty(returnUrl) && FormsAuthentication.IsEnabled)
                    {
                        // Ensure it's a local URL to prevent open redirection vulnerabilities
                        if (UrlHelper.IsLocalUrl(returnUrl)) // Requires a helper method or direct check
                        {
                            Response.Redirect(returnUrl);
                        }
                        else
                        {
                            Response.Redirect(FormsAuthentication.DefaultUrl);
                        }
                    }
                    else
                    {
                        Response.Redirect(FormsAuthentication.DefaultUrl); // Default.aspx or as configured in Web.config
                    }
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