namespace GestionaleHotel.ViewModels
{
    public class PrenotazioneDetailViewModel
    {
        public int PrenotazioneId { get; set; }
        public int ClienteId { get; set; }
        public string ClienteNomeCompleto { get; set; }
        public int CameraId { get; set; }
        public string CameraNumero { get; set; }
        public DateTime DataInizio { get; set; }
        public DateTime DataFine { get; set; }
        public string Stato { get; set; }
    }
}
