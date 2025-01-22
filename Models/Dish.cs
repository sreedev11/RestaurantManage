using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManage.Models
{
    public class Dish
    {
        [Key]
        public int Id { get; set; }
        public string? DishName { get; set; } 
        public int Price { get; set; }
        public int CategoryId { get; set; }

        public virtual Category? Category { get; set; }
    }
}
