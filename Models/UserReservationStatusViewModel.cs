namespace RestaurantManage.Models
{
    public class UserReservationStatusViewModel
    {
        public DateTime ReserveDate { get; set; }
        public string ReserveTime { get; set; }
        public int Number_of_Persons { get; set; }
        public string Table_Name { get; set; }
        public string TableType { get; set; }
        public int Amount { get; set; }
        public string Status { get; set; }
        public int ReserveId { get; set; }  
    }
}
