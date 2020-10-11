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
        private static readonly string _filePath = "AppConfig.xml";

        internal static int Timeout;
        internal static DateTime StopTime;

        internal static void Init()
        {
            Timeout = 180;
            StopTime = DateTime.MinValue;

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
                if (xnode.Name == "StopTime")
                {
                    xnode.InnerText = StopTime.ToShortTimeString();
                }
            }
            xDoc.Save(_filePath);
        }

        private static void Create()
        {
            var xml = new XElement(
                    "AppSettings",
                        new XElement("Timeout", Timeout.ToString()),
                        new XElement("StopTime", StopTime.ToShortTimeString()));
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
                if (xnode.Name == "StopTime")
                {
                    DateTime.TryParse(xnode.InnerText, out StopTime);
                }
            }
        }

    }
}
