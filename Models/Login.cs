using System.ComponentModel.DataAnnotations;

namespace RestaurantManage.Models
{
    public class Login
    { 
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
