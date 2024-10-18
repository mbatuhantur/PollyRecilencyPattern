using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API2.Controllers
{
    //External API
    [Route("api/[controller]")] // api/products
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            return Ok("Products");
        }
    }
}
