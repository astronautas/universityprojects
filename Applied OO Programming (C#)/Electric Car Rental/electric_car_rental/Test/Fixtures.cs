using electric_car_rental.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace electric_car_rental.Test
{
    class Fixtures
    {
        protected static String projectDocPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                                                 + $@"\{ConfigurationManager.AppSettings["project_name"]}\";

        public static void Load()
        {
            ElectricCars();
            Users();
            Stops();
        }

        private static void ElectricCars()
        {
            System.IO.File.WriteAllText(projectDocPath + "electriccars.xml", string.Empty);

            new ElectricCar("Toyota Yaris", 400, 10, 50, 100, 10).Save();
            new ElectricCar("Tesla Model S", 300, 5, 100, 100, 15).Save();
            new ElectricCar("Tesla Model X", 200, 3, 110, 100, 15).Save();
            new ElectricCar("BMW i3", 150, 8, 70, 100, 10).Save();
        }

        private static void Users()
        {
            System.IO.File.WriteAllText(projectDocPath + "users.xml", string.Empty);

            new User("lukas@valatka.net", "Lukas Valatka", 0, "123", true).Save();
            new User("test@gmail.com", "Tom Tester", 0, "123", false).Save();
        }

        private static void Stops()
        {
            System.IO.File.WriteAllText(projectDocPath + "stops.xml", string.Empty);

            new Stop("Smėlio 15", true, Stop.ServiceType.CarWash | Stop.ServiceType.Charging).Save();
            new Stop("Ozas", true, Stop.ServiceType.Charging).Save();
        }
    }
}
