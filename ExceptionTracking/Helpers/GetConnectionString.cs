using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using ExceptionTracking.Models;
using Microsoft.Extensions.Configuration;

namespace ExceptionTracking.Helpers
{
    public class GetConnectionString
    {
        readonly List<List<ExceptionData>> _exceptionData = new List<List<ExceptionData>>();
        public GetConnectionString()
        {
            var configuration = GetConfiguration();
            var connectionStringList = configuration.GetSection("DbSettings:ConnectionString").GetChildren().AsEnumerable().ToList();
            for (int i = 0; i < connectionStringList.Count; i++)
            {
                List<ExceptionData> exceptionDataList = new List<ExceptionData>();
                var connection = new SqlConnection(connectionStringList[i].Value);
                connection.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = connection;
                sqlCommand.CommandText = "SELECT * FROM ExceptionLog WHERE CreatedDate >= DATEADD(hour, -1, GETDATE())";
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    foreach (var unused in sqlDataReader)
                    {
                        ExceptionData exceptionData = new ExceptionData();
                        exceptionData.Id = Int32.Parse(sqlDataReader["Id"].ToString() ?? string.Empty);
                        exceptionData.CreatedDate = DateTime.Parse(sqlDataReader["CreatedDate"].ToString() ?? string.Empty);
                        exceptionData.Message = sqlDataReader["Message"].ToString();
                        exceptionData.StackTrace = sqlDataReader["StackTrace"].ToString();
                        exceptionData.DatabaseName = connection.Database;
                        exceptionDataList.Add(exceptionData);
                    }
                    if (exceptionDataList.Count >= 10)
                    {
                        _exceptionData.Add(exceptionDataList);
                    }
                }
                connection.Close();
            }
        }
        public List<List<ExceptionData>> GetDataList()
        {
            return _exceptionData;
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
