using System.ComponentModel.DataAnnotations;

namespace Shopping.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Làm ơn nhập UserName")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Làm ơn nhập Email"),EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password),Required(ErrorMessage ="Làm ơn nhập Password")]// mã hóa pass
        public string Password { get; set; }

    }
}
