using API2.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API2.Controllers
{
  // External API
  [Route("api/[controller]")] // api/products
  [ApiController]
  public class ProductsController : ControllerBase
  {


    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
            //Timeout testi
            //Thread.Sleep(3500); //Main Thread kitlendi.
            try
            {
                //await Task.Delay(3500);
                var plist = new List<ProductDto>
                {
                   new ProductDto("P-1", 10, 12m),
                   new ProductDto("P-2", 10, 13m)
                };

                // Circuit Braker Patter simüle edelim. custom Exxception fırlatalım

                
                return Ok(plist);
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
    }

  }
}
