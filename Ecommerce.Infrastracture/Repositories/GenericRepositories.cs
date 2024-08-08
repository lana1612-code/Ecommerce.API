using Ecommerce.Core.Entities;
using Ecommerce.Core.IRepositories;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastracture.Repositories
{
    public class GenericRepositories<T> : IGenericRepositories<T> where T : class
    {
        private readonly AppDbContext dbContext;

        public GenericRepositories(AppDbContext dbContext) {
            this.dbContext = dbContext;
        }
        public async Task Create(T model)
        {
          await  dbContext.Set<T>().AddAsync(model);
        }

        public void Delete(int id)
        {
            dbContext.Remove(id);
        }

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter, int page_size , int page_number , string? includeProperte = null)
        {
            /*
              if( typeof(T) == typeof(Products) )
              {
                  var model =  await dbContext.Products.Include(x=>x.Category).ToListAsync();
                  return (IEnumerable<T>)model;
              } 
            */
            IQueryable<T> query = dbContext.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if(includeProperte != null)
            {
                foreach(var property in includeProperte.Split(new char[] { ',' } ,
                    StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            
            if(page_size > 0)
            {
                if(page_size>4 )
                {
                    page_size = 4;
                }
                query = query.Skip(page_size * (page_number -1)).Take(page_size);
            }


         return await   query.ToListAsync();
        }

        public async Task<T> GetByID(int id)
        {
            return await dbContext.Set<T>().FindAsync(id);
        }
        

        public void Update(T model)
        {
            dbContext.Set<T>().Update(model);
        }
    }
}
