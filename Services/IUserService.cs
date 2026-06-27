using MyAssignment.Models;

namespace MyAssignment.Services
{
    public interface IUserService
    {
        List<User> GetAllUsers();
        User? GetUserById(int id);
        User CreateUser(UserDto dto);
        User? UpdateUser(int id, UserDto dto);
        bool DeleteUser(int id);
    }
}
