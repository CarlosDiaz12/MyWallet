using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWallet.Data;
using MyWallet.ErrorHandlers;
using System.Net;

namespace MyWallet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewWalletController : ControllerBase
    {
        private readonly DailyExpensesContext _context;
        public NewWalletController(DailyExpensesContext context)
        {
            _context = context;
        }

        [HttpGet("{Id}")]
        public IActionResult Get(int Id)
        {
            var item = _context.Expenses.Find(Id);
            if (item == null) throw new WebAPIException($"Daily Expense for Id: {Id} does not exists", HttpStatusCode.NotFound);

            return Ok(item);
        }

    }
}
