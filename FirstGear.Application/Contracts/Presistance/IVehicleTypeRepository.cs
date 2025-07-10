using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstGear.Domain.Models;

namespace FirstGear.Application.Contracts.Presistance
{
    public interface IVehicleTypeRepository : IGenericRepository<VehicleType>
    {
        Task Update(VehicleType vehicleType);
    }
}
