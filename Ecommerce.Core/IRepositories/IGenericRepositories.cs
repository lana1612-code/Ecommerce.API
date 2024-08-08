using Ecommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.IRepositories
{
    public interface IGenericRepositories<T>where T : class
    {
        public Task<IEnumerable<T>> GetAll(Expression<Func<T,bool>> filter,int page_size , int page_number , string? includeProperty = null);
        public Task<T> GetByID(int id);
        public Task Create(T model);
        public void Update(T model);
        public void Delete(int id);
    }
}
