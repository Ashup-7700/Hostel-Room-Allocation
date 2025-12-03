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

            // Student mapping
            CreateMap<StudentRequest, Student>()
                .ForMember(dest => dest.StudentId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.RoomAllocations, opt => opt.Ignore())
                .ForMember(dest => dest.Payments, opt => opt.Ignore());

            CreateMap<Student, StudentResponse>();


            #endregion


            #region Room

            CreateMap<RoomRequest, Room>();

            CreateMap<Room, RoomResponse>();

            #endregion


            #region RoomAllocation

            CreateMap<RoomAllocationRequest, RoomAllocation>()
                .ForMember(dest => dest.RoomAllocationId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());

            CreateMap<RoomAllocation, RoomAllocationResponse>()
                .ForMember(dest => dest.Student,
                    opt => opt.MapFrom(src => src.Student))
                .ForMember(dest => dest.Room,
                    opt => opt.MapFrom(src => src.Room));

            #endregion


            #region User
           
            CreateMap<UserRequest, User>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.AllocationsHandled, opt => opt.Ignore());

            CreateMap<User, UserResponse>();
            #endregion

        }
    }
}
