using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Shopping.Rpository.Components
{
    public class BrandsViewComponent : ViewComponent
    {
        private readonly DataContext _dataContext;

        public BrandsViewComponent(DataContext context)
        {
            _dataContext = context;
        }

        //lấy dữ liệu
        public async Task<IViewComponentResult> InvokeAsync() => View(await _dataContext.Brands.ToListAsync());
    }
}
