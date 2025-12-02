using AutoMapper;
using Kemar.HRM.Model.Request;
using Kemar.HRM.Model.Response;
using Kemar.HRM.Repository.Entity;

namespace Kemar.HRM.API.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Student

            // Request → Entity
            CreateMap<StudentRequest, Student>();

            // Entity → Response
            CreateMap<Student, StudentResponse>()
                .ForMember(dest => dest.TotalPayments,
                    opt => opt.MapFrom(src =>
                        src.Payments != null ? src.Payments.Count : 0))
                .ForMember(dest => dest.TotalRoomAllocations,
                    opt => opt.MapFrom(src =>
                        src.RoomAllocations != null ? src.RoomAllocations.Count : 0));

            #endregion


            #region Room

            CreateMap<RoomRequest, Room>();

            CreateMap<Room, RoomResponse>();

            #endregion


            #region RoomAllocation

            // Request → Entity
            CreateMap<RoomAllocationRequest, RoomAllocation>()
                .ForMember(dest => dest.RoomAllocationId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());

            // Entity → Response
            CreateMap<RoomAllocation, RoomAllocationResponse>()
                .ForMember(dest => dest.Student,
                    opt => opt.MapFrom(src => src.Student))
                .ForMember(dest => dest.Room,
                    opt => opt.MapFrom(src => src.Room));

            #endregion
        }
    }
}
