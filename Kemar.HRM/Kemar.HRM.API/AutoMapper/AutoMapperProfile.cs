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
            CreateMap<StudentRequest, Student>();

            CreateMap<Student, StudentResponse>();
            #endregion

            #region Room
            CreateMap<RoomRequest, Room>()
                .ForMember(dest => dest.RoomId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore());

            CreateMap<Room, RoomResponse>();
            #endregion

            #region RoomAllocation
            CreateMap<RoomAllocationRequest, RoomAllocation>()
              .ForMember(dest => dest.RoomAllocationId, opt => opt.MapFrom(src => src.RoomAllocationId))
              .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.StudentId))
              .ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.RoomId))
              .ForMember(dest => dest.ReleasedAt, opt => opt.MapFrom(src => src.ReleasedAt))


             .ForMember(dest => dest.RoomAllocationId, opt => opt.Ignore())
            .ForMember(dest => dest.AllocatedByUser, opt => opt.Ignore());
            CreateMap<RoomAllocation, RoomAllocationResponse>()
                .ForMember(dest => dest.RoomAllocationId, opt => opt.MapFrom(src => src.RoomAllocationId))
                .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.StudentId))
                .ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.RoomId))
                .ForMember(dest => dest.AllocatedAt, opt => opt.MapFrom(src => src.AllocatedAt))
                .ForMember(dest => dest.ReleasedAt, opt => opt.MapFrom(src => src.ReleasedAt))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student != null ? src.Student.Name : string.Empty))
                .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.Room != null ? src.Room.RoomNumber : string.Empty))
                .ForMember(dest => dest.AllocatedByUsername, opt => opt.MapFrom(src => src.AllocatedByUser != null ? src.AllocatedByUser.Username : string.Empty));


            #endregion

            #region User
            CreateMap<UserRequest, User>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Password, opt => opt.Ignore());

            CreateMap<User, UserResponse>();
            #endregion

            #region Payment
            CreateMap<PaymentRequest, Payment>()
                .ForMember(dest => dest.PaymentId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())

                .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.StudentId))
                .ForMember(dest => dest.Student, opt => opt.Ignore()); 

            CreateMap<Payment, PaymentResponse>();
            #endregion


            #region FeeStructure
            CreateMap<FeeStructureRequest, FeeStructure>()
                .ForMember(dest => dest.FeeStructureId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore());

            CreateMap<FeeStructure, FeeStructureResponse>();
            #endregion
        }
    }
}
