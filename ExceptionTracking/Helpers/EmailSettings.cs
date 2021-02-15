using System;
using System.Collections.Generic;
using System.IO;
using ExceptionTracking.Models;
using Microsoft.Extensions.Configuration;
using SrcFramework.Utils;

namespace ExceptionTracking.Helpers
{
    public class EmailSettings
    {
        public EmailSettings(List<List<ExceptionData>> exceptionDataList)
        {
            var configuration = GetConfiguration(); 
            var eMailContent = new EMailContent(); 
            eMailContent.From = configuration.GetSection("EmailSettings:Email").Value;
            eMailContent.ToList = configuration.GetSection("EmailSettings:To").Get<List<string>>();
            eMailContent.Subject = "Exception List";
            string databaseName = "";
            string tableContent = ""; 
            foreach (var variable in exceptionDataList) 
            {
                foreach (var item in variable) 
                {
                    databaseName = item.DatabaseName; 
                    tableContent+= "<tr>" + $"<td>{item.Id}</td>" + $"<td>{item.Message}" + $"<td>{item.StackTrace}</td>" + $"<td>{item.CreatedDate}</td>" + "</tr>";
                }
                eMailContent.Content += "<table border='1'>" + "<tr>" + "<td><b>Database Name</b></td>" + $"<td style='color:#FF0000' colspan='3'><b>{databaseName}</b></td>" + "</tr>" + "<tr>" + "<td><b>Exception Id</b></td>" + "<td><b>Message</b></td>" + "<td><b>Stack Trace</b></td>" + "<td><b>Created Date</b></td>" + "</tr >" + tableContent + "</ table >";
            }

            eMailContent.Password = configuration.GetSection("EmailSettings:Password").Value;
            eMailContent.Port = Int32.Parse(configuration.GetSection("EmailSettings:Port").Value);
            eMailContent.ServerAddress = configuration.GetSection("EmailSettings:ServerAddress").Value;
            EMailHelper.SendEmail(eMailContent);
        }

        private IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
    }
}
