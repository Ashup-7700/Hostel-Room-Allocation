using Kemar.HRM.Repository.Context;
using Kemar.HRM.Repository.Entity;
using Kemar.HRM.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Kemar.HRM.Repository.Repositories
{
    public class FeeStructureRepository : IFeeStructure
    {
        private readonly HostelDbContext _context;

        public FeeStructureRepository(HostelDbContext context)
        {
            _context = context;
        }

        public async Task<FeeStructure?> GetByRoomTypeAsync(string roomType)
        {
            return await _context.FeeStructures
                                 .FirstOrDefaultAsync(fs => fs.RoomType.ToLower() == roomType.ToLower());
        }

        public async Task<IEnumerable<FeeStructure>> GetAllAsync()
        {
            return await _context.FeeStructures.ToListAsync();
        }

        public async Task<FeeStructure> CreateAsync(FeeStructure entity)
        {
            _context.FeeStructures.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<FeeStructure?> UpdateAsync(FeeStructure entity)
        {
            var existing = await _context.FeeStructures.FindAsync(entity.FeeStructureId);

            if (existing == null)
                return null;

            existing.RoomType = entity.RoomType;
            existing.MonthlyRent = entity.MonthlyRent;
            existing.SecurityDeposit = entity.SecurityDeposit;
            existing.UpdatedAt = DateTime.UtcNow;
            existing.UpdatedBy = entity.UpdatedBy;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.FeeStructures.FindAsync(id);

            if (entity == null)
                return false;

            _context.FeeStructures.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
