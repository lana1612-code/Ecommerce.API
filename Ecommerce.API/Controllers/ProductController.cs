using AutoMapper;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Entities.DTO;
using Ecommerce.Core.IRepositories;
using Ecommerce.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<ActionResult<ApiResponse>> GetAll()
        {
            var model = await uniteOfWork.ProductRepositories.GetAll();
            var check = model.Any();
            if (check)
            {
                apiResponse.httpStatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = check;
                var mappingProduct = mapper.Map< IEnumerable<Products>, IEnumerable<ProductDTO> >(model); 
                apiResponse.Result = mappingProduct;
                return apiResponse;
            }
            else
            {
                apiResponse.ErrorMessage = " not found products .";
                apiResponse.httpStatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = check;
                return apiResponse;
            }
        
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetByTd(int id)
        {
           var product = await uniteOfWork.ProductRepositories.GetByID(id);
            return Ok(product);
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
