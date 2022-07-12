using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyWallet.Data;
using MyWallet.Model;
using MyWallet.Model.DTOs;
using System.Linq;
using System.Threading.Tasks;

namespace MyWallet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly DailyExpensesContext _context;
        private readonly Serilog.ILogger _logger;
        public WalletController(DailyExpensesContext context, ILogger<WalletController> logger, Serilog.ILogger serilogLogger)
        {
            _context = context;
            _logger = serilogLogger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _context.Expenses
                                .Select(x => new ExpenseDto
                                {
                                    Description = x.Description,
                                    Amount = x.Amount
                                })
                                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("{Id:int}")]
        public async Task<IActionResult> Get(int Id)
        {
            var item = await _context.Expenses.FindAsync(Id);

            if (item == null)
            {
                _logger.Information($"Daily Expense for Id: {Id} does not exists.");
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
        public async Task<IActionResult> Post([FromBody] ExpenseDto model)
        {
            if (model == null)
            {
                _logger.Error("The request object is null");
                return BadRequest();
            }

            if (model.Amount > 200)
            {
                _logger.Error("The expense amount is not allowed");
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
            await _context.Expenses.AddAsync(newExpense);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
