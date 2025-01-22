using System;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManage.Models
{
    public class Reservation
    {
        [Key]
        public int ReserveId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ReserveDate { get; set; }

        [Required]
        [StringLength(20)]
        public string ReserveTime { get; set; }

        [Required]
        public int Number_of_Persons { get; set; }

        [Required]
        [StringLength(20)]
        public string Table { get; set; }
        [Required]
        public string TableType { get; set; }
        [Required]
        public int Amount {  get; set; }

        [Required]
        public int ID { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending";

        public DateTime CreatedDate { get; set; }
    }
}
