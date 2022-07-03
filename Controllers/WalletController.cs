using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyWallet.Data;
using MyWallet.Model;
using MyWallet.Model.DTOs;
using System.Linq;

namespace MyWallet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly DailyExpensesContext _context;
        private readonly ILogger<WalletController> _logger;
        public WalletController(DailyExpensesContext context, ILogger<WalletController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = _context.Expenses
                                .Select(x => new ExpenseDto
                                {
                                    Description = x.Description,
                                    Amount = x.Amount
                                })
                                .ToList();
            return Ok(result);
        }

        [HttpGet("{Id:int}")]
        public IActionResult Get(int Id)
        {
            var item = _context.Expenses.Find(Id);

            if (item == null)
            {
                _logger.LogInformation($"Daily Expense for Id: {Id} does not exists.");
                return NotFound();
            }

            var result = new Expense
            {
                Description = item.Description,
                Amount = item.Amount
            };

            return Ok(result);
        }

        [HttpPost]
        public IActionResult Post([FromBody] ExpenseDto model)
        {
            if (model == null)
            {
                _logger.LogError("The request object is null");
                return BadRequest();
            }

            if (model.Amount > 200)
            {
                _logger.LogError("The expense amount is not allowed");
                ModelState.AddModelError(string.Empty, "Amount is not allowed");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var newExpense = new Expense
            {
                Description = model.Description,
                Amount = model.Amount
            };
            _context.Expenses.Add(newExpense);
            _context.SaveChanges();

            return Ok();
        }
    }
}
