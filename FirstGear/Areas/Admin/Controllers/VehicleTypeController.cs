using FirstGear.Infrastructure.Common;
using FirstGear.Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FirstGear.Application.ApplicationConstant;
using FirstGear.Application.Contracts.Presistance;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;


namespace FirstGear.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = CustomRole.MasterAdmin + "," + CustomRole.Admin)]
    public class VehicleTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
       

        public VehicleTypeController(IUnitOfWork unitOfWork , IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<VehicleType> vehicleTypes =  await _unitOfWork.VehicleType.GetAllAsync();

            return View(vehicleTypes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
         
        public async Task<IActionResult> Create(VehicleType vehicleType)

        {

            if (ModelState.IsValid)
            {
                await _unitOfWork.VehicleType.Create(vehicleType);
                await _unitOfWork.SaveAsync();

                TempData["success"] = CommonMessage.RecordCreated;

                return RedirectToAction(nameof(Index));
            }



            return View();
        }

        [HttpGet]

        public async Task<IActionResult> Details(Guid id)
        {
            VehicleType vehicleType = await _unitOfWork.VehicleType.GetByIdAsync(id);

            return View(vehicleType);


        }

        [HttpGet]

        public async Task<IActionResult> Edit(Guid id)
        {
            VehicleType vehicleType = await _unitOfWork.VehicleType.GetByIdAsync(id);

            return View(vehicleType);


        }

        [HttpPost]

        public async Task <IActionResult> Edit(VehicleType vehicleType)
        {

            if (ModelState.IsValid)
            {
                await _unitOfWork.VehicleType.Update(vehicleType);
                await _unitOfWork.SaveAsync();

                TempData["warning"] = CommonMessage.RecordUpdated;

                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpGet]

        public async Task<IActionResult> Delete(Guid id)
        {
            VehicleType vehicleType = await _unitOfWork.VehicleType.GetByIdAsync(id);

            return View(vehicleType);


        }

        [HttpPost]

        public async Task <IActionResult> Delete (VehicleType vehicleType)
        {
            

            await _unitOfWork.VehicleType.Delete(vehicleType);
            await _unitOfWork.SaveAsync();

            TempData["error"] = CommonMessage.RecordDeleted;

            return RedirectToAction(nameof(Index));

        }





    }
}
