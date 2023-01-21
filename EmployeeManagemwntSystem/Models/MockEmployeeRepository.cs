using EmployeeManagementSyatem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagemwntSystem.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _employeeList;
        public MockEmployeeRepository()
        {
            _employeeList = new List<Employee>()
            {
               new Employee{Id=1,Name="mouli",Department=Dept.CSE,Email="mouli@gmail.com"},
               new Employee{Id=2,Name="Afrin",Department=Dept.IT,Email="afrin@gmail.com"},
                 new Employee{Id=3,Name="Proma",Department=Dept.Playrol,Email="proma@gmail.com"}

            };
        }

        public Employee Add(Employee employee)
        {
            employee.Id= _employeeList.Max(e => e.Id) + 1;
            _employeeList.Add(employee);
            return employee;
        }

        public Employee Delete(int id)
        {
           Employee employee= _employeeList.FirstOrDefault(e => e.Id == id);
            if(employee!=null)
            {
                _employeeList.Remove(employee);
            }
            return employee;
        }

        public IEnumerable<Employee> GetAllEmployee()
        {
           return _employeeList;
        }

        public Employee GetEmployee(int Id)
        {
            return _employeeList.FirstOrDefault(e => e.Id == Id);
        }

        public Employee Update(Employee employeeChanges)
        {
            Employee employee = _employeeList.FirstOrDefault(e => e.Id == employeeChanges.Id);
            if (employee != null)
            {
                employee.Name = employeeChanges.Name;
                employee.Email = employeeChanges.Email;
                employee.Department = employeeChanges.Department;
            }
            return employee;
        }
    }
}
