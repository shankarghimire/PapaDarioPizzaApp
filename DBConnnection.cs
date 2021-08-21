using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.IO;

namespace PapaDarioPizzaApp
{
    public class DBConnnection
    {
        private static string connectionString = "Springn2021ExamDB";
        public static string GetConnectionString()
        {
           
            ConfigurationBuilder cb = new ConfigurationBuilder();
            cb.SetBasePath(Directory.GetCurrentDirectory());
            cb.AddJsonFile("config.json");
            IConfiguration config = cb.Build();
            return config["ConnectionString:" + connectionString];
        }
    }
}
