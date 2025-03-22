using GestionaleHotel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GestionaleHotel.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GestioneDipendentiController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public GestioneDipendentiController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var utenti = _userManager.Users.ToList();
            return View(utenti);
        }

        public IActionResult Create()
        {
            ViewBag.Ruoli = _roleManager.Roles
                .Where(r => r.Name != "Admin")
                .Select(r => r.Name)
                .ToList();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string email, string password, string nome, string cognome, DateOnly birthDate, string ruolo)
        {
            if (ruolo == "Admin")
            {
                TempData["ErroreRuolo"] = "Non è possibile creare un utente con ruolo Admin.";
                return RedirectToAction("Index");
            }

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                FirstName = nome,
                LastName = cognome,
                BirthDate = birthDate
            };

            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, ruolo);
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            ViewBag.Ruoli = _roleManager.Roles
                .Where(r => r.Name != "Admin")
                .Select(r => r.Name)
                .ToList();

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var ruoliUtente = await _userManager.GetRolesAsync(user);
            if (ruoliUtente.Contains("Admin"))
                return Forbid();

            var ruoliDisponibili = _roleManager.Roles
                .Where(r => r.Name != "Admin")
                .Select(r => r.Name)
                .ToList();

            var ruoloUtente = ruoliUtente.FirstOrDefault();

            ViewBag.Ruoli = ruoliDisponibili;
            ViewBag.Utente = user;
            ViewBag.RuoloCorrente = ruoloUtente;

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, string nuovoRuolo)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var ruoliUtente = await _userManager.GetRolesAsync(user);

            if (ruoliUtente.Contains("Admin"))
                return Forbid();

            if (nuovoRuolo == "Admin")
            {
                TempData["ErroreRuolo"] = "Non è possibile assegnare il ruolo Admin.";
                return RedirectToAction("Index");
            }

            if (ruoliUtente.Any())
                await _userManager.RemoveFromRolesAsync(user, ruoliUtente);

            await _userManager.AddToRoleAsync(user, nuovoRuolo);

            return RedirectToAction("Index");
        }



        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var ruoliUtente = await _userManager.GetRolesAsync(user);
            if (ruoliUtente.Contains("Admin"))
                return Forbid();

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var ruoliUtente = await _userManager.GetRolesAsync(user);
            if (ruoliUtente.Contains("Admin"))
                return Forbid();

            await _userManager.DeleteAsync(user);
            return RedirectToAction("Index");
        }


    }

}
