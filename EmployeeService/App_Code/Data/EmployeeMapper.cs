using System.Collections.Generic;
using System.Data;
using EmployeeService.Models;

namespace EmployeeService.Data
{
    public static class EmployeeMapper
    {
        public static EmployeeDto ToDto(Employee employee)
        {
            var dto = new EmployeeDto
            {
                ID = employee.ID,
                Name = employee.Name,
                ManagerID = employee.ManagerID
            };

            foreach (var child in employee.Employees)
            {
                dto.Employees.Add(ToDto(child));
            }

            return dto;
        }


        internal static Employee MapRow(IDataRecord record)
        {
            return new Employee
            {
                ID = record.GetInt32(record.GetOrdinal("ID")),
                Name = record.GetString(record.GetOrdinal("Name")),
                ManagerID = record.IsDBNull(record.GetOrdinal("ManagerID"))
                                ? (int?)null
                                : record.GetInt32(record.GetOrdinal("ManagerID"))
            };
        }

        internal static void BuildTree(Dictionary<int, Employee> flat, int rootId)
        {
            foreach (var emp in flat.Values)
            {
                if (emp.ID == rootId)
                    continue;

                Employee parent;
                if (emp.ManagerID.HasValue &&
                    flat.TryGetValue(emp.ManagerID.Value, out parent))
                {
                    parent.Employees.Add(emp);
                }
            }
        }
    }
}
