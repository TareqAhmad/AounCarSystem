namespace AounCarSystem.Models
{
    public class ClsCarMovements
    {

        public int Move_Id { get; set; }

        public string FromLocation { get; set; }

        public string ToLocation { get; set; }

        public string MovementDate { get; set; }

        public string Reason { get; set; }

        public ClsCars ChassisNumber { get; set; }










    }
}
