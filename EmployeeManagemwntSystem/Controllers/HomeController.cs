using EmployeeManagementSyatem.Models;
using EmployeeManagemwntSystem.Models;
using EmployeeManagemwntSystem.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace EmployeeManagemwntSystem.Controllers
{

    public class HomeController : Controller
    {
        
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHostingEnvironment hostingEnviroment;
        private readonly ILogger logger;

        public HomeController(IEmployeeRepository employeeRepository,
                               IHostingEnvironment hostingEnviroment,
                               ILogger<HomeController> logger)
        {
           
            _employeeRepository = employeeRepository;
            this.hostingEnviroment = hostingEnviroment;
            this.logger = logger;
        }


        public ViewResult Index()
        {
            var model = _employeeRepository.GetAllEmployee();
            return View(model);

        }


        public ViewResult Details(int? id)
        {

            logger.LogTrace("Trace Log");
            logger.LogDebug("Debug Log");
            logger.LogInformation("Information Log");
            logger.LogWarning("Waring Log");
            logger.LogError("Error Log");
            logger.LogCritical("Critical Log");
           // throw new Exception("Error in details view");
            Employee employee = _employeeRepository.GetEmployee(id.Value);
            if (employee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound",id.Value);
            }
           
            HomeDetailsViewModels homeDetailsViewModels = new HomeDetailsViewModels()
            {
               Employee = employee,
              PageTitle = "Employee Details"
            };

            return View(homeDetailsViewModels);
        }
        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Employee employee = _employeeRepository.GetEmployee(id);
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel
            {
                Id=employee.Id,
                Name=employee.Name,
                Email=employee.Email,
                Department=employee.Department,
                ExistingPhotoPath=employee.PhotoPath
            };
            return View(employeeEditViewModel);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee employee = _employeeRepository.GetEmployee(model.Id);
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Department = model.Department;
                if(model.Photos != null)
                {
                    if(model.ExistingPhotoPath!=null)
                    {
                       string filePath= Path.Combine(hostingEnviroment.WebRootPath,
                            "images", model.ExistingPhotoPath);

                        System.IO.File.Delete(filePath);
                    }
                    employee.PhotoPath = ProcessUploadedFile(model);
                }
               
              Employee updatedEmployee =  _employeeRepository.Update(employee);
                return RedirectToAction("index");
            }
            return View(model);
        }

        private string ProcessUploadedFile(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photos != null )
            {
                
                    string uploadFolder = Path.Combine(hostingEnviroment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photos.FileName;
                    string filePath = Path.Combine(uploadFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        model.Photos.CopyTo(fileStream);
                    }
                
                
            }

            return uniqueFileName;
        }

        [HttpPost]
        public IActionResult Create( EmployeeCreateViewModel model)
        {
           if(ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(model);
               
                Employee newEmployee = new Employee {
                    Name = model.Name,
                    Email=model.Email,
                    Department=model.Department,
                    PhotoPath=uniqueFileName
                };
                _employeeRepository.Add(newEmployee);
                return RedirectToAction("details", new { id = newEmployee.Id });
            }
            return View();
        }

    }

        
    
}
