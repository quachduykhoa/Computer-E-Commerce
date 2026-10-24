using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Shopping.Rpository.Components
{
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly DataContext _dataContext;

        public CategoriesViewComponent(DataContext context)
        {
            _dataContext = context;
        }

        //lấy dữ liệu
        public async Task<IViewComponentResult> InvokeAsync() => View(await _dataContext.Categories.ToListAsync());
    }
}
