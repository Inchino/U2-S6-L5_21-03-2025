using System.ComponentModel.DataAnnotations;

namespace GestionaleHotel.Models
{
    public class Prenotazione
    {
        public int PrenotazioneId { get; set; }

        [Required]
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        [Required]
        public int CameraId { get; set; }
        public Camera Camera { get; set; }

        [Required]
        public DateTime DataInizio { get; set; }

        [Required]
        public DateTime DataFine { get; set; }

        [Required]
        [StringLength(20)]
        public string Stato { get; set; }
    }
}
