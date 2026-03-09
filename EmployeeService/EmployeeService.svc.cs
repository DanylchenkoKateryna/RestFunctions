using System.Configuration;
using System.Net;
using System.ServiceModel.Web;
using EmployeeService.Data;
using EmployeeService.Models;

namespace EmployeeService
{
    public class EmployeeServiceImpl : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeServiceImpl()
        {
            var connectionString = ConfigurationManager
                .ConnectionStrings["EmployeeDB"]
                .ConnectionString;

            _repository = new EmployeeRepository(connectionString);
        }

        internal EmployeeServiceImpl(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public EmployeeDto GetEmployeeById(int id)
        {
            var employee = _repository.GetEmployeeTree(id);

            if (employee == null)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.NotFound;
                
                return null;
            }

            return employee;
        }

        public void EnableEmployee(int id, int enable)
        {
            _repository.SetEnabled(id, enable == 1);

            WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.NoContent;
        }
    }
}
