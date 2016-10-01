using System;

using electric_car_rental.Entities;
using System.Text.RegularExpressions;
using electric_car_rental.Test;
using System.Linq;
using System.Configuration;

namespace electric_car_rental
{
    class Program
    {
        private static User CurrentUser { get; set; }

        static void Main(string[] args)
        {
            if (ConfigurationManager.AppSettings["environment"] == "test")
            {
                Fixtures.Load();
            }

            StartScreen();
        }

        private static void StartScreen()
        {
            while (true)
            {
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Register");
                Console.WriteLine("3. Exit");

                var cmd = Console.ReadLine();

                switch (cmd)
                {
                    case "1" : LoginScreen(); break;
                    case "2" : RegisterScreen(); break;
                    case "3" : System.Environment.Exit(1); break;
                    default:
                        break;
                }
            }
        }

        private static void LoginScreen()
        {
            String email;
            String password;
            User user;

            do
            {
                Console.WriteLine("Enter your email address");
                email = Console.ReadLine();
            } while (email.Length == 0 || (user = User.GetByEmail(email)) == null);

            do
            {
                Console.WriteLine($"Hi {user.FullName}! Enter your password");
                password = Console.ReadLine();
            } while (user.Password != password);

            Console.WriteLine("Logged in!");

            CurrentUser = user;
            AfterLoggedInScreen();
        }

        private static void RegisterScreen()
        {
            String email, password, fullName;
            bool userExists;
        
            var user = new User();

            do
            {
                Console.WriteLine("Enter an email address");
                email = Console.ReadLine();

                user.Email = email;
                userExists = User.All().Any(currentUser => currentUser.Equals(user));
            } while (!user.isValid || userExists);

            do
            {
                Console.WriteLine("Enter your full name");
                fullName = Console.ReadLine();
                user.FullName = fullName;
            } while (fullName.Length == 0);

            do
            {
                Console.WriteLine("Enter a password (>= 5 characters)");
                password = Console.ReadLine();
                user.Password = password;
            } while (password.Length < 5);

            user.Save();

            Console.WriteLine($"Welcome {user.FullName}");
            CurrentUser = user;

            AfterLoggedInScreen();
        }

        private static void AfterLoggedInScreen()
        {
            while (true)
            {
                Console.WriteLine("1. Make a reservation");
                Console.WriteLine("2. Current car");
                Console.WriteLine(CurrentUser.IsAdmin ? "3. List all journeys" : "");
                Console.WriteLine(CurrentUser.IsAdmin ? "4. Delete an electric car" : "");
                Console.WriteLine(CurrentUser.IsAdmin ? "5. Add an electric car" : "");

                var cmd = Console.ReadLine();

                switch (cmd)
                {
                    case "1" : ReservationScreen(); break;
                    case "2" : CurrentCarScreen(); break;
                    case "3" : JourneysScreen(); break;
                    case "4" : DeleteCarScreen(); break;
                    default:
                        break;
                }
            }
        }

        // Maps users to journeys and prints out everything
        private static void JourneysScreen()
        {
            if (!CurrentUser.IsAdmin)
            {
                return;
            }

            var result = Journey.GetUsersJourneys();

            foreach (var pair in result)
            {
                Console.WriteLine(pair.User.FullName);

                if (pair.UserJourneys.Count() == 0)
                {
                    Console.WriteLine("* No journeys");
                }
                
                foreach(var journey in pair.UserJourneys)
                {
                    var carModel = ElectricCar.GetById(journey.ElectricCarID).Model;
                    Console.WriteLine($"* Car: {carModel}, Time: {journey.UsedTime()}, Price: {journey.GetTripCost()}");
                }
            }
        }

