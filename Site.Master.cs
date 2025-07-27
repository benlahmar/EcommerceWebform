using EcommerceWebform.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EcommerceWebform
{
    public partial class SiteMaster : MasterPage
    {
        public User us;
        protected void Page_Load(object sender, EventArgs e)
        {
             us = (User)Session["user"];

            
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text;

            string decodedSearchTerm = HttpUtility.UrlDecode(searchTerm); 

           
            LogSearchActivity(decodedSearchTerm); 

           
            Response.Redirect($"~/Products.aspx?q={searchTerm}"); // Keep searchTerm (encoded) for the URL
        }

        private void LogSearchActivity(string searchTermToLog) // Renamed parameter for clarity
        {
            string logFilePath = Server.MapPath("~/Logs/search_log.txt");

            string logDirectory = Path.GetDirectoryName(logFilePath);
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            // The vulnerability is here: searchTermToLog is now decoded and contains actual newlines.
            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Recherche effectuée par {Context.User.Identity.Name ?? "Anonyme"} : \"{searchTermToLog}\"";

            try
            {
                File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur d'écriture dans le log : {ex.Message}");
            }
        }
    }
}