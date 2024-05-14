using Microsoft.EntityFrameworkCore;
using System.Linq;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly int firstTaskYear = 2003;
        private readonly int secondTaskYear = 2010;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUser()
        {
            //делаю действия с orders тут, потому что нельзя изменять OrderService, а других путей найти не смог
            var selectedOrdersList = await _context.Orders.
                Where(x => x.CreatedAt.Year == firstTaskYear && x.Status == Enums.OrderStatus.Delivered).ToListAsync();
            var userId = selectedOrdersList.GroupBy(x => x.UserId)
                                  .Select(s => new { Id = s.Key, TotalSum = s.Select(s2 => s2.Price).Sum() })
                                  .OrderByDescending(x => x.TotalSum).First().Id;
            return await _context.Users.Where(x => x.Id == userId).FirstAsync();
        }

        public async Task<List<User>> GetUsers()
        {
            return await _context.Users.Where(x => x.Orders
                .Any(x => x.CreatedAt.Year == secondTaskYear && x.Status == Enums.OrderStatus.Paid)).ToListAsync();
        }
    }
}