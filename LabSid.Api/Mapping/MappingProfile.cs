using AutoMapper;
using LabSid.Api.Models;
using LabSid.Models;
using LabSid.Models.Interfaces;

namespace LabSid.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserCreateModel, User>();
            CreateMap<UserUpdateModel, User>();
        }
    }
}
