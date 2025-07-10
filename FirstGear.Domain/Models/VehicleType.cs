using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstGear.Domain.Common;

namespace FirstGear.Domain.Models
{
    public class VehicleType : BaseModel
    {
        [Required]
        public string Name { get; set; }
    }
}
