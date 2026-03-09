using System;
using System.Data;
using System.Data.SqlClient;
using EmployeeService.Data;
using EmployeeService.Models;

namespace InterviewConsole
{
    internal sealed class DemoHelper
    {
        private readonly IEmployeeRepository _repository;
        private readonly string _connectionString;

        internal DemoHelper(IEmployeeRepository repository, string connectionString)
        {
            _repository = repository;
            _connectionString = connectionString;
        }

        internal void GetEmployeeById(int id)
        {
            Console.WriteLine("=== GetEmployeeById({0}) ===", id);

            var employee = _repository.GetEmployeeTree(id);

            if (employee == null)
            {
                Console.WriteLine("Employee with ID={0} not found.", id);
            }
            else
            {
                PrintTree(employee, depth: 0);
            }

            Console.WriteLine();
        }

        internal void EnableEmployee(int id, bool enable)
        {
            Console.WriteLine("=== EnableEmployee(id={0}, enable={1}) ===", id, enable);

            PrintRawRow(id);
            _repository.SetEnabled(id, enable);

            Console.WriteLine("  -> Employee {0} is now {1}.", id, enable ? "ENABLED" : "DISABLED");

            PrintRawRow(id);

            Console.WriteLine();
        }

        private static void PrintTree(EmployeeDto employee, int depth)
        {
            var indent  = new string(' ', depth * 4);
            var manager = employee.ManagerID.HasValue
                ? "ManagerID: " + employee.ManagerID.Value
                : "root";

            Console.WriteLine("{0}[{1}] {2} ({3})", indent, employee.ID, employee.Name, manager);

            foreach (var subordinate in employee.Employees)
            {
                PrintTree(subordinate, depth + 1);
            }
        }

        private void PrintRawRow(int id)
        {
            var dt = GetQueryResult(
                "SELECT ID, Name, ManagerID, Enable FROM dbo.Employee WHERE ID = " + id);

            if (dt.Rows.Count == 0)
            {
                Console.WriteLine("  [DB] Employee {0} not found.", id);
                return;
            }

            var row = dt.Rows[0];
            Console.WriteLine("  [DB] ID={0}, Name={1}, ManagerID={2}, Enable={3}",
                row["ID"],
                row["Name"],
                row["ManagerID"] == DBNull.Value ? "NULL" : row["ManagerID"].ToString(),
                row["Enable"]);
        }

        private DataTable GetQueryResult(string query)
        {
            var dt = new DataTable();

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    using (var adapter = new SqlDataAdapter(cmd))
                        adapter.Fill(dt);
                }
            }

            return dt;
        }
    }
}
