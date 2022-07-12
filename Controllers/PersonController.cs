using GenFu;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWallet.Model;

namespace MyWallet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var list = A.ListOf<Person>(200);
            return Ok(list);
        }
    }
}
