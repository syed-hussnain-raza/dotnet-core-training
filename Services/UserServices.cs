using AutoMapper;
using MyAssignment.Dtos;
using MyAssignment.Models;
using MyAssignment.Repositories;

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

        public async Task<User?> GetUserByIdAsync(int id)
        {
            User? user = await _userRepository.GetByIdAsync(id);
            return user;
        }

        public async Task<User> CreateUserAsync(UserDto dto)
        {
            User user = _mapper.Map<User>(dto);
            user.IsActive = true;

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return user;
        }

        public async Task<User?> UpdateUserAsync(int id, UserDto dto)
        {
            User? user = await _userRepository.GetByIdAsync(id);

            if (user != null)
            {
                _mapper.Map(dto, user);
                await _userRepository.SaveChangesAsync();
            }

            return user;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            User? user = await _userRepository.GetByIdAsync(id);
            bool deleted = false;

            if (user != null)
            {
                _userRepository.Remove(user);
                await _userRepository.SaveChangesAsync();
                deleted = true;
            }

            return deleted;
        }
    }
}