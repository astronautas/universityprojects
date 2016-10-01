using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace electric_car_rental.Entities
{
    public class User : Entity, IEquatable<User>
    {
        public static List<User> users = new List<User>();

        private string _email;

        [XmlAttribute]
        public string Email
        {
            get
            {
                return _email;
            }

            set
            {
                var emailRegex = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                + "@"
                + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";

                if (Regex.Match(value, emailRegex).Success)
                {
                    this.isValid = true;
                } else
                {
                    this.isValid = false;
                }

                _email = value;
            }
        }

        [XmlAttribute]
        public string FullName { get; set; }

        [XmlAttribute]
        public double MoneySpent { get; set; }

        [XmlAttribute]
        public bool IsAdmin { get; set; }

        [XmlAttribute]
        public string Password { get; set; }

        public User(string email, string fullName, double moneySpent, string password, bool isAdmin = false) : base()
        {
            Email = email;
            FullName = fullName;
            MoneySpent = moneySpent;
            IsAdmin  = isAdmin;
            Password = password;
        }

        public User() : this("", "", 0, "") { }

        public bool Save()
        {
            return entityManager.Save<User>(this);
        }

        public bool Delete()
        {
            return entityManager.Delete<User>(this);
        }

        public static List<User> All()
        {
            return entityManager.All<User>();
        }

        public static User GetById(string id)
        {
            User selectedUser;
            var users = All();

            try
            {
                selectedUser = users.First(i => i.ID == id);
            }
            catch (Exception exception)
            {
                return null;
            }

            return selectedUser;
        }

        public static User GetByEmail(string email)
        {
            User selectedUser;
            var users = All();
            
            try
            {
                selectedUser = users.First(i => i.Email == email);
            } catch (Exception exception)
            {
                return null;
            }

            return selectedUser;
        }

        // All users have unique emails - thus, users are same
        // if they both have equal emails
        public bool Equals(User user)
        {
            return (this.Email == user.Email);
        }
    }
}
