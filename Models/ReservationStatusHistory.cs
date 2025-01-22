namespace RestaurantManage.Models
{
    public class ReservationStatusHistory
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public DateTime ReserveDate { get; set; }
        public string ReserveTime { get; set; }
        public int Number_of_Persons { get; set; }
        public string Table_Name { get; set; }
        public string TableType { get; set; }
        public int Amount { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
    }
}
