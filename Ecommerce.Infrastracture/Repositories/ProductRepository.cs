using Ecommerce.Core.Entities;
using Ecommerce.Core.IRepositories;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastracture.Repositories
{
    public class ProductRepository : GenericRepositories<Products>, IProductRepository
    {
        private readonly AppDbContext dbContext;

        public ProductRepository(AppDbContext dbContext):base(dbContext) {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Products>> GetAllProductsByCategoryID(int categoryID)
        {
            
             //  Eager loading
         //    
         //   var products  = (IEnumerable<Products>)await  dbContext.Products
         //       .Include(x=>x.Category)
         //       .Where(c=>c.CategoryId == categoryID)
         //       .ToListAsync();
         //   return  products;
            

            //  Explicit loading

        //        var products = await dbContext.Products.Where(c=>c.CategoryId == categoryID).ToListAsync();
        //        foreach (var product in products)
        //        {
        //            await dbContext.Entry(product).Reference(c=>c.Category).LoadAsync();
        //        }
        //        return products;
             


            // lazy Loading
            var products = await dbContext.Products
                .Where(c=>c.CategoryId == categoryID).ToListAsync();
            return products;



        }

    }
}
