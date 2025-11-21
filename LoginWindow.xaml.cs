using System;
using System.Windows;
using MySql.Data.MySqlClient; // On utilise le connecteur Wamp

namespace MyMusicManager
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent(); // INDISPENSABLE : Charge le visuel
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string user = txtUsername.Text;
            string pass = txtPassword.Password;
            
            // Connexion à Wamp (root / pas de mot de passe)
            // On ajoute SslMode=None à la fin
            // Correction : database=mymusicmanager (tout attaché) et password=root
                    string connectionString = "server=localhost;user=root;password=root;database=mymusicmanager;SslMode=None";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    // Requête SQL Pure (Critère LOTJ)
                    string sql = "SELECT COUNT(*) FROM Utilisateurs WHERE Identifiant = @u AND MotDePasse = @p";
                    
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@u", user);
                        cmd.Parameters.AddWithValue("@p", pass);

                        long count = (long)cmd.ExecuteScalar();

                        if (count > 0)
                        {
                            // Succès ! On ouvre la fenêtre principale
                            MainWindow main = new MainWindow();
                            main.Show();
                            this.Close(); // On ferme le login
                        }
                        else
                        {
                            lblError.Text = "Identifiant ou mot de passe incorrect.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur Wamp : " + ex.Message);
            }
        }
    }
}