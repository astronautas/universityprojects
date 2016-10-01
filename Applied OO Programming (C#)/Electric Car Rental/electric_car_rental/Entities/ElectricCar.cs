using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace electric_car_rental.Entities
{
    public class ElectricCar : Entity
    {
        private static List<ElectricCar> cars = new List<ElectricCar>();

        [XmlAttribute]
        public String Model         { get; set; }

        [XmlAttribute]
        public int    Range         { get; set; }

        [XmlAttribute]
        public int    RechargeTime  { get; set; }

        [XmlAttribute]
        public int    Power         { get; set; }

        [XmlAttribute]
        public string OccupiedBy    { get; set; }

        [XmlAttribute]
        public string StationedAt   { get; set; }

        [XmlAttribute]
        public int    EnergyLeft    { get; set; }

        [XmlAttribute]
        public double PricePerHour  { get; set; }

        public static List<ElectricCar> Cars
        {
            get { return cars; }
        }

        public ElectricCar(string model, int range, int rechargeTime, int power,
                           int energyLeft, double pricePerHour, string occupiedBy = "", 
                           string stationedAt = "") : base()
        {
            Model = model;
            Range = range;
            Power = power;

            RechargeTime = rechargeTime;
            PricePerHour = pricePerHour;
            EnergyLeft   = energyLeft;
            OccupiedBy   = occupiedBy;
            StationedAt  = stationedAt;
        }

        public ElectricCar() : this("", 0, 0, 0, 0, 0)
        {
            GenerateID();
        }

        public static List<ElectricCar> All()
        {
            return entityManager.All<ElectricCar>();
        }

        public bool Save()
        {
            return entityManager.Save<ElectricCar>(this);
        }

        public bool Delete()
        {
            return entityManager.Delete<ElectricCar>(this);
        }

        public static ElectricCar GetById(string id)
        {
            ElectricCar selectedCar;
            var cars = All();

            try
            {
                selectedCar = cars.First(i => i.ID == id);
            }
            catch (Exception exception)
            {
                return null;
            }

            return selectedCar;
        }
    }
}
