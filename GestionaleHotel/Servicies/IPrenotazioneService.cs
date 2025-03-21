using GestionaleHotel.ViewModels;

namespace GestionaleHotel.Services
{
    public interface IPrenotazioneService
    {
        Task<IEnumerable<PrenotazioneListViewModel>> GetAllPrenotazioniAsync();
        Task<PrenotazioneDetailViewModel> GetPrenotazioneByIdAsync(int id);
        Task<bool> AddPrenotazioneAsync(PrenotazioneCreateViewModel model);
        Task<bool> UpdatePrenotazioneAsync(PrenotazioneEditViewModel model);
        Task<bool> DeletePrenotazioneAsync(int id);
    }
}
