namespace AounCarSystem.Models
{
    public class ClsCars
    {

        public int ChassisNumber { get; set; }

        public string CarName { get; set; }

        public string CarType { get; set; }

        public string CarModel { get; set; }

        public decimal CarPrice { get; set; }

        public string Status { get; set; }

        public ClsSuppliers Supplier_Id { get; set; }




    }
}
