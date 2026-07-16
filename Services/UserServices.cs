using AutoMapper;
using MyAssignment.Dtos;
using MyAssignment.Models;
using MyAssignment.Repositories;
using MyAssignment.Helper;
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

        public async Task<List<User>> GetAllUsersAsync()
        {
            List<User> users = await _userRepository.GetAllAsync();
            return users;
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            User? user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new Exception(MessagesConstants.UserNotFound);
            return user;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            User? user = await _userRepository.GetByEmailAsync(email);
            if (user == null) throw new Exception(MessagesConstants.UserNotFound);
            return user;
        }

        public async Task<User> CreateUserAsync(UserDto dto)
        {
            User user = _mapper.Map<User>(dto);
            user.IsActive = true;
            user.UserName = dto.FirstName + dto.LastName;

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return user;
        }

        public async Task<User> UpdateUserAsync(string id, UserDto dto)
        {
            User? user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                throw new Exception(MessagesConstants.UserNotFound);
            }

            _mapper.Map(dto, user);
            user.UserName = dto.FirstName + dto.LastName;
            await _userRepository.SaveChangesAsync();

            return user;
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

        public async Task<PagedResult<User>> GetUsersPagedAsync(QueryParameters parameters)
        {
            Expression<Func<User, bool>>? filter = BuildFilter(parameters.SearchTerm);
            Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = BuildOrderBy(parameters.SortBy, parameters.SortDescending);

            (List<User> items, int totalCount) = await _userRepository.GetPagedAsync(
                parameters.Page, parameters.PageSize, filter, orderBy);

            return new PagedResult<User>(items, totalCount, parameters.Page, parameters.PageSize);
        }

        // Private Helper Methods
        private Expression<Func<User, bool>>? BuildFilter(string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return null;
            }

            return u => (u.FirstName + " " + u.LastName).Contains(searchTerm) || u.Email.Contains(searchTerm);
        }

        // Maps a sort field name from the query string to the actual property.
        // Unrecognized values fall back to sorting by Id rather than throwing.
        private Func<IQueryable<User>, IOrderedQueryable<User>>? BuildOrderBy(string? sortBy, bool descending)
        {
            if (string.IsNullOrWhiteSpace(sortBy))
            {
                return q => q.OrderBy(u => u.Id);
            }

            return sortBy.ToLower() switch
            {
                "fullname" => q => descending ? q.OrderByDescending(u => u.FirstName + " " + u.LastName) : q.OrderBy(u => u.FirstName + " " + u.LastName),
                "email" => q => descending ? q.OrderByDescending(u => u.Email) : q.OrderBy(u => u.Email),
                "membershiptype" => q => descending ? q.OrderByDescending(u => u.MembershipType) : q.OrderBy(u => u.MembershipType),
                _ => q => q.OrderBy(u => u.Id)
            };
        }
    }
}