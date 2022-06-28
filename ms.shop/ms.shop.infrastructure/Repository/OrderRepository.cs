using Microsoft.EntityFrameworkCore;
using ms.shop.domain.Entities;
using ms.shop.domain.Interfaces;
using ms.shop.infrastructure.Data;

namespace ms.shop.infrastructure.Repository
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext context;

        public OrderRepository(ApplicationDbContext context): base(context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Order>> GetOrdersBetween(DateTime startDate, DateTime endDate)
        {
            return await context.Orders.Where(x => x.OrderDate >= startDate && x.OrderDate <= endDate).ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersGreaterThan(decimal amount)
        {
            return await context.Orders.Where(x => x.Amount >= amount).ToListAsync();
        }
    }
}
