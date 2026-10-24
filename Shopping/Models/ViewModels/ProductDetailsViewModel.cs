using System.ComponentModel.DataAnnotations;

namespace Shopping.Models.ViewModels
{
    public class ProductDetailsViewModel
    {
        public ProductModel ProductDetails { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập bình luận Sản phẩm")]
        public string Comment { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập tên Sản phẩm")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập email Sản phẩm")]
        public string Email { get; set; }

    }
}
