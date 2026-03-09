using System;
using System.Configuration;
using EmployeeService.Data;

namespace InterviewConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = ConfigurationManager
                .ConnectionStrings["EmployeeDB"]
                .ConnectionString;

            var repository = new EmployeeRepository(connectionString);
            var demo = new DemoHelper(repository, connectionString);

            demo.GetEmployeeById(id: 1);
            demo.EnableEmployee(id: 5, enable: false);
            demo.EnableEmployee(id: 5, enable: true);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
