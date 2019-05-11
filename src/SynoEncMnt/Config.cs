using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SynoEncMnt
{
    class Config
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static string Host { get; private set; }

        public static string Fingerprint { get; private set; }

        public static string Username { get; private set; }

        public static string Password { get; private set; }

        public static string Share { get; private set; }

        public static string Key { get; private set; }

        public static void Init(string configFileName)
        {
            logger.Debug("start config validation");

            XmlDocument doc = new XmlDocument();
            doc.Load(configFileName);

            Host = doc.SelectSingleNode("/config/host").InnerText;
            Fingerprint = doc.SelectSingleNode("/config/fingerprint").InnerText;
            Username = doc.SelectSingleNode("/config/username").InnerText;
            Password = doc.SelectSingleNode("/config/password").InnerText;
            Share = doc.SelectSingleNode("/config/share").InnerText;
            Key = doc.SelectSingleNode("/config/key").InnerText;

            logger.Debug("config validation successful");
        }

        static Config()
        {
        }
    }
}
