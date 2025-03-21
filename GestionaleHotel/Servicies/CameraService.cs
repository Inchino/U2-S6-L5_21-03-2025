using GestionaleHotel.Data;
using GestionaleHotel.Models;
using GestionaleHotel.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace GestionaleHotel.Services
{
    public class CameraService : ICameraService
    {
        private readonly HotelDbContext _context;

        public CameraService(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CameraListViewModel>> GetAllCamereAsync()
        {
            return await _context.Camere
                .Select(c => new CameraListViewModel
                {
                    CameraId = c.CameraId,
                    Numero = c.Numero,
                    Tipo = c.Tipo,
                    Prezzo = c.Prezzo,
                    Disponibile = c.Disponibile
                })
                .ToListAsync();
        }

        public async Task<CameraDetailViewModel> GetCameraByIdAsync(int id)
        {
            var camera = await _context.Camere.FindAsync(id);
            if (camera == null) return null;

            return new CameraDetailViewModel
            {
                CameraId = camera.CameraId,
                Numero = camera.Numero,
                Tipo = camera.Tipo,
                Prezzo = camera.Prezzo,
                Disponibile = camera.Disponibile
            };
        }

        public async Task<bool> AddCameraAsync(CameraCreateViewModel model)
        {
            var camera = new Camera
            {
                Numero = model.Numero,
                Tipo = model.Tipo,
                Prezzo = model.Prezzo,
                Disponibile = model.Disponibile
            };

            _context.Camere.Add(camera);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateCameraAsync(CameraEditViewModel model)
        {
            var camera = await _context.Camere.FindAsync(model.CameraId);
            if (camera == null) return false;

            camera.Numero = model.Numero;
            camera.Tipo = model.Tipo;
            camera.Prezzo = model.Prezzo;
            camera.Disponibile = model.Disponibile;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteCameraAsync(int id)
        {
            var camera = await _context.Camere.FindAsync(id);
            if (camera == null) return false;

            _context.Camere.Remove(camera);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<CameraListViewModel>> GetCamereDisponibiliAsync()
        {
            return await _context.Camere
                .Where(c => c.Disponibile)
                .Select(c => new CameraListViewModel
                {
                    CameraId = c.CameraId,
                    Numero = c.Numero,
                    Tipo = c.Tipo,
                    Prezzo = c.Prezzo,
                    Disponibile = c.Disponibile
                })
                .ToListAsync();
        }
    }
}
