using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SynoEncMnt
{
    class Engine
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public enum NasAction
        {
            Mount,
            Dismount
        }

        public static int PerformAction(NasAction proc)
        {
            int code = 1;

            if (!OnlyContainsHexCharacters(Config.Fingerprint))
            {
                logger.Fatal("fingerprint contains invalid characters");
                return code;
            }

            byte[] expectedFingerPrint;

            try
            {
                expectedFingerPrint = ByteStringToByteArray(Config.Fingerprint);
            }
            catch (Exception ex)
            {
                logger.Fatal("error parsing fingerprint: {0}", ex.Message);
                return code;
            }

            try
            {
                using (SshClient client = new SshClient(Config.Host, Config.Username, Config.Password))
                {
                    client.HostKeyReceived += (sender, e) =>
                    {
                        if (expectedFingerPrint.Length == e.FingerPrint.Length)
                        {
                            for (var i = 0; i < expectedFingerPrint.Length; i++)
                            {
                                if (expectedFingerPrint[i] != e.FingerPrint[i])
                                {
                                    e.CanTrust = false;
                                    logger.Fatal("fingerprint does not match - expected {0}, got {1}", BitConverter.ToString(expectedFingerPrint).Replace("-", string.Empty), BitConverter.ToString(e.FingerPrint).Replace("-", string.Empty));
                                    break;
                                }
                            }
                        }
                        else
                        {
                            e.CanTrust = false;
                            logger.Fatal("fingerprint length does not match - expected {0}, got {1}", BitConverter.ToString(expectedFingerPrint).Replace("-", string.Empty), BitConverter.ToString(e.FingerPrint).Replace("-", string.Empty));
                        }
                    };

                    client.Connect();
                    ShellStream stream = client.CreateShellStream("customCommand", 80, 24, 800, 600, 1024);
                    Terminal terminal = new Terminal(stream);

                    if (proc == NasAction.Mount)
                    {
                        string resp1 = terminal.SendCommand(string.Format("sudo ~/mount.sh {0}", Config.Share));
                        logger.Trace("resp1: {0}", resp1);

                        if (resp1.Contains("Password:"))
                        {
                            logger.Debug("sudo password req");
                            string resp2 = terminal.SendCommand(Config.Password);
                            logger.Trace("resp2: {0}", resp2);

                            if (resp2.Contains("EncMountKey:"))
                            {
                                logger.Debug("mount key req");
                                string resp3 = terminal.SendCommand(Config.Key);
                                logger.Trace("resp3: {0}", resp3);

                                if (resp3.Contains("EncMountOk"))
                                {
                                    logger.Info("enc mount ok");
                                    code = 0;
                                }
                                else
                                {
                                    logger.Fatal("error finding enc mount ok response");
                                }
                            }
                            else
                            {
                                logger.Fatal("error finding enc mount key prompt");
                            }
                        }
                        else
                        {
                            logger.Fatal("error finding sudo password prompt");
                        }
                    }
                    else if (proc == NasAction.Dismount)
                    {
                        string resp1 = terminal.SendCommand(string.Format("sudo ~/dismount.sh {0}", Config.Share));
                        logger.Trace("resp1: {0}", resp1);

                        if (resp1.Contains("Password:"))
                        {
                            logger.Debug("sudo password req");
                            string resp2 = terminal.SendCommand(Config.Password);
                            logger.Trace("resp2: {0}", resp2);

                            if (resp2.Contains("EncDismountOk"))
                            {
                                logger.Info("enc dismount ok");
                                code = 0;
                            }
                            else
                            {
                                logger.Fatal("error finding enc dismount ok response");
                            }
                        }
                        else
                        {
                            logger.Fatal("error finding sudo password prompt");
                        }
                    }
                    else
                    {
                        logger.Fatal("action incorrect");
                    }

                    client.Disconnect();
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("error in ssh session: {0}", ex.Message);
                return code;
            }

            return code;
        }

        private static bool OnlyContainsHexCharacters(string input)
        {
            return Regex.IsMatch(input, @"\A\b[0-9a-fA-F]+\b\Z");
        }

        private static byte[] ByteStringToByteArray(string hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }
    }
}
