using Mono.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SynoEncMnt
{
    class Program
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        static int Main(string[] args)
        {
            int code = 1;

            try
            {
                string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

                logger.Info("SynoEncMnt version {0}", version);

                bool mount = false;
                bool dismount = false;
                string config = string.Empty;

                OptionSet commandLineOptions = new OptionSet
                {
                    { "mount", v => mount = v != null },
                    { "dismount", v => dismount = v != null },
                    { "config=", v => config = v }
                };

                try
                {
                    commandLineOptions.Parse(args);
                }
                catch (OptionException ex)
                {
                    logger.Fatal("error parsing arguments: {0}", ex.Message);
                    return code;
                }

                string configContents = string.Empty;

                if (config != string.Empty)
                {
                    try
                    {
                        configContents = File.ReadAllText(config);
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("error reading config file: {0}", ex.Message);
                        return code;
                    }
                }
                else
                {
                    logger.Fatal("config file must be specified");
                    return code;
                }

                try
                {
                    Config.Init(configContents);
                }
                catch (Exception ex)
                {
                    logger.Fatal("error parsing config file: {0}", ex.Message);
                    return code;
                }

                if (mount && dismount)
                {
                    logger.Fatal("actions mount and dismount both specified");
                    return code;
                }
                else if (mount)
                {
                    code = Engine.PerformAction(Engine.NasAction.Mount);
                }
                else if (dismount)
                {
                    code = Engine.PerformAction(Engine.NasAction.Dismount);
                }
                else
                {
                    logger.Fatal("action mount or dismount must be specified");
                    return code;
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("unhandled exception: {0}", ex.Message);
            }

            return code;
        }
    }
}
