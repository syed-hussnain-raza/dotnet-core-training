using AutoMapper;
using MyAssignment.Models;
using MyAssignment.Dtos;

namespace MyAssignment.Services
{
    /// <summary>
    /// Provides business logic for managing users, backed by an in-memory store.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        
        /// <summary>
        /// In-memory list acting as a database
        /// </summary>
        private static List<User> _users = new List<User>
        {
            new User(1, "Hussnain", "hussnain@gmail.com", "03001234567", "Premium", true),
            new User(2, "Hasnat", "hasnat@gmail.com", "03001234432", "Premium", true),
            new User(3, "Ali", "ali@gmail.com", "03007654321", "Basic", true),
            new User(4, "Sara", "sara@gmail.com", "03009876543", "Premium", false)
        };

        /// <summary>
        /// Creates a new UserService with the given AutoMapper instance injected.
        /// </summary>
        /// <param name="mapper">AutoMapper instance used to map DTOs to domain models.</param>
        public UserService(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>The full list of users.</returns>
        public List<User> GetAllUsers()
        {
            return _users;
        }

        /// <summary>
        /// Retrieves a single user by id.
        /// </summary>
        /// <param name="id">The unique identifier to search for.</param>
        /// <returns>The matching user, or null if none exists.</returns>
        public User? GetUserById(int id)
        {
            User? user = FindUser(id);
            return user;
        }

        /// <summary>
        /// Creates a new user from the given DTO.
        /// </summary>
        /// <param name="dto">The data used to create the user.</param>
        /// <returns>The newly created user.</returns>
        public User CreateUser(UserDto dto)
        {
            User user = _mapper.Map<User>(dto);
            user.Id = GenerateId();
            user.IsActive = true;
            _users.Add(user);
            return user;
        }

        /// <summary>
        /// Updates an existing user with the given DTO's values.
        /// </summary>
        /// <param name="id">The unique identifier of the user to update.</param>
        /// <param name="dto">The new data to apply.</param>
        /// <returns>The updated user, or null if no user with the given id exists.</returns>
        public User? UpdateUser(int id, UserDto dto)
        {
            User? user = FindUser(id);

            if (user == null)
            {
                return null;
            }

            _mapper.Map(dto, user);
            return user;
        }

        /// <summary>
        /// Deletes a user by id.
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete.</param>
        /// <returns>True if the user was found and deleted; otherwise false.</returns>
        public bool DeleteUser(int id)
        {
            User? user = FindUser(id);

            if (user == null)
            {
                return false;
            }

            _users.Remove(user);
            return true;
        }

        /// <summary>
        /// Generates the next available user id. Safe on an empty list.
        /// </summary>
        /// <returns>A new unique id.</returns>
        private int GenerateId()
        {
            int nextId = _users.Count == 0 ? 1 : _users.Max(u => u.Id) + 1;
            return nextId;
        }

        /// <summary>
        /// Finds a user by id in the in-memory list.
        /// </summary>
        private User? FindUser(int id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }
    }
}