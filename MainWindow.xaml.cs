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
            StatsWindow stats = new StatsWindow();
            stats.ShowDialog();
        }

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PdfDocument document = new PdfDocument();
                document.Info.Title = "Ma Collection Musicale";

                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);

                XFont fontTitre = new XFont("Arial", 24, XFontStyle.Bold);
                XFont fontNormal = new XFont("Arial", 12, XFontStyle.Regular);
                XFont fontGras = new XFont("Arial", 12, XFontStyle.Bold);
                
                XBrush brushTitre = XBrushes.Purple; 
                XBrush brushPrix = XBrushes.DarkMagenta; 

                gfx.DrawString("MY MUSIC MANAGER", fontTitre, brushTitre,
                    new XRect(0, 30, page.Width, page.Height), XStringFormats.TopCenter);
                
                gfx.DrawString("Catalogue Officiel", fontNormal, XBrushes.Gray,
                    new XRect(0, 60, page.Width, page.Height), XStringFormats.TopCenter);

                int y = 100;

                using (var context = new AppDbContext())
                {
                    var albums = context.Albums.Include(a => a.Artiste).ToList();

                    gfx.DrawString("ALBUM", fontGras, XBrushes.Black, 40, y);
                    gfx.DrawString("ARTISTE", fontGras, XBrushes.Black, 250, y);
                    gfx.DrawString("PRIX", fontGras, XBrushes.Black, 450, y);
                    
                    XPen penLigne = new XPen(XColors.Purple, 1);
                    gfx.DrawLine(penLigne, 40, y + 5, page.Width - 40, y + 5);
                    y += 30;

                    foreach (var album in albums)
                    {
                        gfx.DrawString(album.Titre, fontNormal, XBrushes.Black, 40, y);
                        string nomArtiste = album.Artiste != null ? album.Artiste.Nom : "Inconnu";
                        gfx.DrawString(nomArtiste, fontNormal, XBrushes.DarkSlateGray, 250, y);
                        gfx.DrawString(album.Prix + " €", fontGras, brushPrix, 450, y);
                        y += 25;
                    }
                }

                string filename = "MaCollection.pdf";
                document.Save(filename);
                Process.Start(new ProcessStartInfo(filename) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur PDF : " + ex.Message);
            }
        }

        // --- NOUVELLE FONCTION POUR SUPPRIMER ---
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            // 1. Récupérer l'album de la ligne cliquée
            var albumToDelete = ((FrameworkElement)sender).DataContext as Album;

            if (albumToDelete == null) return;

            // 2. Demander confirmation
            var result = MessageBox.Show($"Supprimer '{albumToDelete.Titre}' ?", 
                                         "Confirmation", 
                                         MessageBoxButton.YesNo, 
                                         MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try 
                {
                    using (var context = new AppDbContext())
                    {
                        // 3. Suppression en base
                        context.Albums.Remove(albumToDelete);
                        context.SaveChanges();
                    }
                    // 4. Mise à jour de l'affichage
                    ChargerAlbums();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Impossible de supprimer : " + ex.Message);
                }
            }
        }
    }
}