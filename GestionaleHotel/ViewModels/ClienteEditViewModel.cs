using System.ComponentModel.DataAnnotations;

namespace GestionaleHotel.ViewModels
{
    public class ClienteEditViewModel
    {
        public int ClienteId { get; set; }

        [Required]
        [StringLength(50)]
        public string Nome { get; set; }

        [Required]
        [StringLength(50)]
        public string Cognome { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Telefono { get; set; }
    }
}
