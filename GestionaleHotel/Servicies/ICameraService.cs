using GestionaleHotel.ViewModels;

namespace GestionaleHotel.Services
{
    public interface ICameraService
    {
        Task<IEnumerable<CameraListViewModel>> GetAllCamereAsync();
        Task<CameraDetailViewModel> GetCameraByIdAsync(int id);
        Task<bool> AddCameraAsync(CameraCreateViewModel model);
        Task<bool> UpdateCameraAsync(CameraEditViewModel model);
        Task<bool> DeleteCameraAsync(int id);
        Task<IEnumerable<CameraListViewModel>> GetCamereDisponibiliAsync();
    }
}
