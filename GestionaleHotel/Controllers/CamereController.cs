using GestionaleHotel.Services;
using GestionaleHotel.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GestionaleHotel.Controllers
{
    [Authorize(Roles = "Admin,Editor")]
    public class CamereController : Controller
    {
        private readonly ICameraService _cameraService;

        public CamereController(ICameraService cameraService)
        {
            _cameraService = cameraService;
        }

        public async Task<IActionResult> Index()
        {
            var camere = await _cameraService.GetAllCamereAsync();
            return View(camere);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CameraCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _cameraService.AddCameraAsync(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var camera = await _cameraService.GetCameraByIdAsync(id);
            if (camera == null) return NotFound();

            var model = new CameraEditViewModel
            {
                CameraId = camera.CameraId,
                Numero = camera.Numero,
                Tipo = camera.Tipo,
                Prezzo = camera.Prezzo,
                Disponibile = camera.Disponibile
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CameraEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _cameraService.UpdateCameraAsync(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var camera = await _cameraService.GetCameraByIdAsync(id);
            if (camera == null) return NotFound();

            return View(camera);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _cameraService.DeleteCameraAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
