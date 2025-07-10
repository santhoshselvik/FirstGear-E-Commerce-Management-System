using FirstGear.Infrastructure.Common;
using FirstGear.Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FirstGear.Application.ApplicationConstant;
using FirstGear.Application.Contracts.Presistance;
using System.Threading.Tasks;
using FirstGear.Domain.ApplicationEnum;
using Microsoft.AspNetCore.Mvc.Rendering;
using FirstGear.Domain.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using FirstGear.Application.Interface;



namespace FirstGear.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = CustomRole.MasterAdmin + "," + CustomRole.Admin)]
    public class PostController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUserNameService _userName;
       

        public PostController(IUnitOfWork unitOfWork , IWebHostEnvironment webHostEnvironment , IUserNameService userName)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _userName = userName;

        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Post> posts =  await _unitOfWork.Post.GetAllPost();

            return View(posts);
        }

        public IActionResult Create()
        {

            IEnumerable<SelectListItem> brandList = _unitOfWork.Brand.Query().Select(
                x => new SelectListItem
                {
                    Text = x.Name.ToUpper(),
                    Value = x.Id.ToString()
                });
            IEnumerable<SelectListItem> vehicleTypeList = _unitOfWork.VehicleType.Query().Select(
                x => new SelectListItem
                {
                    Text = x.Name.ToUpper(),
                    Value = x.Id.ToString()
                });


            IEnumerable<SelectListItem> engineAndFuelType = Enum.GetValues(typeof(EngineAndFuelType)).Cast<EngineAndFuelType>().Select(
                x => new SelectListItem
                {
                    Text = x.ToString().ToUpper(),
                    Value = ((int)x).ToString()
                });

            IEnumerable<SelectListItem> transmission = Enum.GetValues(typeof(EngineAndFuelType)).Cast<Transmission>().Select(
                x => new SelectListItem
                {
                    Text = x.ToString().ToUpper(),
                    Value = ((int)x).ToString()
                });

            PostVM postVM = new PostVM()
            {
                Post = new Post(),
                BrandList = brandList,
                VehicleTypeList = vehicleTypeList,
                EngineAndFuelTypeList = engineAndFuelType,
                TransmissionList = transmission
            };
            return View(postVM);
        }

        [HttpPost]
         
        public async Task<IActionResult> Create(PostVM postVM)

        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            var file = HttpContext.Request.Form.Files;

            

            if (file.Count > 0)
            {
                string newFileName = Guid.NewGuid().ToString();

                var upload = Path.Combine(webRootPath, @"Images\post");

                var extension = Path.GetExtension(file[0].FileName);

                using (var fileStream = new FileStream(Path.Combine(upload, newFileName + extension), FileMode.Create))
                {
                    file[0].CopyTo(fileStream);
                }

            
                postVM.Post.VehicleImage = @"\Images\post\" + newFileName + extension;
            }

            postVM.Post.CreatedOn = DateTime.Now;

            if (ModelState.IsValid)
            {
                await _unitOfWork.Post.Create(postVM.Post);
                await _unitOfWork.SaveAsync();

                TempData["success"] = CommonMessage.RecordCreated;

                return RedirectToAction(nameof(Index));
            }



            return View();
        }

        [HttpGet]

        public async Task<IActionResult> Details(Guid id)
        {
            Post post = await _unitOfWork.Post.GetPostById(id);
          

            post.CreatedBy = await _userName.GetUserName(post.CreatedBy);
            post.ModifiedBy = await _userName.GetUserName(post.ModifiedBy);



            return View(post);


        }

        [HttpGet]

        public async Task<IActionResult> Edit(Guid id)
        {
            Post post = await _unitOfWork.Post.GetByIdAsync(id);


            IEnumerable<SelectListItem> brandList = _unitOfWork.Brand.Query().Select(
                x => new SelectListItem
                {
                    Text = x.Name.ToUpper(),
                    Value = x.Id.ToString()
                });
            IEnumerable<SelectListItem> vehicleTypeList = _unitOfWork.VehicleType.Query().Select(
                x => new SelectListItem
                {
                    Text = x.Name.ToUpper(),
                    Value = x.Id.ToString()
                });


            IEnumerable<SelectListItem> engineAndFuelType = Enum.GetValues(typeof(EngineAndFuelType)).Cast<EngineAndFuelType>().Select(
                x => new SelectListItem
                {
                    Text = x.ToString().ToUpper(),
                    Value = ((int)x).ToString()
                });

            IEnumerable<SelectListItem> transmission = Enum.GetValues(typeof(EngineAndFuelType)).Cast<Transmission>().Select(
                x => new SelectListItem
                {
                    Text = x.ToString().ToUpper(),
                    Value = ((int)x).ToString()
                });

            PostVM postVM = new PostVM()
            {
                Post =  post,
                BrandList = brandList,
                VehicleTypeList = vehicleTypeList,
                EngineAndFuelTypeList = engineAndFuelType,
                TransmissionList = transmission
            };
            return View(postVM);


        }

        [HttpPost]

        public async Task <IActionResult> Edit(PostVM postVM)
        {


            string webRootPath = _webHostEnvironment.WebRootPath;
            var file = HttpContext.Request.Form.Files;

            if (file.Count > 0)
            {
                string newFileName = Guid.NewGuid().ToString();
                var upload = Path.Combine(webRootPath, @"Images\post");

                var extension = Path.GetExtension(file[0].FileName);

                //Delete Old Image

                var objFromdb = await _unitOfWork.Post.GetByIdAsync(postVM.Post.Id);
                if (objFromdb.VehicleImage != null)
                {
                    var oldImagePath = Path.Combine(webRootPath, objFromdb.VehicleImage.Trim('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }


                }

                using (var fileStream = new FileStream(Path.Combine(upload, newFileName + extension), FileMode.Create))
                {
                    file[0].CopyTo(fileStream);
                }

                postVM.Post.VehicleImage = @"\Images\post\" + newFileName + extension;
            }

            if (ModelState.IsValid)
            {
                await _unitOfWork.Post.Update(postVM.Post);
                await _unitOfWork.SaveAsync();

                TempData["warning"] = CommonMessage.RecordUpdated;

                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpGet]

        public async Task<IActionResult> Delete(Guid id)
        {
            Post post = await _unitOfWork.Post.GetByIdAsync(id);

            IEnumerable<SelectListItem> brandList = _unitOfWork.Brand.Query().Select(
               x => new SelectListItem
               {
                   Text = x.Name.ToUpper(),
                   Value = x.Id.ToString()
               });
            IEnumerable<SelectListItem> vehicleTypeList = _unitOfWork.VehicleType.Query().Select(
                x => new SelectListItem
                {
                    Text = x.Name.ToUpper(),
                    Value = x.Id.ToString()
                });


            IEnumerable<SelectListItem> engineAndFuelType = Enum.GetValues(typeof(EngineAndFuelType)).Cast<EngineAndFuelType>().Select(
                x => new SelectListItem
                {
                    Text = x.ToString().ToUpper(),
                    Value = ((int)x).ToString()
                });

            IEnumerable<SelectListItem> transmission = Enum.GetValues(typeof(EngineAndFuelType)).Cast<Transmission>().Select(
                x => new SelectListItem
                {
                    Text = x.ToString().ToUpper(),
                    Value = ((int)x).ToString()
                });

            PostVM postVM = new PostVM()
            {
                Post = post,
                BrandList = brandList,
                VehicleTypeList = vehicleTypeList,
                EngineAndFuelTypeList = engineAndFuelType,
                TransmissionList = transmission
            };

            return View(postVM);


        }

        [HttpPost]

        public async Task <IActionResult> Delete (PostVM postVM)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;

            if(string.IsNullOrEmpty(postVM.Post.VehicleImage))
            {
                var objFromdb = await _unitOfWork.Post.GetByIdAsync(postVM.Post.Id);
                if (objFromdb.VehicleImage != null)
                {
                    var oldImagePath = Path.Combine(webRootPath, objFromdb.VehicleImage.Trim('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }


                }

            }

            await _unitOfWork.Post.Delete(postVM.Post);
            await _unitOfWork.SaveAsync();

            TempData["error"] = CommonMessage.RecordDeleted;

            return RedirectToAction(nameof(Index));

        }





    }
}
