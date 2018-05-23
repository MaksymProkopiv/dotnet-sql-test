using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Data.SqlClient;
using System.IO;
using Microsoft.Extensions.Configuration.Json;

namespace CoreApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json");

            var configuration = builder.Build();

            JsonConfigurationProvider jcp = configuration.Providers.FirstOrDefault() as JsonConfigurationProvider;

            string connStr = string.Empty;
            int i = 1;

            while (jcp.TryGet("TestConnection" + i, out connStr))
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
