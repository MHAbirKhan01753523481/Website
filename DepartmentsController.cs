using System;
using System.Collections.Generic;
using System.Linq;
using Alphasoft.Data;
using Alphasoft.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;
using Alphasoft.Models;

namespace Alphasoft.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UnitOfWork.UnitOfWork _work;

        public DepartmentsController(ApplicationDbContext context)
        {
            _context = context;
            _work = new UnitOfWork.UnitOfWork(_context);
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateView()
        {

            Department department = new Department();
            return PartialView("_Create", department);
        }

        public IActionResult Create(Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Departments.Add(department);
                _context.SaveChanges();

                ModelState.Clear();
                department = new Department();
                return PartialView("_Create", department);
            }
            return PartialView("_Create", department);
        }


        public IActionResult EditView(int id)
        {
            Department department = _context.Departments.Find(id);

            return PartialView("_Edit", department);
        }


        public IActionResult Edit(Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Departments.Update(department);
                _context.SaveChanges();

                return PartialView("_Edit", department);
            }
          
            return PartialView("_Edit", department);
        }

        public IActionResult Delete(int id)
        {
            var department = _context.Departments.Find(id);
            _context.Departments.Remove(department);
            _context.SaveChanges();
            return Json("Delete Successfully!");
        }

        public IActionResult LoadDepartment()
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDir = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            var department = _work.Departments.GetAll();

            var departmentList = new List<DepartmentViewModel>();

            //Sorting    
            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDir))
            {
                department = department.AsQueryable().OrderBy(sortColumn + " " + sortColumnDir).ToList();
            }
            else
            {
                department = department.OrderByDescending(x => x.Id).ToList();
            }

            //Search    
            if (!string.IsNullOrEmpty(searchValue))
            {
                department = department.Where(x => x.Name.Contains(searchValue)).ToList();
            }

            foreach (var item in department)
            {
                departmentList.Add(new DepartmentViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Order=item.Order,

                });
            }

            //total number of rows count     
            recordsTotal = departmentList.Count();

            //Paging     
            var data = departmentList.Skip(skip).Take(pageSize).ToList();

            //Returning Json Data    
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }
    }
}