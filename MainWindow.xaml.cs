using System;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using MyMusicManager.Models;

// Usings pour PdfSharp 1.50
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.Diagnostics;

namespace MyMusicManager
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ChargerAlbums();
        }

        private void ChargerAlbums()
        {
            using (var context = new AppDbContext())
            {
                var liste = context.Albums.Include(a => a.Artiste).ToList();
                GridAlbums.ItemsSource = liste;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddAlbumWindow addWindow = new AddAlbumWindow();
            addWindow.ShowDialog();
            ChargerAlbums();
        }

        private void BtnStats_Click(object sender, RoutedEventArgs e)
        {
            // On ouvre la fenêtre des graphiques
            StatsWindow stats = new StatsWindow();
            stats.ShowDialog();
        }
        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 1. Création du document (Version 1.50)
                PdfDocument document = new PdfDocument();
                document.Info.Title = "Ma Collection Musicale";

                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);

                // --- CODE SIMPLIFIÉ POUR VERSION 1.50 ---
                // Ici, on peut utiliser des entiers (20) et XFontStyle marche directement !
                
                XFont fontTitre = new XFont("Arial", 20, XFontStyle.Bold);
                XFont fontNormal = new XFont("Arial", 12, XFontStyle.Regular);
                XFont fontGras = new XFont("Arial", 12, XFontStyle.Bold);
                // ----------------------------------------

                // Titre
                gfx.DrawString("Ma Collection Musicale", fontTitre, XBrushes.DarkBlue,
                    new XRect(0, 20, page.Width, page.Height), XStringFormats.TopCenter);

                int y = 80;

                using (var context = new AppDbContext())
                {
                    var albums = context.Albums.Include(a => a.Artiste).ToList();

                    // En-têtes
                    gfx.DrawString("Titre", fontGras, XBrushes.Black, 40, y);
                    gfx.DrawString("Artiste", fontGras, XBrushes.Black, 250, y);
                    gfx.DrawString("Prix", fontGras, XBrushes.Black, 450, y);
                    
                    // Ligne
                    gfx.DrawLine(XPens.Black, 40, y + 5, page.Width - 40, y + 5);
                    y += 30;

                    foreach (var album in albums)
                    {
                        gfx.DrawString(album.Titre, fontNormal, XBrushes.Black, 40, y);

                        string nomArtiste = album.Artiste != null ? album.Artiste.Nom : "Inconnu";
                        gfx.DrawString(nomArtiste, fontNormal, XBrushes.Black, 250, y);

                        gfx.DrawString(album.Prix + " €", fontNormal, XBrushes.Black, 450, y);

                        y += 25;
                    }
                }

                string filename = "MaCollection.pdf";
                document.Save(filename);

                // Ouverture du PDF
                Process.Start(new ProcessStartInfo(filename) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur PDF : " + ex.Message);
            }
        }
    }
}