using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcommerceWebform.Helpers
{
    public class UrlHelper
    {
        /// <summary>
        /// Détermine si l'URL spécifiée est une URL locale (dans le même domaine et chemin relatif).
        /// Ceci est crucial pour prévenir les attaques de redirection ouverte.
        /// </summary>
        /// <param name="url">L'URL à vérifier.</param>
        /// <returns>True si l'URL est locale, False sinon.</returns>
        public static bool IsLocalUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }

            // Pour une URL absolue, vérifier qu'elle pointe vers le même hôte que la requête actuelle
            // et que le chemin commence par la racine de l'application.
            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                Uri uri = new Uri(url);
                // Vérifier que l'hôte de l'URL correspond à l'hôte de la requête
                // et que le schéma (http/https) correspond.
                if (uri.Host.Equals(HttpContext.Current.Request.Url.Host, StringComparison.OrdinalIgnoreCase) &&
                    uri.Scheme.Equals(HttpContext.Current.Request.Url.Scheme, StringComparison.OrdinalIgnoreCase))
                {
                    // Optionnel: Vérifier que le port correspond également si pertinent pour votre environnement
                    if (uri.Port != HttpContext.Current.Request.Url.Port)
                    {
                        return false;
                    }
                    // C'est une URL absolue vers le même domaine. Considérer comme locale.
                    return true;
                }
                return false; // Absolue mais vers un hôte différent
            }
            else
            {
                // URL relative (commence par /, ~/, ../ ou nom de fichier)
                // C'est le cas le plus courant et le plus sûr.
                // HttpContext.Current.Request.IsLocal est pour les requêtes de l'hôte local, pas les URL locales.
                // Il suffit de vérifier qu'elle commence par '/' et n'est pas '//' ou '\' (chemin UNC)
                return url.StartsWith("/") && !url.StartsWith("//") && !url.StartsWith("/\\");
            }
        }
    }
}