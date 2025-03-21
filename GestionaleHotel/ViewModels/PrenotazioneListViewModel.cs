namespace GestionaleHotel.ViewModels
{
    public class PrenotazioneListViewModel
    {
        public int PrenotazioneId { get; set; }
        public string ClienteNomeCompleto { get; set; }
        public string CameraNumero { get; set; }
        public DateTime DataInizio { get; set; }
        public DateTime DataFine { get; set; }
        public string Stato { get; set; }
    }
}
