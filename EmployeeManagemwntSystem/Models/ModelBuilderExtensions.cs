using EmployeeManagementSyatem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagemwntSystem.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
               new Employee
               {
                   Id = 1,
                   Name = "Mary",
                   Department = Dept.IT,
                   Email = "mary@gmail.com"
               },
                new Employee
                {
                    Id = 2,
                    Name = "Jon",
                    Department = Dept.CSE,
                    Email = "jon@gmail.com"
                }
               );
        }
    }
}
