using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public ProductsController(IGenericRepository<Product> ProductRep)
        {
            _productRep = ProductRep;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
        {
            var spec = new ProductWithBrandAndCategorySpecifications(); 
            var result = await _productRep.GetAllWithSpecAsync(spec);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            var spec = new ProductWithBrandAndCategorySpecifications(id);
            var product = await _productRep.GetWithSpecAsync(spec);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

    }

}
