using EcommerceWebform.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EcommerceWebform.Account
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        //private readonly UserService _userService; // Pour une future implémentation

        public ForgotPassword()
        {
            // _userService = new UserService(); // Pour une future implémentation
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Initialisation ou logique spécifique au chargement de la page
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string email = txtEmail.Text.Trim();

                // --- IMPLÉMENTATION FUTURE : Logique de réinitialisation de mot de passe ---
                // 1. Rechercher l'utilisateur par email.
                // 2. Générer un token de réinitialisation unique et à durée de vie limitée.
                // 3. Stocker le token haché et sa date d'expiration en base de données pour l'utilisateur.
                // 4. Envoyer un email à l'utilisateur avec un lien contenant le token.
                //    Le lien mènera à une page ResetPassword.aspx?token=XYZ

                litMessage.Text = "<div class='alert alert-success' role='alert'>Si votre adresse email est dans notre système, un lien de réinitialisation de mot de passe vous a été envoyé. Vérifiez votre boîte de réception (et vos spams).</div>";
                txtEmail.Text = string.Empty; // Effacer le champ après l'envoi
            }
            else
            {
                litMessage.Text = "<div class='alert alert-danger' role='alert'>Veuillez corriger les erreurs de saisie.</div>";
            }
        }
    }
}