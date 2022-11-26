using ECommerceProject.Interfaces;
using ECommerceProject.Models.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerceProject.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public  bool Delete(ApplicationUser user)
        {
            _context.Remove(user);
            return Save();
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<ApplicationUser> GetUserById(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<ApplicationUser> GetDetails(string id)
        {
            var user = await _context.Users.Where(i=>i.Id == id).FirstOrDefaultAsync();
            return  user;
        }



        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(ApplicationUser user)
        {
            _context.Update(user);
            return Save();
        }

        public  async Task<ApplicationUser> GetByIdNoTracking(string id)
        {
            return await _context.Users.Where(u=>u.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }

    }
}
