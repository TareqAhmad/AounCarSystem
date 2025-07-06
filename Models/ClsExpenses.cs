namespace AounCarSystem.Models
{
    public class ClsExpenses
    {

        public int Expense_Id { get; set; }

        public string ExpenseType { get; set; }

        public decimal Amount { get; set; }

        public string ExpenseDate { get; set; }

        public string Description { get; set; }

        public int ChassisNumber { get; set; }

        public string CarName { get; set; }

    }
}
