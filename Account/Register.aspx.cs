using EcommerceWebform.Models;
using EcommerceWebform.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EcommerceWebform.Account
{
    public partial class Register : System.Web.UI.Page
    {
        private readonly UserService _userService;

        public Register()
        {
            // Instanciez votre service ici
            _userService = new UserService();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Pas de logique spécifique au chargement de la page pour l'inscription initiale
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            // Important : Exécuter la validation côté serveur
            // Page.IsValid est VRAI si tous les validateurs ASP.NET côté client ET serveur sont passés.
            if (Page.IsValid)
            {
                // Créer un nouvel objet User à partir des données du formulaire
                var newUser = new User
                {
                    Username = txtUsername.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    // Le mot de passe en clair est assigné ici et sera haché par le UserService
                    Password = txtPassword.Text,
                    ConfirmPassword = txtConfirmPassword.Text // Utilisé par le CompareValidator côté client/serveur
                };

                string errorMessage;
                // Appeler le UserService pour enregistrer l'utilisateur
                if (_userService.RegisterUser(newUser, out errorMessage))
                {
                    // Inscription réussie !
                    litMessage.Text = "<div class='alert alert-success' role='alert'>Inscription réussie ! Vous pouvez maintenant vous <a href='Login.aspx'>connecter</a>.</div>";
                    // Optionnel : Réinitialiser les champs du formulaire après une inscription réussie
                    txtUsername.Text = string.Empty;
                    txtEmail.Text = string.Empty;
                    txtPassword.Text = string.Empty;
                    txtConfirmPassword.Text = string.Empty;
                }
                else
                {
                    // Échec de l'inscription : afficher le message d'erreur du service
                    // ValidationSummary affichera déjà les erreurs des validateurs.
                    // Ajouter le message du UserService au ValidationSummary.
                    ValidationSummary1.Controls.Add(new LiteralControl(errorMessage));
                    litMessage.Text = $"<div class='alert alert-danger' role='alert'>{errorMessage}</div>";

                    // Si vous voulez ajouter des erreurs spécifiques à des contrôles
                     //Validator.IsValid = false; // Pour marquer la page comme invalide
                    // Page.Validators.Add(new CustomValidator { IsValid = false, ErrorMessage = errorMessage });
                }
            }
            else
            {
                // La validation côté serveur (RequiredFieldValidator, RegularExpressionValidator, CompareValidator)
                // a échoué. ValidationSummary affichera déjà les erreurs.
                litMessage.Text = "<div class='alert alert-danger' role='alert'>Veuillez corriger les erreurs de saisie.</div>";
            }
        }
    }
}