using activeOrNotActive.Data;
using activeOrNotActive.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace activeOrNotActive.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly MyDbContext _dbContext;
        public DepartmentController(MyDbContext x)
        {
            _dbContext = x;
        }
        public IActionResult Index()
        {

            var dep = _dbContext.departments.Include("Employees").ToList();

            return View(dep);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Department department)
        {
            if (department != null)
            {
                _dbContext.departments.Add(department);
                _dbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id) { 
        var dep = _dbContext.departments.Include("Employees").First(x => x.Id == id);

            if(dep != null && dep.Employees!=null)
            {
                _dbContext.employees.RemoveRange(dep.Employees);
                _dbContext.departments.Remove(dep);
                _dbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        
        }

        [HttpGet]
        public IActionResult Edit(int id) {

            var dep = _dbContext.departments.First(x=>x.Id == id);  
            return View("Create", dep);
        }
        [HttpPost]
        public IActionResult Edit(Department department) { 
            _dbContext.departments.Update(department);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        
        }

    }
}
