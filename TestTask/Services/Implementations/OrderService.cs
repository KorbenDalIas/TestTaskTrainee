using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Enums;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order> GetOrder()
        {
            return await _context.Orders
                .OrderByDescending(x => x.CreatedAt).Where( x => x.Quantity > 1).FirstAsync();
        }

        public async Task<List<Order>> GetOrders()
        {
            return await _context.Orders
                .Where(x => x.User.Status == UserStatus.Active)
                .OrderBy(x => x.CreatedAt).ToListAsync();
        }
    }
}
