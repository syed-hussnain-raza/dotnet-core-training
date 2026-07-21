using AutoMapper;
using MyAssignment.Dtos;
using MyAssignment.Helper;
using MyAssignment.Models;
using MyAssignment.Repositories;
using MyAssignment.Constants;
using System.Linq.Expressions;

namespace MyAssignment.Services.Users
{
    /// <summary>
    /// Provides business logic for managing users, backed by IUserRepository.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public UserService(IGenericRepository<User> userRepository, IMapper mapper)
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
            User? user = await _userRepository.FirstOrDefaultAsync(u => u.Email == email);
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

        public async Task<PagedResult<UserResponseDto>> GetUsersPagedAsync(Dictionary<string, string> queryParams)
        {
            (List<User> items, int totalCount) = await _userRepository.GetPagedDynamicAsync(queryParams);

            int page = 1;
            int pageSize = 10;
            
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            Dictionary<string, string> caseInsensitiveParams = new Dictionary<string, string>(queryParams, comparer);

            if (caseInsensitiveParams.TryGetValue("page", out string? pStr) && int.TryParse(pStr, out int p)) page = p;
            if (caseInsensitiveParams.TryGetValue("pageSize", out string? psStr) && int.TryParse(psStr, out int ps)) pageSize = ps;

            List<UserResponseDto> dtos = _mapper.Map<List<UserResponseDto>>(items);

            return new PagedResult<UserResponseDto>(dtos, totalCount, page, pageSize);
        }
    }
}
