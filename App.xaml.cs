using System.Windows;

namespace MyMusicManager
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // On force un message dès le lancement
            MessageBox.Show("L'application démarre !");
            base.OnStartup(e);
        }
    }
}