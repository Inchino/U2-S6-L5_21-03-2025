using System.ComponentModel.DataAnnotations;

namespace GestionaleHotel.Models
{
    public class Camera
    {
        public int CameraId { get; set; }

        [Required]
        [StringLength(10)]
        public string Numero { get; set; }

        [Required]
        [StringLength(20)]
        public string Tipo { get; set; }

        [Range(0, 10000)]
        public decimal Prezzo { get; set; }

        public bool Disponibile { get; set; } = true;

        public ICollection<Prenotazione> Prenotazioni { get; set; }
    }
}
