using Kemar.HRM.Repository.Entity;

namespace Kemar.HRM.Repository.Interface
{
    public interface IFeeStructure
    {
        Task<FeeStructure?> GetByRoomTypeAsync(string roomType);
        Task<IEnumerable<FeeStructure>> GetAllAsync();
        Task<FeeStructure> CreateAsync(FeeStructure entity);
        Task<FeeStructure?> UpdateAsync(FeeStructure entity);
        Task<bool> DeleteAsync(int id);

    }
}
