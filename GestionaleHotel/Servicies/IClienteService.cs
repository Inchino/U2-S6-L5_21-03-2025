using GestionaleHotel.ViewModels;

namespace GestionaleHotel.Services
{
    public interface IClienteService
    {
        Task<IEnumerable<ClienteListViewModel>> GetAllClientiAsync();
        Task<ClienteDetailViewModel> GetClienteByIdAsync(int id);
        Task<bool> AddClienteAsync(ClienteCreateViewModel model);
        Task<bool> UpdateClienteAsync(ClienteEditViewModel model);
        Task<bool> DeleteClienteAsync(int id);
    }
}
