
using Shipping.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Shipping.DTOs.GovernmentDTOs;

namespace Shipping.Services
{
    public class GovernmentService : IGovernmentService
    {
        private readonly ShippingContext _context;
        public GovernmentService(ShippingContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<GovernmentDTO>> GetAllExistGovernmentsAsync()
        {
            return await _context.Governments
                .Include(g => g.Branch)
                .Where(g => g.IsDeleted == false)
                .Select(g => new GovernmentDTO
                {
                    Id = g.Id,
                    Name = g.Name,
                    IsDeleted = g.IsDeleted,
                    Branch_Id = g.Branch_Id
                })
                .ToListAsync();
        }

        
        public async Task<IEnumerable<GovernmentDTO>> GetAllGovernmentsAsync()
        {
            return await _context.Governments
                .Include(g => g.Branch)
                .Select(g => new GovernmentDTO
                {
                    Id = g.Id,
                    Name = g.Name,
                    IsDeleted = g.IsDeleted,
                    Branch_Id = g.Branch_Id
                })
                .ToListAsync();
        }


        public async Task<GovernmentDTO> GetGovernmentByIdAsync(int id)
        {
            return await _context.Governments
                .Include(g => g.Branch)
                .Where(g => g.Id == id)
                .Select(g => new GovernmentDTO
                {
                    Id = g.Id,
                    Name = g.Name,
                    IsDeleted = g.IsDeleted,
                    Branch_Id = g.Branch_Id
                })
                .FirstOrDefaultAsync();
        }


        public async Task AddGovernmentAsync(GovernmentCreateDTO governmentCreateDto)
        {
            var government = new Government
            {
                Name = governmentCreateDto.Name,
                Branch_Id = governmentCreateDto.Branch_Id
            };

            await _context.Governments.AddAsync(government);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateGovernmentAsync(int id, GovernmentDTO governmentDto)
        {
            var government = await _context.Governments.FindAsync(id);
            if (government == null)
                throw new Exception("Government not found.");

            government.Name = governmentDto.Name;
            government.IsDeleted = governmentDto.IsDeleted;
            government.Branch_Id = governmentDto.Branch_Id;

            _context.Governments.Update(government);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteGovernmentAsync(int id)
        {
            var government = await _context.Governments.FindAsync(id);
            if (government == null) throw new Exception("Government not found.");
            if (government.IsDeleted) throw new Exception("Government is already deleted.");

            government.IsDeleted = true;
            _context.Governments.Update(government);
            await _context.SaveChangesAsync();
        }
    }
}