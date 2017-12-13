using System;
using System.IO;

namespace WPWS_Project2
{
    //DEAN JONES 
    //JUN.13, 2017 
    //LOGGER (WRITES TO LOG FILE)


    class Logger
    {
        //WRITE TO (LOG FILE)
        public static void Log(string logMessage, TextWriter w)
        {
            w.WriteLine(logMessage);

            //w.Write("\r\nLog Entry : ");
            //w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
            //    DateTime.Now.ToLongDateString());
            //w.WriteLine("  :{0}", logMessage);
            //w.WriteLine("-------------------------------");
        }

        //DUMP (FILE CONTENTS) TO CONSOLE
        public static void DumpLog(StreamReader r)
        {
            string line;
            while ((line = r.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
        }
    }
}
