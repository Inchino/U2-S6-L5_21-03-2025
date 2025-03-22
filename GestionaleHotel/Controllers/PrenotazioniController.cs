using GestionaleHotel.Services;
using GestionaleHotel.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace GestionaleHotel.Controllers
{
    [Authorize]
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

        [Authorize(Roles = "Admin,Editor,Viewer")]
        public async Task<IActionResult> Index()
        {
            var prenotazioni = await _prenotazioneService.GetAllPrenotazioniAsync();
            return View(prenotazioni);
        }

        [Authorize(Roles = "Admin,Editor")]
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
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Create(PrenotazioneCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"Errore su '{key}': {error.ErrorMessage}");
                    }
                }

                // Ricarica i SelectList
                var clienti = await _clienteService.GetAllClientiAsync();
                var camere = await _cameraService.GetCamereDisponibiliAsync();
                model.Clienti = new SelectList(clienti, "ClienteId", "NomeCompleto");
                model.CamereDisponibili = new SelectList(camere, "CameraId", "Numero");

                return View(model);
            }

            var success = await _prenotazioneService.AddPrenotazioneAsync(model);

            if (!success)
            {
                ModelState.AddModelError("", "Errore: la prenotazione non è stata salvata. Verifica che la camera sia disponibile.");

                // Ricarica i SelectList
                var clienti = await _clienteService.GetAllClientiAsync();
                var camere = await _cameraService.GetCamereDisponibiliAsync();
                model.Clienti = new SelectList(clienti, "ClienteId", "NomeCompleto");
                model.CamereDisponibili = new SelectList(camere, "CameraId", "Numero");

                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Edit(int id)
        {
            var prenotazione = await _prenotazioneService.GetPrenotazioneByIdAsync(id);
            if (prenotazione == null) return NotFound();

            var clienti = await _clienteService.GetAllClientiAsync();
            var camere = await _cameraService.GetCamereDisponibiliAsync();

            var model = new PrenotazioneEditViewModel
            {
                PrenotazioneId = prenotazione.PrenotazioneId,
                ClienteId = prenotazione.ClienteId,
                CameraId = prenotazione.CameraId,
                DataInizio = prenotazione.DataInizio,
                DataFine = prenotazione.DataFine,
                Stato = prenotazione.Stato,
                Clienti = new SelectList(clienti, "ClienteId", "NomeCompleto", prenotazione.ClienteId),
                CamereDisponibili = new SelectList(camere, "CameraId", "Numero", prenotazione.CameraId)
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Edit(PrenotazioneEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var clienti = await _clienteService.GetAllClientiAsync();
                var camere = await _cameraService.GetCamereDisponibiliAsync();

                model.Clienti = new SelectList(clienti, "ClienteId", "NomeCompleto", model.ClienteId);
                model.CamereDisponibili = new SelectList(camere, "CameraId", "Numero", model.CameraId);

                return View(model);
            }

            var success = await _prenotazioneService.UpdatePrenotazioneAsync(model);

            if (!success)
            {
                ModelState.AddModelError("", "Errore durante l'aggiornamento della prenotazione.");

                var clienti = await _clienteService.GetAllClientiAsync();
                var camere = await _cameraService.GetCamereDisponibiliAsync();

                model.Clienti = new SelectList(clienti, "ClienteId", "NomeCompleto", model.ClienteId);
                model.CamereDisponibili = new SelectList(camere, "CameraId", "Numero", model.CameraId);

                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Delete(int id)
        {
            var prenotazione = await _prenotazioneService.GetPrenotazioneByIdAsync(id);
            if (prenotazione == null) return NotFound();

            return View(prenotazione);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _prenotazioneService.DeletePrenotazioneAsync(id);

            if (!success)
            {
                ModelState.AddModelError("", "Errore durante l'eliminazione della prenotazione.");
                var prenotazione = await _prenotazioneService.GetPrenotazioneByIdAsync(id);
                return View("Delete", prenotazione);
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
