using AutoMapper;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Entities.DTO;
using Ecommerce.Core.IRepositories;
using Ecommerce.Infrastracture.Repositories;
using Ecommerce.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        private readonly IUniteOfWork<Products> uniteOfWork;
        private readonly IMapper mapper;
        public ApiResponse apiResponse;
        public OrderController(AppDbContext dbContext, IUniteOfWork<Products> uniteOfWork, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.uniteOfWork = uniteOfWork;
            this.mapper = mapper;
            apiResponse = new ApiResponse();
        }

        [HttpGet("Order/{user_id}")]
        public async Task<ActionResult<ApiResponse>> GetOrder(int user_id)
        {
            var orders = await uniteOfWork.OrderRepositories.GetAllOrderByUserID(user_id);
            var mapping = mapper.Map< IEnumerable<Orders> , IEnumerable<OrderDTO> >(orders);
            return Ok(mapping);
        }
    }
}
