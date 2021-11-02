using System;
using System.IO;
using System.Text;
//using NLog;
//using NLog.Layouts;

namespace Service_eBike_Pruefstand
{
    public class Logger
    {
        public static bool[] logState = new bool[4];
        public string Path { get; private set; }
        public Logger(string name)
        {
            Path = $"/home/pi/Documents/{name}/{name}_{DateTime.Now:yyyy-MM-dd HH:mm.ss}.txt";
        }

        public void Log(string s)
        {
            using (StreamWriter streamWriter = File.AppendText(Path))
                streamWriter.WriteLine($"{ DateTime.Now:yyyy-MM-dd HH:mm:ss}" + ", " + s);
        }

    }
}


//Log file using NLog 

//private CsvLayout csvLayout;
//private NLog.Config.LoggingConfiguration loggingConfiguration;
//private NLog.Targets.FileTarget fileTarget;
//private NLog.Logger logger;

//public Logger(string name)
//{
//    csvLayout = new CsvLayout()
//    {
//        Columns =
//                {
//                    new CsvColumn("Time", Layout.FromMethod(layoutMethod => layoutMethod.TimeStamp.ToString("yyyy.mm.dd HH.mm.ss"))),
//                    new CsvColumn("Value", "${message}"),
//                },
//    };

//    loggingConfiguration = new NLog.Config.LoggingConfiguration();

//    // Targets where to log to: File and Console
//    fileTarget = new NLog.Targets.FileTarget("logfile")
//    {
//        FileName = $"/home/pi/Documents/Temperatur/{name}_{DateTime.Now:yyyy-mm-dd HH:mm.ss}.csv",
//        Layout = csvLayout,
//    };
//    // Rules for mapping loggers to targets
//    loggingConfiguration.AddRule(LogLevel.Trace, LogLevel.Fatal, fileTarget);
//    // Apply config           
//    LogManager.Configuration = loggingConfiguration;
//    logger = LogManager.GetLogger(name);
//}

//public void log(string s)
//{
//    logger.Trace(s);
//}
