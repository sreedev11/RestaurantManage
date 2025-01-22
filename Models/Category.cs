using System.ComponentModel.DataAnnotations;

namespace RestaurantManage.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; } = string.Empty; 
        public byte[]? Logo { get; set; } 
    }
}
