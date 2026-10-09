using System;

namespace BudgetTracker
{
    public class Transaction
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }      // "Income" or "Expense"
        public decimal Amount { get; set; }
    }
}