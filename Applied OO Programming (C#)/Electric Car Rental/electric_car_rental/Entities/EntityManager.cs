using electric_car_rental.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace electric_car_rental.Entities
{
    public class EntityManager : IEntityMaintainer
    {
        public bool Save<T>(Entity item)
        {
            return BaseStorage.Save<T>(item);
        }

        public bool Delete<T>(Entity item)
        {
            return BaseStorage.Delete<T>(item);
        }

        public List<T> All<T>()
        {
            return BaseStorage.All<T>();
        }
    }
}
