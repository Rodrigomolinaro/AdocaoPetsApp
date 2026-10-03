using Microsoft.EntityFrameworkCore;
using AdocaoPetsApp.Data;
using AdocaoPetsApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdocaoPetsApp.Services
{
    public class PetService
    {
        private readonly DatabaseContext _context;

        public PetService(DatabaseContext context)
        {
            _context = context;
        }

        // Buscar todos os pets disponíveis
        public async Task<List<Pet>> GetPetsAsync()
        {
            return await _context.Pets.ToListAsync();
        }

        // Salvar um novo pet no banco
        public async Task<int> SalvarPetAsync(Pet pet)
        {
            _context.Pets.Add(pet);
            return await _context.SaveChangesAsync();
        }

        // Excluir um pet
        public async Task<int> DeletarPetAsync(Pet pet)
        {
            _context.Pets.Remove(pet);
            return await _context.SaveChangesAsync();
        }
    }
}