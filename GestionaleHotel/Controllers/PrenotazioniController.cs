using GestionaleHotel.Services;
using GestionaleHotel.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GestionaleHotel.Controllers
{
    public class PrenotazioniController : Controller
    {
        private readonly IPrenotazioneService _prenotazioneService;
        private readonly IClienteService _clienteService;
        private readonly ICameraService _cameraService;

        public PrenotazioniController(IPrenotazioneService prenotazioneService, IClienteService clienteService, ICameraService cameraService)
        {
            _prenotazioneService = prenotazioneService;
            _clienteService = clienteService;
            _cameraService = cameraService;
        }

        public async Task<IActionResult> Index()
        {
            var prenotazioni = await _prenotazioneService.GetAllPrenotazioniAsync();
            return View(prenotazioni);
        }

        public async Task<IActionResult> Create()
        {
            var clienti = await _clienteService.GetAllClientiAsync();
            var camereDisponibili = await _cameraService.GetCamereDisponibiliAsync();

            var model = new PrenotazioneCreateViewModel
            {
                Clienti = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(clienti, "ClienteId", "NomeCompleto"),
                CamereDisponibili = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(camereDisponibili, "CameraId", "Numero")
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PrenotazioneCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _prenotazioneService.AddPrenotazioneAsync(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var prenotazione = await _prenotazioneService.GetPrenotazioneByIdAsync(id);
            if (prenotazione == null) return NotFound();

            var model = new PrenotazioneEditViewModel
            {
                PrenotazioneId = prenotazione.PrenotazioneId,
                ClienteId = prenotazione.ClienteId,
                CameraId = prenotazione.CameraId,
                DataInizio = prenotazione.DataInizio,
                DataFine = prenotazione.DataFine,
                Stato = prenotazione.Stato
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PrenotazioneEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _prenotazioneService.UpdatePrenotazioneAsync(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var prenotazione = await _prenotazioneService.GetPrenotazioneByIdAsync(id);
            if (prenotazione == null) return NotFound();

            return View(prenotazione);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _prenotazioneService.DeletePrenotazioneAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
