using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace DontSleepWPF
{
    internal class AppSettings
    {
        private static readonly string _filePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
            "DontSleep", 
            "AppConfig.xml");

        internal static int Timeout;

        internal static void Init()
        {
            Timeout = 180;

            if (!File.Exists(_filePath))
            {
                Create();
                return;
            }

            Load();
        }

        internal static void Save()
        {
            if (!File.Exists(_filePath))
            {
                Create();
                return;
            }

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(_filePath);
            XmlElement xRoot = xDoc.DocumentElement;
            foreach (XmlNode xnode in xRoot)
            {
                if (xnode.Name == "Timeout")
                {
                    xnode.InnerText = Timeout.ToString();
                }
            }
            xDoc.Save(_filePath);
        }

        private static void Create()
        {
            var dir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "DontSleep");

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var xml = new XElement(
                    "AppSettings",
                        new XElement("Timeout", Timeout.ToString()));
            xml.Save(_filePath);
        }

        private static void Load()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(_filePath);
            XmlElement xRoot = xDoc.DocumentElement;
            foreach (XmlNode xnode in xRoot)
            {
                if (xnode.Name == "Timeout")
                {
                    int.TryParse(xnode.InnerText, out Timeout);
                }
            }
        }

    }
}
