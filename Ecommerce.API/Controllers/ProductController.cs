using AutoMapper;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Entities.DTO;
using Ecommerce.Core.IRepositories;
using Ecommerce.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        private readonly IUniteOfWork<Products> uniteOfWork;
        private readonly IMapper mapper;

        // private readonly IProductRepository productRepository;
        //   private readonly IGenericRepositories<Products> IgenericRepositories;
        //  private readonly IProductRepository productRepository;
        public ApiResponse apiResponse;
        public ProductController(AppDbContext dbContext ,IUniteOfWork<Products> uniteOfWork , IMapper mapper) {
            this.dbContext = dbContext;
            this.uniteOfWork = uniteOfWork;
            this.mapper = mapper;
            //  this.productRepository = productRepository;
            //       this.IgenericRepositories = IgenericRepositories;
            //  this.productRepository = productRepository;
            apiResponse = new ApiResponse();
        }
        [HttpGet]
        [ResponseCache(CacheProfileName =("defaultCache"))]
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme,Roles ="Admin")]
        public async Task<ActionResult<ApiResponse>> GetAll([FromQuery] string? catName = null,
            int PageSize = 2 , int PageNumber =1)
        {
            Expression<Func<Products, bool>> filter = null;
             if (!string.IsNullOrEmpty(catName))
            {
                filter = x=>x.Category.Name.Contains(catName);
            }
            var model = await uniteOfWork.ProductRepositories.GetAll(filter:filter,page_size : PageSize, page_number : PageNumber , 
                includeProperty :"Category");
            var check = model.Any();
            if (check)
            {
                apiResponse.StatusCode = 200;
                apiResponse.IsSuccess = check;
                var mappingProduct = mapper.Map< IEnumerable<Products>, IEnumerable<ProductDTO> >(model); 
                apiResponse.Result = mappingProduct;
                return apiResponse;
            }
            else
            {
                apiResponse.Message = " not found products .";
                apiResponse.StatusCode = 200;
                apiResponse.IsSuccess = check;
                return apiResponse;
            }
        
        }
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse>> GetByTd([FromQuery]int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new ApiValidationResponse(new List<string> { "Invalid ID", "Try positive integer" }
                    , 400));
                }
                var product = await uniteOfWork.ProductRepositories.GetByID(id);
                var mappingProduct = mapper.Map<Products,ProductDTO>(product);
               
              

                if (product == null)
                {
                   
                    return NotFound(new ApiResponse(404, "Product Not Found"));
                }

                    return Ok(new ApiResponse(200, result: mappingProduct));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiValidationResponse (new List<string> { "internal error server" , ex.Message}));
            }
        }
        [HttpPost]
        public async  Task<ActionResult> CreateProduct(PostProductDTO Newproduct)
        {
            Products product = new Products ()
            { 
                Price = Newproduct.Price,
                Name = Newproduct.Name,
                Image = Newproduct.Image,
                CategoryId = Newproduct.Category_Id
            };

            await uniteOfWork.ProductRepositories.Create(product);
            await uniteOfWork.save();
            return Ok();
        }
        [HttpPut]
        public async Task<ActionResult<ApiResponse>> UpdateProduct(Products Newproduct)
        {
            uniteOfWork.ProductRepositories.Update(Newproduct);
          await   uniteOfWork.save();
            return Ok(Newproduct);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> Delete(int id)
        {
            uniteOfWork.ProductRepositories.Delete(id);
          await   uniteOfWork.save();
            return Ok();
        }
        [HttpGet("Product/{cat_id}")]
        public async Task<ActionResult<ApiResponse>> GetProduct(int cat_id) {
            var products = await uniteOfWork.ProductRepositories.GetAllProductsByCategoryID(cat_id);
            var mappingProducts = mapper.Map<IEnumerable<Products>, IEnumerable<ProductDTO>>(products);
            return Ok(mappingProducts);
        }
        [HttpPost("PostByMapping")]
        public async Task<ActionResult> CreateProductMapping(PostProductDTO Newproduct)
        {
            var product = mapper.Map<PostProductDTO, Products>(Newproduct);
            await uniteOfWork.ProductRepositories.Create(product);
            await uniteOfWork.save();
            return Ok();
        }
 
    }
}
