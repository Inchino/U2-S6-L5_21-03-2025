using GestionaleHotel.Data;
using GestionaleHotel.Models;
using GestionaleHotel.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace GestionaleHotel.Services
{
    public class ClienteService : IClienteService
    {
        private readonly HotelDbContext _context;

        public ClienteService(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ClienteListViewModel>> GetAllClientiAsync()
        {
            return await _context.Clienti
                .Select(c => new ClienteListViewModel
                {
                    ClienteId = c.ClienteId,
                    NomeCompleto = $"{c.Nome} {c.Cognome}",
                    Email = c.Email,
                    Telefono = c.Telefono
                })
                .ToListAsync();
        }

        public async Task<ClienteDetailViewModel> GetClienteByIdAsync(int id)
        {
            var cliente = await _context.Clienti.FindAsync(id);
            if (cliente == null) return null;

            return new ClienteDetailViewModel
            {
                ClienteId = cliente.ClienteId,
                Nome = cliente.Nome,
                Cognome = cliente.Cognome,
                Email = cliente.Email,
                Telefono = cliente.Telefono
            };
        }

        public async Task<bool> AddClienteAsync(ClienteCreateViewModel model)
        {
            var cliente = new Cliente
            {
                Nome = model.Nome,
                Cognome = model.Cognome,
                Email = model.Email,
                Telefono = model.Telefono
            };

            _context.Clienti.Add(cliente);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateClienteAsync(ClienteEditViewModel model)
        {
            var cliente = await _context.Clienti.FindAsync(model.ClienteId);
            if (cliente == null) return false;

            cliente.Nome = model.Nome;
            cliente.Cognome = model.Cognome;
            cliente.Email = model.Email;
            cliente.Telefono = model.Telefono;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteClienteAsync(int id)
        {
            var cliente = await _context.Clienti.FindAsync(id);
            if (cliente == null) return false;

            _context.Clienti.Remove(cliente);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
