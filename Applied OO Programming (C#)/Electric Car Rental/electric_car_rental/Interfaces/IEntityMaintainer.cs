using electric_car_rental.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace electric_car_rental.Interfaces
{
    interface IEntityMaintainer : IMaintainer
    {
        bool Save<T>(Entity item);
    }
}
