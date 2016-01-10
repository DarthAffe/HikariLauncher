using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace HikariLauncher.Helper
{
    public static class SerializationHelper
    {
        #region Methods

        public static string Serialize<T>(T obj)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StringWriter stringWriter = new StringWriter())
            using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter))
            {
                serializer.Serialize(xmlWriter, obj);
                return stringWriter.ToString();
            }
        }

        public static T Deserialize<T>(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StringReader stringReader = new StringReader(xml))
            using (XmlReader xmlReader = XmlReader.Create(stringReader))
                return (T)serializer.Deserialize(xmlReader);
        }

        #endregion
    }
}
