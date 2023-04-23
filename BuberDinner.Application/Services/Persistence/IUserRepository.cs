using BuberDinner.Domain.Entities;

namespace BuberDinner.Application.Services.Persistence
{
    public interface IUserRepository
    {
        void Add(User user);
        User? GetUserByEmail(string email);
    }
}