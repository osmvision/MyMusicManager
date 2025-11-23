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
                // 1. Création du document
                PdfDocument document = new PdfDocument();
                document.Info.Title = "Ma Collection Musicale";

                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);

                // --- POLICES (Version 1.50 compatible) ---
                XFont fontTitre = new XFont("Arial", 24, XFontStyle.Bold);
                XFont fontNormal = new XFont("Arial", 12, XFontStyle.Regular);
                XFont fontGras = new XFont("Arial", 12, XFontStyle.Bold);
                
                // --- COULEURS URBAINES ---
                // On remplace le Bleu par du Violet et du Noir pour rester lisible mais stylé
                XBrush brushTitre = XBrushes.Purple; 
                XBrush brushPrix = XBrushes.DarkMagenta; 

                // TITRE
                gfx.DrawString("MY MUSIC MANAGER", fontTitre, brushTitre,
                    new XRect(0, 30, page.Width, page.Height), XStringFormats.TopCenter);
                
                // SOUS-TITRE
                gfx.DrawString("Catalogue Officiel", fontNormal, XBrushes.Gray,
                    new XRect(0, 60, page.Width, page.Height), XStringFormats.TopCenter);

                int y = 100;

                using (var context = new AppDbContext())
                {
                    var albums = context.Albums.Include(a => a.Artiste).ToList();

                    // EN-TÊTES DU TABLEAU
                    gfx.DrawString("ALBUM", fontGras, XBrushes.Black, 40, y);
                    gfx.DrawString("ARTISTE", fontGras, XBrushes.Black, 250, y);
                    gfx.DrawString("PRIX", fontGras, XBrushes.Black, 450, y);
                    
                    // Ligne de séparation Violette
                    XPen penLigne = new XPen(XColors.Purple, 1);
                    gfx.DrawLine(penLigne, 40, y + 5, page.Width - 40, y + 5);
                    y += 30;

                    // LISTING DES ALBUMS
                    foreach (var album in albums)
                    {
                        // Titre
                        gfx.DrawString(album.Titre, fontNormal, XBrushes.Black, 40, y);

                        // Artiste
                        string nomArtiste = album.Artiste != null ? album.Artiste.Nom : "Inconnu";
                        gfx.DrawString(nomArtiste, fontNormal, XBrushes.DarkSlateGray, 250, y);

                        // Prix (En couleur pour ressortir)
                        gfx.DrawString(album.Prix + " €", fontGras, brushPrix, 450, y);

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