using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using EmployeeService.Models;

namespace EmployeeService.Data
{
    public sealed class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connectionString;

        public EmployeeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public EmployeeDto GetEmployeeTree(int id)
        {
            const string sql = @"
                WITH EmployeeHierarchy AS (
                    SELECT ID, Name, ManagerID
                    FROM dbo.Employee
                    WHERE ID = @EmployeeID
                    UNION ALL
                    SELECT e.ID, e.Name, e.ManagerID
                    FROM dbo.Employee AS e
                    JOIN EmployeeHierarchy AS h ON e.ManagerID = h.ID
                )
                SELECT ID, Name, ManagerID
                FROM EmployeeHierarchy
                OPTION (MAXRECURSION 100);";

            var flat = new Dictionary<int, EmployeeDto>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd  = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add(new SqlParameter("@EmployeeID", SqlDbType.Int) { Value = id });

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        var dto = EmployeeMapper.MapRow(reader);
                        flat[dto.ID] = dto;
                    }
            }

            EmployeeDto root;
            if (!flat.TryGetValue(id, out root))
                return null;

            EmployeeMapper.BuildTree(flat, rootId: id);
            return root;
        }

        public void SetEnabled(int id, bool enable)
        {
            const string sql = @"
                UPDATE dbo.Employee
                SET Enable = @Enable
                WHERE ID = @EmployeeID;";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd  = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add(new SqlParameter("@EmployeeID", SqlDbType.Int) { Value = id });
                cmd.Parameters.Add(new SqlParameter("@Enable",     SqlDbType.Bit) { Value = enable });

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
