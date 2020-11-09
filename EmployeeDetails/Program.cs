using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using EmployeeBLL;
using EmployeeDetails.Manager;

namespace EmployeeDetails
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // TODO: IoC instead of direct instanciation
            IEmployeeRepository employeeRepository = new EmployeeRepository();
            IEmployeeManager employeeManager = new EmployeeManager(employeeRepository);
            EmployeeForm mainForm = new EmployeeForm(employeeManager);
            Application.Run(mainForm);
        }
    }
}
