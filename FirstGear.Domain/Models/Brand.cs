using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstGear.Domain.Common;
using Microsoft.AspNetCore.Http;


namespace FirstGear.Domain.Models
{
    public class Brand : BaseModel
    {
        

        [Required]
        public string Name { get; set; }

        [Display(Name = " Established Year")]
        public int EstablishedYear { get; set; }

        [Display(Name = " Brand Logo ")]
        public string BrandLogo { get; set; }

      
       

    }
}
