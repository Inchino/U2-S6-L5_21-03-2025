using GestionaleHotel.Data;
using GestionaleHotel.Models;
using GestionaleHotel.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GestionaleHotel.Services
{
    public class PrenotazioneService : IPrenotazioneService
    {
        private readonly HotelDbContext _context;

        public PrenotazioneService(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PrenotazioneListViewModel>> GetAllPrenotazioniAsync()
        {
            return await _context.Prenotazioni
                .Include(p => p.Cliente)
                .Include(p => p.Camera)
                .Select(p => new PrenotazioneListViewModel
                {
                    PrenotazioneId = p.PrenotazioneId,
                    ClienteNomeCompleto = $"{p.Cliente.Nome} {p.Cliente.Cognome}",
                    CameraNumero = p.Camera.Numero,
                    DataInizio = p.DataInizio,
                    DataFine = p.DataFine,
                    Stato = p.Stato
                })
                .ToListAsync();
        }

        public async Task<PrenotazioneDetailViewModel> GetPrenotazioneByIdAsync(int id)
        {
            var prenotazione = await _context.Prenotazioni
                .Include(p => p.Cliente)
                .Include(p => p.Camera)
                .FirstOrDefaultAsync(p => p.PrenotazioneId == id);

            if (prenotazione == null) return null;

            return new PrenotazioneDetailViewModel
            {
                PrenotazioneId = prenotazione.PrenotazioneId,
                ClienteId = prenotazione.ClienteId,
                ClienteNomeCompleto = $"{prenotazione.Cliente.Nome} {prenotazione.Cliente.Cognome}",
                CameraId = prenotazione.CameraId,
                CameraNumero = prenotazione.Camera.Numero,
                DataInizio = prenotazione.DataInizio,
                DataFine = prenotazione.DataFine,
                Stato = prenotazione.Stato
            };
        }

        public async Task<bool> AddPrenotazioneAsync(PrenotazioneCreateViewModel model)
        {
            var camera = await _context.Camere.FindAsync(model.CameraId);
            if (camera == null || !camera.Disponibile)
                return false;

            var prenotazione = new Prenotazione
            {
                ClienteId = model.ClienteId,
                CameraId = model.CameraId,
                DataInizio = model.DataInizio,
                DataFine = model.DataFine,
                Stato = model.Stato
            };

            _context.Prenotazioni.Add(prenotazione);

            camera.Disponibile = false;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdatePrenotazioneAsync(PrenotazioneEditViewModel model)
        {
            var prenotazione = await _context.Prenotazioni.FindAsync(model.PrenotazioneId);
            if (prenotazione == null) return false;

            prenotazione.ClienteId = model.ClienteId;
            prenotazione.CameraId = model.CameraId;
            prenotazione.DataInizio = model.DataInizio;
            prenotazione.DataFine = model.DataFine;
            prenotazione.Stato = model.Stato;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeletePrenotazioneAsync(int id)
        {
            var prenotazione = await _context.Prenotazioni.FindAsync(id);
            if (prenotazione == null) return false;

            var camera = await _context.Camere.FindAsync(prenotazione.CameraId);
            if (camera != null)
            {
                camera.Disponibile = true;
            }

            _context.Prenotazioni.Remove(prenotazione);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
