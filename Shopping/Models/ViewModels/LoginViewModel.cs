using System.ComponentModel.DataAnnotations;

namespace Shopping.Models.ViewModels
{
    public class LoginViewModel
    {
        public int Id { get; set; }
   
        [Required(ErrorMessage = "Làm ơn nhập UserName")]
        public string Username { get; set; }
   
        [DataType(DataType.Password), Required(ErrorMessage = "Làm ơn nhập Password")]// mã hóa pass
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
