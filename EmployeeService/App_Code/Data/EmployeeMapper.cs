using System.Collections.Generic;
using System.Data;
using EmployeeService.Models;

namespace EmployeeService.Data
{
    internal static class EmployeeMapper
    {
        internal static EmployeeDto MapRow(IDataRecord record)
        {
            return new EmployeeDto
            {
                ID = record.GetInt32(record.GetOrdinal("ID")),
                Name = record.GetString(record.GetOrdinal("Name")),
                ManagerID = record.IsDBNull(record.GetOrdinal("ManagerID"))
                                ? (int?)null
                                : record.GetInt32(record.GetOrdinal("ManagerID"))
            };
        }

        internal static void BuildTree(Dictionary<int, EmployeeDto> flat, int rootId)
        {
            foreach (var emp in flat.Values)
            {
                if (emp.ID == rootId)
                    continue;

                EmployeeDto parent;
                if (emp.ManagerID.HasValue &&
                    flat.TryGetValue(emp.ManagerID.Value, out parent))
                {
                    parent.Employees.Add(emp);
                }
            }
        }
    }
}
