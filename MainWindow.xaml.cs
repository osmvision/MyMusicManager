using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore; // Nécessaire pour .Include()
using MyMusicManager.Models;

namespace MyMusicManager
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent(); // Charge le XAML ci-dessus
            ChargerAlbums();       // Récupère les données
        }

        private void ChargerAlbums()
        {
            using (var context = new AppDbContext())
            {
                // Récupère les albums et les artistes associés
                var listeAlbums = context.Albums
                                         .Include(a => a.Artiste)
                                         .ToList();

                // Remplit la grille
                GridAlbums.ItemsSource = listeAlbums;
            }
        }

        // Les actions des boutons (Vides pour l'instant)
        private void BtnAdd_Click(object sender, RoutedEventArgs e) 
        { 
            // 1. On ouvre la fenêtre d'ajout
            AddAlbumWindow addWindow = new AddAlbumWindow();
            addWindow.ShowDialog(); // ShowDialog bloque la fenêtre principale tant qu'on n'a pas fini

            // 2. Quand on revient, on rafraîchit la liste pour voir le nouvel album
            ChargerAlbums();
        }
        private void BtnStats_Click(object sender, RoutedEventArgs e) 
        { 
            MessageBox.Show("Graphiques à venir !"); 
        }

        private void BtnExport_Click(object sender, RoutedEventArgs e) 
        { 
            MessageBox.Show("PDF à venir !"); 
        }
    }
}