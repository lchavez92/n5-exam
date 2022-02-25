using System;
using AutoMapper;
using N5.Exam.Domain.Models;
using N5.Exam.Infrastructure.Models;

namespace N5.Exam.Domain.Profiles
{
    public class PermissionProfile : Profile
    {
        public PermissionProfile()
        {
            CreateMap<Permission, PermissionItemDto>()
                .ForMember(x => x.PermissionTypeDescription, opt =>
                    opt.MapFrom(src => src.PermissionType.Description));

            CreateMap<RequestPermissionDto, Permission>()
                .ForMember(x => x.PermissionDate,
                    opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<ModifyPermissionDto, Permission>();
        }
    }
}
