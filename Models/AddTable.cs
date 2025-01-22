using System.ComponentModel.DataAnnotations;

namespace RestaurantManage.Models
{
    public class AddTable
    {
        [Key]
        public int TableId { get; set; }
        public string TableName { get; set; }
        public string TableType { get; set; }
        public int Amount {  get; set; }

    }
}
