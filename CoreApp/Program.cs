using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading;

namespace CoreApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .AddEnvironmentVariables();

            var configuration = builder.Build();

            JsonConfigurationProvider jsonConfig = configuration.Providers.FirstOrDefault() as JsonConfigurationProvider;
            EnvironmentVariablesConfigurationProvider envConfig = configuration.Providers.LastOrDefault() as EnvironmentVariablesConfigurationProvider;

            while (true)
            {
                string connStr = string.Empty;

                if (envConfig.TryGet("TestConnection", out connStr))
                {
                    TestConnection(connStr);
                    return;
                }

                int i = 1;

                while (jsonConfig.TryGet("TestConnection" + i, out connStr))
                {
                    i++;

                    LogToConsole("--------------------");
                    LogToConsole("Will test " + connStr);
                    TestConnection(connStr);
                }

                LogToConsole("--------------------");
                LogToConsole("And now ENV variables");

                i = 1;

                while (envConfig.TryGet("TestConnection" + i, out connStr))
                {
                    i++;

                    LogToConsole("--------------------");
                    LogToConsole("Will test " + connStr);

                    TestConnection(connStr);
                }


                Thread.Sleep(new TimeSpan(0, 1, 0));
            }


            //Console.WriteLine("We're done. Press any key to quit.");
            //Console.ReadKey();
        }

        private static void TestConnection(string connStr)
        {
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                try
                {
                    connection.Open();
                    LogToConsole("Success");
                }
                catch (Exception ex)
                {
                    LogToConsole(ex.Message);
                    LogToConsole(ex.StackTrace);
                }
            }
        }

        private static void LogToConsole(string message)
        {
            Console.WriteLine($"{DateTime.Now} - {message}");
        }
    }
}
