using System.ServiceModel;
using System.ServiceModel.Web;
using EmployeeService.Models;

namespace EmployeeService
{
    [ServiceContract]
    public interface IEmployeeService
    {
        [OperationContract]
        [WebInvoke(
            Method = "GET",
            UriTemplate = "GetEmployeeById?id={id}",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        EmployeeDto GetEmployeeById(int id);

        [OperationContract]
        [WebInvoke(
            Method = "PUT",
            UriTemplate = "EnableEmployee?id={id}",
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        void EnableEmployee(int id, int enable);
    }
}
