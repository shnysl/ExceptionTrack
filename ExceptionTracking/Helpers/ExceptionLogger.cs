using System;
using System.IO;
using ExceptionTracking.Models;
using Newtonsoft.Json;

namespace ExceptionTracking.Helpers
{
    static class ExceptionLogger
    {
        private static void Log(ExceptionLog exceptionLog)
        {
            try
            {
                string path = $"{AppDomain.CurrentDomain.BaseDirectory}\\ExceptionLog\\{DateTime.Now.Year}\\{DateTime.Now.Month}\\{DateTime.Now.Day}\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var sw = new StreamWriter($"{path}Log_{DateTime.Now:HH_mm}.txt", true);
                sw.WriteLine(JsonConvert.SerializeObject(exceptionLog));
                sw.Flush();
                sw.Close();
            }
            catch (Exception exception)
            {
                Console.WriteLine("************ERROR*************");
                Console.WriteLine(exceptionLog.Message);
                Console.WriteLine(exceptionLog.StackTrace);
                Console.WriteLine(exception.Message);
                Console.WriteLine(exception.StackTrace);
                Console.ReadKey();
                // ignored
            }
        }

        public static void Log(Exception exception)
        {
            var exceptionLog = new ExceptionLog
            {
                DateTime = DateTime.Now,
                Message = exception.Message,
                StackTrace = exception.StackTrace
            };

            if (exception.InnerException != null)
            {
                exceptionLog.StackTrace += Environment.NewLine + exception.InnerException.Message + Environment.NewLine + exception.InnerException.StackTrace;
            }
            ExceptionLogger.Log(exceptionLog);
        }
    }
}
