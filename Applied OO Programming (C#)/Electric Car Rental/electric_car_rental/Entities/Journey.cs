using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace electric_car_rental.Entities
{
    public class Journey : Entity
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        [XmlAttribute]
        public string StartStop { get; set; }

        [XmlAttribute]
        public string EndStop { get; set; }

        [XmlAttribute]
        public string ElectricCarID { get; set; }

        [XmlAttribute]
        public string UserID { get; set; }

        public Journey(DateTime? startTime, string startStop, string endStop, string electricCarID, 
                       string userID, DateTime? endTime = null)
        {
            StartTime = startTime;
            StartStop = startStop;
            EndTime   = endTime;
            EndStop = endStop;
            ElectricCarID = electricCarID;
            UserID = userID;
        }

        public Journey() : this(null, "", "", "", "") { }

        public double UsedTime()
        {        
            DateTime start = StartTime.Value;

            return (DateTime.Now - start).Hours;
        }

        public static IEnumerable<UserWithJourneys> GetUsersJourneys()
        {
            var journeys = from user in User.All()
                           join journey in Journey.All() on user.ID equals journey.UserID into gj
                           select new UserWithJourneys(user: user, userJourneys: gj);

            return journeys;
        }

        public double GetTripCost()
        {
            var currentCarCost = ElectricCar.GetById(ElectricCarID).PricePerHour;

            return UsedTime() * currentCarCost + currentCarCost;
        }

        public void Finish()
        {
            var currentCar  = ElectricCar.GetById(ElectricCarID);
            var currentUser = User.GetById(UserID);

            this.EndTime = DateTime.Now;

            currentCar.OccupiedBy = "";
            currentUser.MoneySpent += GetTripCost();

            currentCar.Save();
            currentUser.Save();
        }

        public bool Save()
        {
            return entityManager.Save<Journey>(this);
        }

        public static List<Journey> All()
        {
            return entityManager.All<Journey>();
        }
    }
}
