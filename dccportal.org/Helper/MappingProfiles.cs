using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dccportal.org.Dto;
using dccportal.org.Entities;

namespace dccportal.org.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Believer, BelieversDto>().ReverseMap();
            CreateMap<Department, DepartmentDto>().ReverseMap();
            CreateMap<Member, MemberDto>().ReverseMap();
            CreateMap<UsersAccount, UsersAccountDto>().ReverseMap();
            CreateMap<Unit, UnitDto>()
            .ForMember(d => d.Department, i => i.MapFrom(s => s.Dept.DeptName)).ReverseMap();
            CreateMap<Post, PostDto>().ReverseMap();
            CreateMap<WorkerAttendance, WorkerAttendanceDto>().ReverseMap();
            CreateMap<Due, DueDto>().ReverseMap();
            CreateMap<Role,RolesDto>().ReverseMap();
            CreateMap<Payment, PaymentModelDto>().ReverseMap();
        }
    }
}