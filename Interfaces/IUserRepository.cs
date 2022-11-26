using ECommerceProject.Models.Data;

namespace ECommerceProject.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsers();
        Task<ApplicationUser> GetUserById(string id);
        bool Add(ApplicationUser user);
        bool Update(ApplicationUser user);
        bool Delete(ApplicationUser user);
        bool Save();
        Task<ApplicationUser> GetDetails(string id);
        Task<ApplicationUser> GetByIdNoTracking(string id);

    }
}
