using AutoMapper;
using MyAssignment.Models;
using MyAssignment.Dtos;

namespace MyAssignment.Helper
{
    /// <summary>
    /// AutoMapper configuration for the User API.
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Tell AutoMapper: UserDto → User
            CreateMap<UserDto, User>();
        }
    }
}