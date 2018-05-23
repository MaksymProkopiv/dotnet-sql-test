using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

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

            string connStr = string.Empty;
            int i = 1;

            while (jsonConfig.TryGet("TestConnection" + i, out connStr))
            {
                i++;

                Console.WriteLine("--------------------");
                Console.WriteLine("Will test " + connStr);

                using (SqlConnection connection = new SqlConnection(connStr))
                {
                    try
                    {
                        connection.Open();
                        Console.WriteLine("Success");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);
                    }
                }
            }

            Console.WriteLine("--------------------");
            Console.WriteLine("And now ENV variables");

            i = 1;

            while (envConfig.TryGet("TestConnection" + i, out connStr))
            {
                i++;

                Console.WriteLine("--------------------");
                Console.WriteLine("Will test " + connStr);

                using (SqlConnection connection = new SqlConnection(connStr))
                {
                    try
                    {
                        connection.Open();
                        Console.WriteLine("Success");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);
                    }
                }
            }

            Console.WriteLine("We're done. Press any key to quit.");
            Console.ReadKey();
        }
    }
}
