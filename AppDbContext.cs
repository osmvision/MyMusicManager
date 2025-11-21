using Microsoft.EntityFrameworkCore;
using MyMusicManager.Models;

namespace MyMusicManager // <--- C'est ça la clé ! Il faut que ce soit le même nom partout.
{
    public class AppDbContext : DbContext
    {
        public DbSet<Artiste> Artistes { get; set; }
        public DbSet<Album> Albums { get; set; }

       protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    // On ajoute SslMode=None ici aussi
    // La même correction ici
        string connectionString = "server=localhost;user=root;password=root;database=mymusicmanager;SslMode=None";      
    
    optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
}
    }
}