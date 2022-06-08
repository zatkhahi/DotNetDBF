using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DotNetDBF
{
    public class Logs
    {
        public static void WriteLine(string msg)
        {
            string path = @"dotnetdbf-logs.txt";
            File.AppendAllLines(path, new[] { msg + "\r\n" });
        }
    }
}
