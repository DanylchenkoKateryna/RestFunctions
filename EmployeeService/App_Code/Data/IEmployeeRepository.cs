using EmployeeService.Models;

namespace EmployeeService.Data
{
    public interface IEmployeeRepository
    {
        EmployeeDto GetEmployeeTree(int id);

        void SetEnabled(int id, bool enable);
    }
}
