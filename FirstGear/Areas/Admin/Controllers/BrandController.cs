using FirstGear.Infrastructure.Common;
using FirstGear.Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FirstGear.Application.ApplicationConstant;
using FirstGear.Application.Contracts.Presistance;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using FirstGear.Domain.ApplicationEnum;
using Microsoft.AspNetCore.Authorization;


namespace FirstGear.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = CustomRole.MasterAdmin + "," + CustomRole.Admin)]
    public class BrandController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<BrandController> _logger;


        public BrandController(IUnitOfWork unitOfWork , IWebHostEnvironment webHostEnvironment , ILogger<BrandController> logger)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                List<Brand> brands = await _unitOfWork.Brand.GetAllAsync();

                _logger.LogInformation("Brand List fetched Succesfully!!");
                return View(brands);

            }
            catch (Exception ex)
            {
                _logger.LogError("Something went Wrong");
                return View();
                

            }
        }
        public IActionResult Create()
        { 
            return View();
        }

        [HttpPost]
         
        public async Task<IActionResult> Create(Brand brand)

        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            var file = HttpContext.Request.Form.Files;

            

            if (file.Count > 0)
            {
                string newFileName = Guid.NewGuid().ToString();

                var upload = Path.Combine(webRootPath, @"Images\brand");

                var extension = Path.GetExtension(file[0].FileName);

                using (var fileStream = new FileStream(Path.Combine(upload, newFileName + extension), FileMode.Create))
                {
                    file[0].CopyTo(fileStream);
                }

                brand.BrandLogo = @"\Images\brand\" + newFileName + extension;
            }

            brand.CreatedOn = DateTime.Now;

            if (ModelState.IsValid)
            {
                await _unitOfWork.Brand.Create(brand);
                await _unitOfWork.SaveAsync();

                TempData["success"] = CommonMessage.RecordCreated;

                return RedirectToAction(nameof(Index));
            }



            return View();
        }

        [HttpGet]

        public async Task<IActionResult> Details(Guid id)
        {
            Brand brand = await _unitOfWork.Brand.GetByIdAsync(id);

            return View(brand);


        }

        [HttpGet]

        public async Task<IActionResult> Edit(Guid id)
        {
            Brand brand = await _unitOfWork.Brand.GetByIdAsync(id);

            return View(brand);


        }

        [HttpPost]

        public async Task <IActionResult> Edit(Brand brand)
        {


            string webRootPath = _webHostEnvironment.WebRootPath;
            var file = HttpContext.Request.Form.Files;

            if (file.Count > 0)
            {
                string newFileName = Guid.NewGuid().ToString();
                var upload = Path.Combine(webRootPath, @"Images\brand");

                var extension = Path.GetExtension(file[0].FileName);

                //Delete Old Image

                var objFromdb = await _unitOfWork.Brand.GetByIdAsync(brand.Id);
                if (objFromdb.BrandLogo != null)
                {
                    var oldImagePath = Path.Combine(webRootPath, objFromdb.BrandLogo.Trim('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }


                }

                using (var fileStream = new FileStream(Path.Combine(upload, newFileName + extension), FileMode.Create))
                {
                    file[0].CopyTo(fileStream);
                }

                brand.BrandLogo = @"\Images\brand\" + newFileName + extension;
            }

            if (ModelState.IsValid)
            {
                await _unitOfWork.Brand.Update(brand);
                await _unitOfWork.SaveAsync();

                TempData["warning"] = CommonMessage.RecordUpdated;

                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpGet]

        public async Task<IActionResult> Delete(Guid id)
        {
            Brand brand = await _unitOfWork.Brand.GetByIdAsync(id);

            return View(brand);


        }

        [HttpPost]

        public async Task <IActionResult> Delete (Brand brand)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;

            if(string.IsNullOrEmpty(brand.BrandLogo))
            {
                var objFromdb = await _unitOfWork.Brand.GetByIdAsync(brand.Id);
                if (objFromdb.BrandLogo != null)
                {
                    var oldImagePath = Path.Combine(webRootPath, objFromdb.BrandLogo.Trim('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }


                }

            }

            await _unitOfWork.Brand.Delete(brand);
            await _unitOfWork.SaveAsync();

            TempData["error"] = CommonMessage.RecordDeleted;

            return RedirectToAction(nameof(Index));

        }





    }
}
