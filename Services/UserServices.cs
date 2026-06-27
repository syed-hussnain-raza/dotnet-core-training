using AutoMapper;
using MyAssignment.Models;

namespace MyAssignment.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;

        // moved from controller
        private static List<User> _users = new List<User>
        {
          new User(1, "Hussnain", "hussnain@gmail.com", "03001234567", "Premium", true),
          new User(2, "Hasnat",   "hasnat@gmail.com",   "03001234432", "Premium", true),
          new User(3, "Ali",      "ali@gmail.com",       "03007654321", "Basic",   true),
          new User(4, "Sara",     "sara@gmail.com",      "03009876543", "Premium", false)
        };

        public UserService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public List<User> GetAllUsers()
        {
            return _users;
        }

        public User? GetUserById(int id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public User CreateUser(UserDto dto)
        {
            var user = _mapper.Map<User>(dto);
            user.Id = GenerateId();
            user.IsActive = true;
            _users.Add(user);
            return user;
        }

        public User? UpdateUser(int id, UserDto dto)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);

            if (user == null) return null;

            _mapper.Map(dto, user);
            return user;
        }

        public bool DeleteUser(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);

            if (user == null) return false;

            _users.Remove(user);
            return true;
        }

        // Private helpers
        private int GenerateId() => _users.Max(u => u.Id) + 1;
    }
}