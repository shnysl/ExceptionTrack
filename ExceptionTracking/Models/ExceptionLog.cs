
using System;

namespace ExceptionTracking.Models
{
    public class ExceptionLog
    {
        public DateTime DateTime  { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }
}
