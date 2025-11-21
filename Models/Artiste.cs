using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyMusicManager.Models
{
    public class Artiste
    {
        [Key]
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        
        // AJOUT DES ? ICI AUSSI
        public string? GenrePrincipal { get; set; }
        public string? Pays { get; set; }
        public string? Biographie { get; set; }

        public virtual ICollection<Album> Albums { get; set; } = new List<Album>();
    }
}