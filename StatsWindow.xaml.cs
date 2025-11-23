using System;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using LiveCharts; // Nécessaire pour le graphique
using LiveCharts.Wpf; // Nécessaire pour les séries

namespace MyMusicManager
{
    public partial class StatsWindow : Window
    {
        public StatsWindow()
        {
            InitializeComponent();
            ChargerDonnees();
        }

        private void ChargerDonnees()
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    // 1. On récupère les albums
                    var albums = context.Albums.Include(a => a.Artiste).ToList();

                    // 2. On groupe par nom d'artiste et on compte
                    // Ex: Daft Punk -> 2 albums, Ninho -> 1 album
                    var stats = albums
                        .GroupBy(a => a.Artiste != null ? a.Artiste.Nom : "Inconnu")
                        .Select(groupe => new 
                        { 
                            NomArtiste = groupe.Key, 
                            Nombre = groupe.Count() 
                        })
                        .ToList();

                    // 3. On crée la collection pour le graphique
                    SeriesCollection series = new SeriesCollection();

                    foreach (var item in stats)
                    {
                        series.Add(new PieSeries
                        {
                            Title = item.NomArtiste, // Le texte dans la légende
                            Values = new ChartValues<int> { item.Nombre }, // La valeur
                            DataLabels = true // Affiche le chiffre sur le camembert
                        });
                    }

                    // 4. On envoie les données au visuel
                    MyPieChart.Series = series;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur stats : " + ex.Message);
            }
        }
    }
}