using System;
using System.IO;

namespace Arego.OrderTransfer.Process
{
    public class LogFileWriter
    {
        private static StreamWriter _writer;

        //================================================================================
        // Static konstruktor som skapar eller öppnar loggfilen.
        //================================================================================
        static LogFileWriter()
        {
            var path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            path = path.Substring(0, path.LastIndexOf("\\"));
            path += @"\Logg";

            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
        }

        ~LogFileWriter()
        {
            _writer.Flush();
            _writer.Close();
        }

        //======================================================================
        // Skriver en rad till loggfilen.
        //======================================================================
        public static void WriteLine(string line)
        {
            var today = DateTime.Today.ToShortDateString().Replace('/', '-');

            var path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            path = path.Substring(0, path.LastIndexOf("\\"));
            path += @"\Logg";
            var fileName = @"\" + today + " loggfil.txt";

            if (!File.Exists(path + fileName))
            {
                var fs = File.Create(path + fileName);
                fs.Close();

                _writer = new StreamWriter(path + fileName, true);
            }
            else
            {
                if (_writer == null)
                    _writer = new StreamWriter(path + fileName, true);
            }

            _writer.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + ": " + line);
            _writer.Flush();
        } 
    }
}