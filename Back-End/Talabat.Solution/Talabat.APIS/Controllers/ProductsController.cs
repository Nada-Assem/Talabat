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
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> ProductRep , IMapper mapper)
        {
            _productRep = ProductRep;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductToReturnDto>>> GetProduct()
        {
            var spec = new ProductWithBrandAndCategorySpecifications(); 
            var result = await _productRep.GetAllWithSpecAsync(spec);
            return Ok(_mapper.Map<IEnumerable<Product>, IEnumerable<ProductToReturnDto>>(result));

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

    }

}
