using Ecommerce.Core.Entities;
using Ecommerce.Core.IRepositories;
using Ecommerce.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        private readonly IGenericRepositories<Categories> igenericRepositories;

        public CategoryController(AppDbContext dbContext , IGenericRepositories<Categories>IgenericRepositories)
        {
            this.dbContext = dbContext;
            igenericRepositories = IgenericRepositories;
        }

    }
}