        private static void ReservationScreen()
        {
            if (ElectricCar.All().Any(car => (car.OccupiedBy == CurrentUser.ID)))
            {
                Console.WriteLine("You haven't finished the trip yet!");

                return;
            }

            int selectedNumber;
            bool parse;
            var stops = Stop.All();

            Console.WriteLine("Select an electric car" + Environment.NewLine);
        
            // Only not occupied cars can be reserved
            var cars = ElectricCar.All().Where(car => string.IsNullOrEmpty(car.OccupiedBy));

            var index = 0;

            foreach (var car in cars)
            {
                Console.WriteLine($"{index}. {car.Model}");
                index++;
            }

            do
            {
                parse = int.TryParse(Console.ReadLine(), out selectedNumber);
            } while (!parse || selectedNumber > index || selectedNumber < 0);

            var selectedCar = cars.ElementAt(selectedNumber);

            index = 0;

            // Selecting start stop
            Console.WriteLine("Select the start stop");

            foreach (var stop in stops)
            {
                Console.WriteLine($"{index}. {stop.Address} ({stop.Services.ToString()})");
                index++;
            }

            do
            {
                parse = int.TryParse(Console.ReadLine(), out selectedNumber);
            } while (!parse || selectedNumber > index || selectedNumber < 0);

            var selectedStartStop = stops.ElementAt(selectedNumber);

            // Selecting end stop
            index = 0;
            Console.WriteLine("Select the end stop");

            foreach (var stop in stops)
            {
                Console.WriteLine($"{index}. {stop.Address} ({stop.Services.ToString()})");
                index++;
            }

            do
            {
                parse = int.TryParse(Console.ReadLine(), out selectedNumber);
            } while (!parse || selectedNumber > index || selectedNumber < 0);

            var selectedEndStop = stops.ElementAt(selectedNumber);

            var journey = new Journey(startTime: DateTime.Now, startStop: selectedStartStop.ID, 
                                      endStop: selectedEndStop.ID, electricCarID: selectedCar.ID,
                                      userID: CurrentUser.ID);

            selectedCar.OccupiedBy = CurrentUser.ID;

            journey.Save();
            selectedCar.Save();
        }

        private static void CurrentCarScreen()
        {
            ElectricCar currentCar;

            try
            {
                currentCar = ElectricCar.All().Where(car => car.OccupiedBy == CurrentUser.ID).First();
            } catch (Exception e)
            {
                Console.WriteLine("No current car");
                return;
            }

            var currentJourney = Journey.All().Where(journey => (journey.ElectricCarID == currentCar.ID) &&
                                (journey.EndTime == null)).First();

            Console.WriteLine($"Current car: {currentCar.Model}");
            Console.WriteLine($"Usage time: {currentJourney.UsedTime()} hours");
            Console.WriteLine($"Trip cost: {currentJourney.GetTripCost()}");

            while (true)
            {
                Console.WriteLine("1. Finish reservation");
                Console.WriteLine("2. Back");

                var cmd = Console.ReadLine();

                switch (cmd)
                {
                    case "1": FinishReservation(); break;
                    case "2": AfterLoggedInScreen(); break;
                    default: break;
                }
            }
        }  

        private static void FinishReservation()
        {
            ElectricCar currentCar;

            try
            {
                currentCar = ElectricCar.All().Where(car => car.OccupiedBy == CurrentUser.ID).First();
            }
            catch (Exception e)
            {
                Console.WriteLine("No current car");
                return;
            }

            var currentJourney = Journey.All().Where(journey => (journey.ElectricCarID == currentCar.ID) && 
                                (journey.EndTime == null)).First();

            currentJourney.Finish();
            AfterLoggedInScreen();
        }

        private static void DeleteCarScreen()
        {
            if (CurrentUser.IsAdmin)
            {
                var cars = ElectricCar.All();
                var index = 0;
                int selectedNumber;
                bool parse;

                foreach (var car in cars)
                {
                    Console.WriteLine(index + " " + car.Model);
                    index++;
                }

                do
                {
                    Console.WriteLine("Enter a number of a car you wish to delete");
                    parse = int.TryParse(Console.ReadLine(), out selectedNumber);
                } while (!parse || selectedNumber > index || selectedNumber < 0);

                cars.ElementAt(selectedNumber).Delete();
            }
        }
    }
}
