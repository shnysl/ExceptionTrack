using System;

namespace ExceptionTracking.Models
{
    public class ExceptionData
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Message  { get; set; }
        public string StackTrace { get; set; }
        public string DatabaseName { get; set; }
    }
}
