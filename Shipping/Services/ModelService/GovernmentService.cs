
using Shipping.Models;
using Shipping.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Shipping.Services
{
    public class GovernmentService : IGovernmentService
    {
        private readonly ShippingContext _context;

        public GovernmentService(ShippingContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GovernmentDTO>> GetAllGovernmentsAsync()
        {
            return await _context.Governments
                .Where(g => !g.IsDeleted)
                .Select(g => new GovernmentDTO
                {
                    Id = g.Id,
                    Name = g.Name,
                    Branch_Id = g.Branch_Id
                })
                .ToListAsync();
        }

        public async Task<GovernmentDTO> GetGovernmentByIdAsync(int id)
        {
            var government = await _context.Governments
                .Where(g => g.Id == id && !g.IsDeleted)
                .Select(g => new GovernmentDTO
                {
                    Id = g.Id,
                    Name = g.Name,
                    Branch_Id = g.Branch_Id
                })
                .FirstOrDefaultAsync();

            return government;
        }

        public async Task AddGovernmentAsync(GovernmentDTO governmentDto)
        {
            var government = new Government
            {
                Name = governmentDto.Name,
                Branch_Id = governmentDto.Branch_Id
            };

            await _context.Governments.AddAsync(government);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateGovernmentAsync(int id, GovernmentDTO governmentDto)
        {
            var government = await _context.Governments.FindAsync(id);
            if (government == null || government.IsDeleted)
                throw new Exception("Government not found.");

            government.Name = governmentDto.Name;
            government.Branch_Id = governmentDto.Branch_Id;

            _context.Governments.Update(government);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteGovernmentAsync(int id)
        {
            var government = await _context.Governments.FindAsync(id);
            if (government == null || government.IsDeleted)
                throw new Exception("Government not found.");

            government.IsDeleted = true;
            _context.Governments.Update(government);
            await _context.SaveChangesAsync();
        }
    }
}