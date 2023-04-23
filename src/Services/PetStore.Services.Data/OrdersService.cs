﻿namespace PetStore.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using PetStore.Data.Common.Repositories;
    using PetStore.Data.Models;
    using PetStore.Services.Data.Contracts;
    using PetStore.Web.ViewModels.Orders;

    public class OrdersService : IOrdersService
    {
        private readonly IDeletableEntityRepository<Order> orderRepo;

        public OrdersService(IDeletableEntityRepository<Order> orderRepo)
            => this.orderRepo = orderRepo;

        public async Task AddOrderAsync(OrderDetailsViewModel orderInfo)
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
            };

            await this.orderRepo.AddAsync(order);
            await this.orderRepo.SaveChangesAsync();
        }

        public IQueryable<Order> GetAllClientsOrders(string clientId)
            => this.orderRepo.AllAsNoTracking()
                             .Where(o => o.ApplicationUserId == clientId)
                             .Include(o => o.Product)
                             .OrderByDescending(o => o.CreatedOn);
    }
}
