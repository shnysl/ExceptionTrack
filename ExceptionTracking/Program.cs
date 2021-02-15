using System;
using ExceptionTracking.Helpers;

namespace ExceptionTracking
{
    static class Program
    {
        private static void Main()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += MyHandler;
            GetConnectionString getConnectionString = new GetConnectionString();
            if (getConnectionString.GetDataList().Count != 0)
            {
                EmailSettings emailSettings = new EmailSettings(getConnectionString.GetDataList());
            }
            
            void MyHandler(object sender, UnhandledExceptionEventArgs args)
            {
                Exception exception = (Exception)args.ExceptionObject;
                ExceptionLogger.Log(exception);
            }
        }
    }
}
