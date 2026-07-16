using AutoMapper;
using MyAssignment.Dtos;
using MyAssignment.Helper;
using MyAssignment.Models;
using MyAssignment.Repositories;
using MyAssignment.Constants;
using System.Linq.Expressions;

namespace MyAssignment.Services
{
    /// <summary>
    /// Provides business logic for managing users, backed by IUserRepository.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }



        public async Task<UserResponseDto> GetUserByIdAsync(string id)
        {
            User? user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new Exception(MessagesConstants.UserNotFound);
            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<UserResponseDto> GetUserByEmailAsync(string email)
        {
            User? user = await _userRepository.GetByEmailAsync(email);
            if (user == null) throw new Exception(MessagesConstants.UserNotFound);
            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<UserResponseDto> CreateUserAsync(UserDto dto)
        {
            User user = _mapper.Map<User>(dto);
            user.IsActive = true;
            user.UserName = UsernameGenerator.Generate(dto.FirstName, dto.LastName);

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<UserResponseDto> UpdateUserAsync(string id, UserDto dto)
        {
            User? user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                throw new Exception(MessagesConstants.UserNotFound);
            }

            _mapper.Map(dto, user);
            user.UserName = UsernameGenerator.Generate(dto.FirstName, dto.LastName);
            await _userRepository.SaveChangesAsync();

            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            User? user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                throw new Exception(MessagesConstants.UserNotFound);
            }

            _userRepository.Remove(user);
            await _userRepository.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResult<UserResponseDto>> GetUsersPagedAsync(QueryParameters parameters)
        {
            Expression<Func<User, bool>>? filter = BuildFilter(parameters.SearchTerm);
            Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = BuildOrderBy(parameters.SortBy, parameters.SortDescending);

            (List<User> items, int totalCount) = await _userRepository.GetPagedAsync(
                parameters.Page, parameters.PageSize, filter, orderBy);

            List<UserResponseDto> dtos = _mapper.Map<List<UserResponseDto>>(items);

            return new PagedResult<UserResponseDto>(dtos, totalCount, parameters.Page, parameters.PageSize);
        }

        // Private Helper Methods
        private Expression<Func<User, bool>>? BuildFilter(string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return null;
            }

            return u => u.UserName.Contains(searchTerm) || u.Email.Contains(searchTerm);
        }

        // Maps a sort field name from the query string to the actual property.
        // Unrecognized values fall back to sorting by Id rather than throwing.
        private Func<IQueryable<User>, IOrderedQueryable<User>>? BuildOrderBy(string? sortBy, bool descending)
        {
            if (string.IsNullOrWhiteSpace(sortBy))
            {
                return q => q.OrderBy(u => u.Id);
            }

            switch (sortBy.ToLower())
            {
                case SortFieldsConstants.FullName:
                    if (descending) return q => q.OrderByDescending(u => u.UserName);
                    else return q => q.OrderBy(u => u.UserName);

                case SortFieldsConstants.Email:
                    if (descending) return q => q.OrderByDescending(u => u.Email);
                    else return q => q.OrderBy(u => u.Email);

                case SortFieldsConstants.MembershipType:
                    if (descending) return q => q.OrderByDescending(u => u.MembershipType);
                    else return q => q.OrderBy(u => u.MembershipType);

                default:
                    return q => q.OrderBy(u => u.Id);
            }
        }
    }
}