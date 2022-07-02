using Microsoft.EntityFrameworkCore;
using MyWallet.Model;

namespace MyWallet.Data
{
    public class DailyExpensesContext: DbContext
    {
        public DailyExpensesContext(DbContextOptions<DailyExpensesContext> contextOptions): base(contextOptions) { }
        public DailyExpensesContext() { }
        public DbSet<Expense> Expenses { get; set; }

        
    }
}
