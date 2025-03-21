using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace GestionaleHotel.ViewModels
{
    public class PrenotazioneCreateViewModel
    {
        [Required]
        public int ClienteId { get; set; }

        [Required]
        public int CameraId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DataInizio { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DataFine { get; set; }

        [Required]
        [StringLength(20)]
        public string Stato { get; set; }

        public SelectList Clienti { get; set; }
        public SelectList CamereDisponibili { get; set; }
    }
}
