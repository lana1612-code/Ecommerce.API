using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.IRepositories
{
    public interface IUniteOfWork<T>where T :class
    {
        public ICategoryRepositories CategoryRepositories { get; set; }
        public IProductRepository ProductRepositories { get; set; }
        public IOrderRepositories OrderRepositories { get; set; }
        public Task<int> save();
    }
}
