namespace AounCarSystem.Models
{
    public class ClsMasterSalesInvoice
    {
        public int Inv_Id { get; set; }

        public decimal Inv_Value { get; set; }

        public string Inv_Date { get; set; }

        public string Payment_type { get; set; }

        public int Cust_Id { get; set; }

        public string CustomerName { get; set; }

        public List<ClsDetailsSalesInvoice> Details { get; set; } = new List<ClsDetailsSalesInvoice>();


    }
}
