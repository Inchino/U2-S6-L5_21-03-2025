using System.ComponentModel.DataAnnotations;

namespace GestionaleHotel.Models
{
    public class Cliente
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

        public ICollection<Prenotazione> Prenotazioni { get; set; }
    }
}
