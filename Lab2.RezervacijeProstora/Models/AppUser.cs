using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Lab2.RezervacijeProstora.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        [StringLength(11, MinimumLength = 11)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "OIB smije sadrzavati samo brojeve.")]
        public string OIB { get; set; } = string.Empty;

        [Required]
        [StringLength(13, MinimumLength = 13)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "JMBG smije sadrzavati samo brojeve.")]
        public string JMBG { get; set; } = string.Empty;
    }
}
