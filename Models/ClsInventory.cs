namespace AounCarSystem.Models
{
    public class ClsInventory
    {

        public int Inventory_Id { get; set; }

        public ClsCars ChassisNumber { get; set; }

        public string Location { get; set; }

        public int Quantity { get; set; }

        public string Last_Update { get; set; }




    }
}
