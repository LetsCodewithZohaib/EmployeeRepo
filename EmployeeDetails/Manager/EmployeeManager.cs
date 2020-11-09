using EmployeeBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDetails.Manager
{
    public class EmployeeManager: IEmployeeManager
    {
        private IEmployeeRepository employeeRepository;
        public EmployeeManager(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        public Root GetAll(string criteria = "")
        {
            return employeeRepository.GetAll(criteria);
        }

        public bool AddEmployee(Datum objEmployee)
        {
            return employeeRepository.AddEmployee(objEmployee);
        }

        public bool UpdateEmployee(Datum objEmployee)
        {
            return employeeRepository.UpdateEmployee(objEmployee);
        }

        public List<Datum> Search(string criteria)
        {
            return employeeRepository.Search(criteria);
        }

        public bool DeleteEmployee(int id)
        {
           return employeeRepository.DeleteEmployee(id);
        }

    }
}
