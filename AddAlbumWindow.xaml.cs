using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using MyMusicManager.Models;
using System.Linq;

namespace MyMusicManager
{
    public partial class AddAlbumWindow : Window
    {
        public AddAlbumWindow()
        {
            InitializeComponent();
        }

        private async void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text)) return;

            try
            {
                string url = $"https://itunes.apple.com/search?term={txtSearch.Text}&entity=album&limit=10";
                using (HttpClient client = new HttpClient())
                {
                    string json = await client.GetStringAsync(url);
                    var result = JsonConvert.DeserializeObject<ItunesResult>(json);
                    ListResults.ItemsSource = result?.Results;
                }
            }
            catch (Exception ex) { MessageBox.Show("Erreur : " + ex.Message); }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var selectedAlbum = (ItunesAlbum)btn.Tag;

            // Sécurisation anti-crash
            if (selectedAlbum == null || selectedAlbum.ArtistName == null || selectedAlbum.CollectionName == null) return;

            using (var context = new AppDbContext())
            {
                var artiste = context.Artistes.FirstOrDefault(a => a.Nom == selectedAlbum.ArtistName);
                if (artiste == null)
                {
                    artiste = new Artiste { Nom = selectedAlbum.ArtistName };
                    context.Artistes.Add(artiste);
                    context.SaveChanges();
                }

                int annee = 2000;
                if (selectedAlbum.ReleaseDate != null && selectedAlbum.ReleaseDate.Length >= 4)
                    int.TryParse(selectedAlbum.ReleaseDate.Substring(0, 4), out annee);

                var album = new Album
                {
                    Titre = selectedAlbum.CollectionName,
                    Annee = annee,
                    Prix = (decimal)selectedAlbum.CollectionPrice,
                    UrlPochette = selectedAlbum.ArtworkUrl100,
                    ArtisteId = artiste.Id,
                    Format = "Digital"
                };

                context.Albums.Add(album);
                context.SaveChanges();
                MessageBox.Show("Album ajouté !");
                this.Close();
            }
        }

        // C'EST CETTE FONCTION QUI MANQUAIT !
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    // Classes de lecture JSON
    public class ItunesResult { public List<ItunesAlbum>? Results { get; set; } }
    public class ItunesAlbum
    {
        public string? ArtistName { get; set; }
        public string? CollectionName { get; set; }
        public string? ArtworkUrl100 { get; set; }
        public string? ReleaseDate { get; set; }
        public double CollectionPrice { get; set; }
    }
}