using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace electric_car_rental.Entities
{
    class BaseStorage
    {
        protected static String projectDocPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                                                 + $@"\{ConfigurationManager.AppSettings["project_name"]}\";

        public static bool Delete<T>(Entity item)
        {
            XDocument document;
            String typeName = typeof(T).Name.ToString();
            String typeFile = typeof(T).Name.ToString().ToLower() + "s.xml";

            try
            {
                document = XDocument.Load(projectDocPath + typeFile);
            }
            catch (Exception e)
            {
                document = new XDocument(new XElement(typeName + "s"));
                document.Save(projectDocPath + typeFile);
            }

            // Find whether entity exists by id - if it does, delete it. Otherwise, return false
            var rootNode = document.Element(typeName + "s");
            var entityNodes = rootNode.Elements(typeName);
            var id = item.ID;
            var thisNode = entityNodes.FirstOrDefault(node => (string)node.Attribute("ID") == item.ID);

            if (thisNode != null)
            {
                thisNode.Remove();
                document.Save(projectDocPath + typeFile);

                return true;
            } else
            {
                return false;
            }
        }

        public static bool Save<T>(Entity objectToSave)
        {
            XDocument document;
            String typeName = typeof(T).Name.ToString();
            String typeFile = typeof(T).Name.ToString().ToLower() + "s.xml";
            var xElement = SerializeToXElement(objectToSave); 

            try
            {
                document = XDocument.Load(projectDocPath + typeFile);
            }
            catch (Exception e)
            {
                document = new XDocument(new XElement(typeName + "s"));
                document.Save(projectDocPath + typeFile);
            }

            // Find whether entity exists by id - if it does, update it. Otherwise, add it as a new node
            var rootNode = document.Element(typeName + "s");
            var entityNodes = rootNode.Elements(typeName);
            var id = objectToSave.ID;
            var thisNode = entityNodes.FirstOrDefault(node => (string)node.Attribute("ID") == objectToSave.ID);

            if (thisNode != null)
            {
                thisNode.ReplaceWith(xElement);
            }
            else
            {
                rootNode.Add(xElement);
            }

            document.Save(projectDocPath + typeFile);

            return true;
        }

        public static List<T> All<T>()
        {
            XDocument document;
            var entities = new List<T>();
            String typeName = typeof(T).Name.ToString();
            String typeFile = typeof(T).Name.ToString().ToLower() + "s.xml";

            try
            {
                document = XDocument.Load(projectDocPath + typeFile);
            }
            catch (Exception e)
            {
                document = new XDocument(new XElement(typeName + "s"));
                document.Save(projectDocPath + typeFile);

                return entities;
            }

            var xmlNodes = from xmlNode in document.Root.Elements(typeName)
                           select xmlNode;

            foreach (var xmlNode in xmlNodes)
            {
                Type type = typeof(T);
                var obj   = DeserializeFromXElement(xmlNode, type);

                entities.Add((T)obj); // as entities is a list of T, casting now allows not to cast in future
            }

            return entities;
        }

        public static object DeserializeFromXElement(XElement element, Type t)
        {
            using (XmlReader reader = element.CreateReader())
            {
                XmlSerializer serializer = new XmlSerializer(t);
                return serializer.Deserialize(reader);
            }
        }

        public static XElement SerializeToXElement(Entity o)
        {
            var doc = new XDocument();

            using (XmlWriter writer = doc.CreateWriter())
            {
                XmlSerializer serializer = new XmlSerializer(o.GetType());
                serializer.Serialize(writer, o);
            }

            return doc.Root;
        }
    }
}
