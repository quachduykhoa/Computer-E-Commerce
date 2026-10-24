using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shopping.Models;
using Shopping.Rpository;

namespace Shopping.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Slider")]
    [Authorize]
    public class SliderController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SliderController(DataContext context, IWebHostEnvironment webHostEnvironment)
        {
            _dataContext = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Silders.OrderByDescending(p => p.Id).ToListAsync());
        }

        [Route("Create")]
        public IActionResult Create()
        {
            
            return View();
        }

        [Route("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderModel slider)
        {
            
            if (ModelState.IsValid)
            {
                //code thêm dữ liệu
                if (slider.ImageUpLoad != null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/sliders");
                    string imageName = Guid.NewGuid().ToString() + "_" + slider.ImageUpLoad.FileName;
                    string filePath = Path.Combine(uploadDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await slider.ImageUpLoad.CopyToAsync(fs);
                    fs.Close();
                    slider.Image = imageName;

                }

                _dataContext.Add(slider);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Thêm slider thành công";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Model có một vài thứ đang bị lỗi";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }

            return View(slider);
        }

        [Route("Edit")]
        public async Task<IActionResult> Edit(int Id)
        {
            SliderModel slider = await _dataContext.Silders.FindAsync(Id);

           
            return View(slider);
        }

        [Route("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SliderModel slider)
        {
            var slider_existed = _dataContext.Silders.Find(slider.Id);

            if (ModelState.IsValid)
            {
                //code thêm dữ liệu
                if (slider.ImageUpLoad != null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/sliders");
                    string imageName = Guid.NewGuid().ToString() + "_" + slider.ImageUpLoad.FileName;
                    string filePath = Path.Combine(uploadDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await slider.ImageUpLoad.CopyToAsync(fs);
                    fs.Close();
                    slider_existed.Image = imageName;

                }
                slider_existed.Name = slider.Name;
                slider_existed.Description = slider.Description;
                slider_existed.Status = slider.Status;


                _dataContext.Update(slider_existed);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Cập nhật slider thành công";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Model có một vài thứ đang bị lỗi";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }

            return View(slider);
        }
        
        [Route("Delete")]        
        public async Task<IActionResult> Delete(int Id)
        {
            SliderModel slider = await _dataContext.Silders.FindAsync(Id);

            string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/sliders");
            string oldfileImage = Path.Combine(uploadDir, slider.Image);
            if (System.IO.File.Exists(oldfileImage))         //tồn tại
            {
                System.IO.File.Delete(oldfileImage);
            }

            _dataContext.Silders.Remove(slider);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Slider đã xóa";
            return RedirectToAction("Index");
        }
    }
}
