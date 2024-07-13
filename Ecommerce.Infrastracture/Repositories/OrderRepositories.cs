using Ecommerce.Core.Entities;
using Ecommerce.Core.IRepositories;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastracture.Repositories
{
    public class OrderRepositories : GenericRepositories<Orders>, IOrderRepositories
    {
        private readonly AppDbContext dbContext;

        public OrderRepositories(AppDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Orders>> GetAllOrderByUserID(int userID)
        {
            var orders = await dbContext.Orders
                .Where(o=>o.LocalUserId == userID).ToListAsync();
            return orders;
           
        }

    }
}
