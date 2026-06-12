using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab2.RezervacijeProstora.Models
{
    public class ProstorDatoteka
    {
        [Key]
        public int Id { get; set; }

        public int ProstorZaProbuId { get; set; }

        [ForeignKey(nameof(ProstorZaProbuId))]
        public virtual ProstorZaProbu ProstorZaProbu { get; set; } = null!;

        [Required]
        [StringLength(255)]
        public string FileName { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string FilePath { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string ContentType { get; set; } = string.Empty;

        public long FileSize { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
