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
      Thread.Sleep(3500); //Main Thread kitlendi.

      var plist = new List<ProductDto>();
      plist.Add(new ProductDto("P-1", 10, 12));
      plist.Add(new ProductDto("P-2", 10, 13));

     // Circuit Braker Patter simüle edelim. custom Exxception fırlatalım

     throw new Exception("Hata");

      return Ok(plist);
    }

  }
}
