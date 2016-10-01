using System.Collections.Generic;

namespace electric_car_rental.Entities
{
    public struct UserWithJourneys
    {
        public User User { get; set; }
        public IEnumerable<Journey> UserJourneys { get; set; }

        public UserWithJourneys(User user, IEnumerable<Journey> userJourneys)
        {
            User = user;
            UserJourneys = userJourneys;
        }
    }
}
