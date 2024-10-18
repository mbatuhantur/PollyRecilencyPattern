using Microsoft.AspNetCore.Mvc;

namespace API_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : Controller
    {
        private readonly HttpClient api2;


        public TestsController(IHttpClientFactory httpClientFactory)
        {
            api2 = httpClientFactory.CreateClient("api2");
        }
        [HttpGet]
        public async Task<IActionResult> GetRequestAsync()
        {
            // /api/products endpoint istek at.
            var response = await api2.GetStringAsync("/api/products");

            return Ok(response);
        }
    }
}
