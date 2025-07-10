using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FirstGear.Domain.Models.ViewModel
{
    public class PostVM
    {
        public Post Post { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> BrandList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> VehicleTypeList { get; set; }

        public IEnumerable<SelectListItem> EngineAndFuelTypeList { get; set; }

        public IEnumerable<SelectListItem> TransmissionList { get; set; }
    }
}
