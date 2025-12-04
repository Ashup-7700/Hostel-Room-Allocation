using AutoMapper;
using Kemar.HRM.Model.Common;
using Kemar.HRM.Model.Filter;
using Kemar.HRM.Model.Request;
using Kemar.HRM.Model.Response;
using Kemar.HRM.Repository.Context;
using Kemar.HRM.Repository.Entity;
using Kemar.HRM.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Kemar.HRM.Repository.Repositories
{
    public class RoomAllocationRepository : IRoomAllocation
    {
        private readonly HostelDbContext _context;
        private readonly IMapper _mapper;

        public RoomAllocationRepository(HostelDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultModel> AddOrUpdateAsync(RoomAllocationRequest request)
        {
            try
            {
                if (request.RoomAllocationId.HasValue && request.RoomAllocationId.Value > 0)
                {
                    var existing = await _context.RoomAllocations
                        .FirstOrDefaultAsync(ra => ra.RoomAllocationId == request.RoomAllocationId.Value);

                    if (existing == null)
                        return ResultModel.NotFound("Room allocation not found");

                    _mapper.Map(request, existing);

                    existing.UpdatedAt = DateTime.UtcNow;
                    if (!string.IsNullOrWhiteSpace(request.UpdatedBy))
                        existing.UpdatedBy = request.UpdatedBy;

                    await _context.SaveChangesAsync();

                    return ResultModel.Updated(null, "Room allocation updated successfully");
                }
                else
                {
                    if (await ExistsActiveAllocationAsync(request.StudentId, request.RoomId))
                    {
                        return ResultModel.Failure(ResultCode.DuplicateRecord, "Active allocation already exists for this student-room");
                    }

                    var entity = _mapper.Map<RoomAllocation>(request);

                    entity.CreatedAt = DateTime.UtcNow;
                    entity.CreatedBy = request.CreatedBy;
                    entity.IsActive = true;

                    if (request.AllocatedByUserId.HasValue)
                        entity.AllocatedByUserId = request.AllocatedByUserId.Value;

                    await _context.RoomAllocations.AddAsync(entity);
                    await _context.SaveChangesAsync();

                    return ResultModel.Created(null, "Room allocation created successfully");
                }
            }
            catch (Exception ex)
            {
                return ResultModel.Failure(ResultCode.ExceptionThrown, ex.Message);
            }
        }

        public async Task<ResultModel> GetByIdAsync(int allocationId)
        {
            var entity = await _context.RoomAllocations
                .AsNoTracking()
                .Include(ra => ra.Student)
                .Include(ra => ra.Room)
                .Include(ra => ra.AllocatedBy)
                .FirstOrDefaultAsync(ra => ra.RoomAllocationId == allocationId);

            if (entity == null)
                return ResultModel.NotFound("Room allocation not found");

            var dto = _mapper.Map<RoomAllocationResponse>(entity);
            return ResultModel.Success(dto, "Room allocation fetched successfully");
        }

        public async Task<ResultModel> GetByFilterAsync(RoomAllocationFilter filter)
        {
            var query = _context.RoomAllocations
                .AsNoTracking()
                .Include(ra => ra.Student)
                .Include(ra => ra.Room)
                .Include(ra => ra.AllocatedBy)
                .AsQueryable();

            if (filter.StudentId.HasValue)
                query = query.Where(ra => ra.StudentId == filter.StudentId.Value);

            if (filter.RoomId.HasValue)
                query = query.Where(ra => ra.RoomId == filter.RoomId.Value);

            if (filter.AllocatedByUserId.HasValue)
                query = query.Where(ra => ra.AllocatedByUserId == filter.AllocatedByUserId.Value);

            if (filter.IsActive.HasValue)
                query = query.Where(ra => ra.IsActive == filter.IsActive.Value);

            if (filter.FromAllocatedAt.HasValue)
                query = query.Where(ra => ra.AllocatedAt >= filter.FromAllocatedAt.Value);

            if (filter.ToAllocatedAt.HasValue)
                query = query.Where(ra => ra.AllocatedAt <= filter.ToAllocatedAt.Value);

            var list = await query.ToListAsync();

            var dtoList = _mapper.Map<List<RoomAllocationResponse>>(list);
            return ResultModel.Success(dtoList, "Room allocation list fetched successfully");
        }

        public async Task<ResultModel> DeleteAsync(int allocationId, string deletedBy = null)
        {
            var existing = await _context.RoomAllocations.FirstOrDefaultAsync(ra => ra.RoomAllocationId == allocationId);
            if (existing == null)
                return ResultModel.NotFound("Room allocation not found");

            existing.IsActive = false;
            existing.UpdatedAt = DateTime.UtcNow;
            if (!string.IsNullOrWhiteSpace(deletedBy))
                existing.UpdatedBy = deletedBy;

            await _context.SaveChangesAsync();

            return ResultModel.Success(null, "Room allocation deleted successfully");
        }

        public async Task<bool> ExistsActiveAllocationAsync(int studentId, int roomId)
        {
            return await _context.RoomAllocations
                .AnyAsync(ra => ra.StudentId == studentId && ra.RoomId == roomId && ra.IsActive);

        }
    }
}
