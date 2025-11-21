using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMusicManager.Models
{
    public class Album
    {
        [Key]
        public int Id { get; set; }
        public string Titre { get; set; } = string.Empty;
        public int Annee { get; set; }
        public string Format { get; set; } = "CD";
        public decimal Prix { get; set; }
        
        // AJOUT DU ? ICI (Important !)
        public string? UrlPochette { get; set; }

        public int ArtisteId { get; set; }
        
        [ForeignKey("ArtisteId")]
        public virtual Artiste? Artiste { get; set; }
    }
}