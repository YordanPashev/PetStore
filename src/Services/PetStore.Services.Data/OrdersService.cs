namespace PetStore.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using PetStore.Data.Common.Repositories;
    using PetStore.Data.Models;
    using PetStore.Data.Models.Enums;
    using PetStore.Services.Data.Contracts;
    using PetStore.Web.ViewModels.Orders;

    public class OrdersService : IOrdersService
    {
        private readonly IDeletableEntityRepository<Order> orderRepo;

        public OrdersService(IDeletableEntityRepository<Order> orderRepo)
            => this.orderRepo = orderRepo;

        public async Task AddOrderAsync(CreateOrderFullInfoViewModel orderInfo)
        {
            Order order = new Order()
            {
                FirstName = orderInfo.FirstName,
                LastName = orderInfo.LastName,
                PhoneNumber = orderInfo.PhoneNumber,
                Email = orderInfo.Email,
                Address = orderInfo.Address,
                ProductId = orderInfo.ProductId,
                Quantity = orderInfo.Quantity,
                ApplicationUserId = orderInfo.ClientId,
                TotalPrice = orderInfo.TotalPriceWithDiscount ?? orderInfo.TotalPriceWithoutDiscount,
                Status = orderInfo.Status,
            };

            await this.orderRepo.AddAsync(order);
            await this.orderRepo.SaveChangesAsync();
        }

        public async Task ChangeOrderStatus(Order order, string status)
        {
            order.Status = Enum.Parse<OrderStatus>(status);

            await this.orderRepo.SaveChangesAsync();
        }

        public IQueryable<Order> GetAllClientsOrders(string clientId)
            => this.orderRepo.AllAsNoTracking()
                             .Where(o => o.ApplicationUserId == clientId)
                             .Include(o => o.Product)
                             .ThenInclude(p => p.Category)
                             .OrderByDescending(o => o.CreatedOn);

        public IQueryable<Order> GetAllOrders()
            => this.orderRepo.AllAsNoTracking()
                             .Include(o => o.Product)
                             .ThenInclude(p => p.Category);

        public async Task<Order> GetOrderByIdAsync(string id)
            => await this.orderRepo.AllAsNoTracking()
                             .Where(o => o.Id == id)
                             .Include(o => o.Product)
                             .ThenInclude(p => p.Category)
                             .FirstOrDefaultAsync();

        public async Task<Order> GetOrderByIdForEditAsync(string id)
            => await this.orderRepo.All()
                             .Where(o => o.Id == id)
                             .Include(o => o.Product)
                             .ThenInclude(p => p.Category)
                             .FirstOrDefaultAsync();
    }
}
