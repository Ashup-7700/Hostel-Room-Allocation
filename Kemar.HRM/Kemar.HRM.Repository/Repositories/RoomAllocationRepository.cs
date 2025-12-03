//using AutoMapper;
//using Kemar.HRM.Repository.Context;
//using Kemar.HRM.Repository.Entity;
//using Kemar.HRM.Repository.Interface;
//using Microsoft.EntityFrameworkCore;

//public class RoomAllocationRepository : IRoomAllocation
//{
//    private readonly HostelDbContext _context;
//    private readonly IMapper _mapper;

//    public RoomAllocationRepository(HostelDbContext context, IMapper mapper)
//    {
//        _context = context;
//        _mapper = mapper;
//    }

//    public async Task<IEnumerable<RoomAllocation>> GetAllAsync()
//    {
//        return await _context.RoomAllocations
//            .Include(r => r.Student)
//            .Include(r => r.Room)
//            .ToListAsync();
//    }

//    public async Task<RoomAllocation?> GetByIdAsync(int id)
//    {
//        return await _context.RoomAllocations
//            .Include(r => r.Student)
//            .Include(r => r.Room)
//            .FirstOrDefaultAsync(r => r.RoomAllocationId == id);
//    }

//    public async Task<IEnumerable<RoomAllocation>> GetAllocationsByStudent(int studentId)
//    {
//        return await _context.RoomAllocations
//            .Where(x => x.StudentId == studentId)
//            .Include(r => r.Student)
//            .Include(r => r.Room)
//            .ToListAsync();
//    }

//    public async Task<IEnumerable<RoomAllocation>> GetAllocationsByRoom(int roomId)
//    {
//        return await _context.RoomAllocations
//            .Where(x => x.RoomId == roomId)
//            .Include(r => r.Student)
//            .Include(r => r.Room)
//            .ToListAsync();
//    }

//    public async Task<RoomAllocation> CreateAsync(RoomAllocation entity)
//    {
//        await _context.RoomAllocations.AddAsync(entity);
//        await _context.SaveChangesAsync();
//        return entity;
//    }

//    public async Task<RoomAllocation?> UpdateAsync(RoomAllocation entity)
//    {
//        var existing = await GetByIdAsync(entity.RoomAllocationId);
//        if (existing == null)
//            return null;

//        _context.Entry(existing).CurrentValues.SetValues(entity);
//        await _context.SaveChangesAsync();
//        return existing;
//    }

//    public async Task<bool> DeleteAsync(int id)
//    {
//        var entity = await GetByIdAsync(id);
//        if (entity == null) return false;

//        _context.RoomAllocations.Remove(entity);
//        await _context.SaveChangesAsync();
//        return true;
//    }

//    public async Task<bool> CheckoutAsync(int allocationId)
//    {
//        var entity = await GetByIdAsync(allocationId);
//        if (entity == null) return false;

//        entity.CheckoutDate = DateTime.Now;

//        _context.RoomAllocations.Update(entity);
//        await _context.SaveChangesAsync();
//        return true;
//    }
//}
