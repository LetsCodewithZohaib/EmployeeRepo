using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeBLL
{
    public interface IEmployeeRepository
    {
        Root GetAll(string criteria = "");
        bool AddEmployee(Datum objEmployee);
        bool UpdateEmployee(Datum objEmployee);
        List<Datum> Search(string criteria);
        bool DeleteEmployee(int id);
    }
}
