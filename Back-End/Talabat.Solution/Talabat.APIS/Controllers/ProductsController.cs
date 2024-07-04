using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Talabat.APIS.DTOS;
using Talabat.APIS.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications.ProductSpecs;
using Talabat.DataAcces;

namespace Talabat.APIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<Product> _productRep;
        private readonly IGenericRepository<ProductBrand> _brandsRepo;
        private readonly IGenericRepository<ProductCategory> _categorysRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> ProductRep 
            , IGenericRepository<ProductBrand> brandsRepo
            , IGenericRepository<ProductCategory> categorysRepo
            , IMapper mapper)
        {
            _productRep = ProductRep;
            _brandsRepo = brandsRepo;
            _categorysRepo = categorysRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts(string sort)
        {
            var spec = new ProductWithBrandAndCategorySpecifications(sort); 
            var result = await _productRep.GetAllWithSpecAsync(spec);
            return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(result));

        }

        //Improving Swagger Documentation
        [ProducesResponseType(typeof(ProductToReturnDto) , StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse) , StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetById(int id)
        {
            var spec = new ProductWithBrandAndCategorySpecifications(id);
            var product = await _productRep.GetWithSpecAsync(spec);
            if (product == null)
                return NotFound(new ApiResponse(400));
            return Ok(_mapper.Map<Product , ProductToReturnDto>(product) );
        }


        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await _brandsRepo.GetAllAsync();
            return Ok(brands);
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategories()
        {
            var categories = await _categorysRepo.GetAllAsync();
            return Ok(categories);
        }
    }

}