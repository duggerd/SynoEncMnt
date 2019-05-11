using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SynoEncMnt
{
    class Terminal
    {
        private ShellStream ShellStream { get; set; }

        public Terminal(ShellStream shellStream)
        {
            ShellStream = shellStream;
        }

        public string SendCommand(string customCMD)
        {
            StringBuilder answer;

            var reader = new StreamReader(ShellStream);
            var writer = new StreamWriter(ShellStream);
            writer.AutoFlush = true;
            WriteStream(customCMD, writer, ShellStream);
            answer = ReadStream(reader);
            return answer.ToString();
        }

        private void WriteStream(string cmd, StreamWriter writer, ShellStream stream)
        {
            writer.Write(cmd + '\n');
            Thread.Sleep(5000);
            /*while (stream.Length == 0)
            {
                Thread.Sleep(500);
            }*/
        }

        private StringBuilder ReadStream(StreamReader reader)
        {
            StringBuilder result = new StringBuilder();

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                result.AppendLine(line);
            }
            return result;
        }
    }
}
