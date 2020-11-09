using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EmployeeBLL;

namespace EmployeeUnitTest
{
    [TestClass]
    public class TestEmployeeRepository
    {
        EmployeeRepository employeeRepository;

        public TestEmployeeRepository()
        {
            employeeRepository = new EmployeeRepository();
        }

        [TestMethod]
        public void EmployeeRepository_GetAll()
        {
            Root loginList = employeeRepository.GetAll();
            Assert.IsTrue(loginList.data.Count>0,"Employee list is not empty.");
        }

        [TestMethod]
        public void EmployeeRepository_AddEmployee()
        {
            var guidID = Guid.NewGuid().ToString();
            Datum employee = new Datum();
            employee.name = guidID;
            employee.email = guidID + "@test.com";
            employee.gender = "Male";
            employee.status = "Active";
            employee.created_at = DateTime.Now;
            employee.updated_at = DateTime.Now;
            var result = employeeRepository.AddEmployee(employee);
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void EmployeeRepository_UpdateEmployee()
        {
            Root loginList = employeeRepository.GetAll();
            var guidID = Guid.NewGuid().ToString();
            Datum employee = new Datum();
            employee.id = loginList.data.Count - 1;
            employee.name = guidID + "@test.com";
            employee.email = guidID + "@test.com";
            employee.gender = "Male";
            employee.status = "Active";
            employee.created_at = DateTime.Now;
            employee.updated_at = DateTime.Now;
            var result = employeeRepository.UpdateEmployee(employee);
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void EmployeeRepository_DeleteEmployee()
        {
            Root loginList = employeeRepository.GetAll();
            int id = loginList.data.Count - 1;
            var result = employeeRepository.DeleteEmployee(id);
            Assert.AreEqual(true, result);
        }
    }
}
