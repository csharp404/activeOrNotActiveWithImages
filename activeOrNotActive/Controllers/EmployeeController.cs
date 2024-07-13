using activeOrNotActive.Data;
using activeOrNotActive.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace activeOrNotActive.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly MyDbContext _dbContext;
        private readonly IWebHostEnvironment env;
        public EmployeeController(MyDbContext x,IWebHostEnvironment webHost)
        {
            _dbContext = x;
            env = webHost;
        }
        public IActionResult Index()
        {
            var emps = _dbContext.employees.Include("Department").ToList();
            return View(emps);
        }
        public IActionResult Active(int id)
        {
            var emp = _dbContext.employees.FirstOrDefault(x => x.Id == id);
            if (emp != null) { 
                emp.IsActive = true;
            _dbContext.employees.Update(emp);
                _dbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public IActionResult NonActive(int id) {

            var emp = _dbContext.employees.FirstOrDefault(x => x.Id == id);
            if (emp != null)
            {
                 emp.IsActive = false;
                _dbContext.employees.Update(emp);
                _dbContext.SaveChanges();
            }
            return RedirectToAction("Index");

        }

        public IActionResult ActiveEmps(int id)
        {
            List<Employee> emps;
            if (id == 0)
            {
                emps = _dbContext.employees.Where(x => x.IsActive == false).Include("Department").ToList();
            }
            else
            {
                emps = _dbContext.employees.Where(x => x.IsActive == true).Include("Department").ToList();
            }

            return View(emps);
        }


        [HttpGet]
        public IActionResult Create() {
            ViewBag.dep = _dbContext.departments.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Employee employee,IFormFile file)
        {
            if (employee != null) { 
            var path = Path.Combine(env.WebRootPath, "Image", file.FileName);

            using (var stream = new FileStream(path,FileMode.Create))
            {
                file.CopyTo(stream);
            }
            employee.imgPath = path;
            employee.imgname = file.FileName;

                _dbContext.employees.Add(employee);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.dep = _dbContext.departments;
            var emp = _dbContext.employees.First(x => x.Id == id);
            if (emp != null) { 
            return View("Create",emp);
            }
            return View();
        }
        [HttpPost]
        public IActionResult Edit(Employee employee,IFormFile file)
        {
            if (employee != null)
            {
                var oldpath = Path.Combine(env.WebRootPath,"Image",employee.imgname);
                var newpath = Path.Combine(env.WebRootPath,"Image",file.FileName);
                if (System.IO.File.Exists(oldpath))
                {
                    System.IO.File.Delete(oldpath);
                }
                using (var stream = new FileStream(newpath,FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                employee.imgname = file.FileName;
                employee.imgPath = newpath;
                _dbContext.employees.Update(employee);
                _dbContext.SaveChanges();
            }
                return RedirectToAction("Index");
        }
        public IActionResult Delete(int id) {
        
        var emp = _dbContext.employees.First(x=>x.Id==id);
            if (emp != null) {
            _dbContext.employees.Remove(emp);
                _dbContext.SaveChanges();
            }
            return RedirectToAction("Index");   
        }

       




    }
}
