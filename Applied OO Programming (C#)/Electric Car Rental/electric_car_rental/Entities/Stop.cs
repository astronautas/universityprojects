using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace electric_car_rental.Entities
{
    public class Stop : Entity
    {
        [XmlAttribute]
        public string Address { get; set; }

        [Flags]
        public enum ServiceType : byte
        {
            Charging        = 1,
            CarWash         = 2,
            CustomerService = 4
        }

        public ServiceType? Services { get; set; }

        public Stop(string address, bool hasCharging = false, ServiceType? services = null) : base()
        {
            Address  = address;
            Services = services;
        }

        public Stop() : this("", false)
        {

        }

        public static List<Stop> All()
        {
            return entityManager.All<Stop>();
        }

        public bool Save()
        {
            return entityManager.Save<Stop>(this);
        }
    }
}
