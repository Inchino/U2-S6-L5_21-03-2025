using GestionaleHotel.Services;
using GestionaleHotel.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GestionaleHotel.Controllers
{
    public class ClientiController : Controller
    {
        private readonly IClienteService _clienteService;

        public ClientiController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        public async Task<IActionResult> Index()
        {
            var clienti = await _clienteService.GetAllClientiAsync();
            return View(clienti);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClienteCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _clienteService.AddClienteAsync(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var cliente = await _clienteService.GetClienteByIdAsync(id);
            if (cliente == null) return NotFound();

            var model = new ClienteEditViewModel
            {
                ClienteId = cliente.ClienteId,
                Nome = cliente.Nome,
                Cognome = cliente.Cognome,
                Email = cliente.Email,
                Telefono = cliente.Telefono
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ClienteEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _clienteService.UpdateClienteAsync(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await _clienteService.GetClienteByIdAsync(id);
            if (cliente == null) return NotFound();

            return View(cliente);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _clienteService.DeleteClienteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
