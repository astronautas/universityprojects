using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace electric_car_rental.Entities
{
    public class Entity : IEquatable<Entity>
    {
        [XmlIgnore]
        protected static EntityManager entityManager = new EntityManager();

        [XmlAttribute]
        public string ID { get; set; }

        [XmlIgnore]
        public bool isValid { get; protected set; }

        public Entity()
        {
            ID = GenerateID();
        }

        public string GenerateID()
        {
            return Guid.NewGuid().ToString("N");
        }

        // ID is unique for every entity. Thus, two
        // equal entities will have same IDs
        public bool Equals(Entity objectToBeCompared)
        {
            return (this.ID == objectToBeCompared.ID);
        }
    }
}
