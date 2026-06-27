using AutoMapper;
using MyAssignment.Models;

namespace MyAssignment.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Tell AutoMapper: UserDto → User
            // Id and IsActive are NOT in UserDto so they keep their defaults
            CreateMap<UserDto, User>();
        }
    }
}
