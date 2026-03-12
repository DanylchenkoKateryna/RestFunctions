using EmployeeService.Models;

namespace EmployeeService.Data
{
    public interface IEmployeeRepository
    {
        Employee GetEmployeeTree(int id);

        void SetEnabled(int id, bool enable);
    }
}
