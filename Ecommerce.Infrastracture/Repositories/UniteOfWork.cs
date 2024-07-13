using Ecommerce.Core.IRepositories;
using Ecommerce.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastracture.Repositories
{
    public class UniteOfWork<T> : IUniteOfWork<T> where T : class
    {
        private readonly AppDbContext dbContext;

        public UniteOfWork(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
            ProductRepositories = new ProductRepository(dbContext);
            OrderRepositories = new OrderRepositories(dbContext);
        }
        public ICategoryRepositories CategoryRepositories { get; set; }
        public IProductRepository ProductRepositories { get; set; }
        public IOrderRepositories OrderRepositories { get; set; }

        public async Task<int> save()
        {
          return await  dbContext.SaveChangesAsync();
        }
    }
    
    
}
