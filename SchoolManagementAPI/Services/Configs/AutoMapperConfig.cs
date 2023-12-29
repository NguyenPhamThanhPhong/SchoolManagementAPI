using AutoMapper;
using SchoolManagementAPI.Models.Embeded.SchoolClass;
using SchoolManagementAPI.Models.Entities;
using SchoolManagementAPI.RequestResponse.Request;

namespace SchoolManagementAPI.Services.Configs
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<SchoolMemberCreateRequest, Student>();
            CreateMap<SchoolMemberCreateRequest, Lecturer>();
            CreateMap<AccountCreateRequest, Admin>();
            CreateMap<PostCreateRequest, Post>();
            CreateMap<PostUpdateRequest, Post>();
            CreateMap<SchoolClassCreateRequest, SchoolClass>();
            CreateMap<SchoolClassUpdateSectionsRequest, Section>();
        }
    }
}
