using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alphasoft.Data;
using Alphasoft.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;
using Alphasoft.Models;

namespace Alphasoft.Controllers
{
  
    public class DesignationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DesignationsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateView()
        {
            Designation designation = new Designation();
            return PartialView("_Create", designation);
        }

        public IActionResult Create(Designation designation)
        {
            if (ModelState.IsValid)
            {
                _context.Designations.Add(designation);
                _context.SaveChanges();

                ModelState.Clear();
                designation = new Designation();
                return PartialView("_Create", designation);
            }
            return PartialView("_Create", designation);

        }
        public IActionResult EditView(int id)
        {
            var designation = _context.Designations.Find(id);

            return PartialView("_Edit",designation);
        }
        public IActionResult Edit(Designation designation)
        {
            if (ModelState.IsValid)
            {
                _context.Designations.Update(designation);
                _context.SaveChanges();
                return PartialView("_Edit", designation);
            }

            return PartialView("_Edit", designation);
        }
        public IActionResult Delete(int id)
        {
            var designation = _context.Designations.Find(id);
            _context.Designations.Remove(designation);
            _context.SaveChanges();
            return Json("Delete Successfully");
        }
        public IActionResult LoadDesignations()
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

            var dedsignation =_context.Designations.ToList();

            var designationList = new List<DesignationViewModel>();

            //Sorting    
            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDir))
            {
                dedsignation = dedsignation.AsQueryable().OrderBy(sortColumn + " " + sortColumnDir).ToList();
            }
            else
            {
                dedsignation = dedsignation.OrderByDescending(x => x.Id).ToList();
            }

            //Search    
            if (!string.IsNullOrEmpty(searchValue))
            {
                dedsignation = dedsignation.Where(x => x.Name.Contains(searchValue)).ToList();
            }

            foreach (var item in dedsignation)
            {
                designationList.Add(new DesignationViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Order=item.Order,

                });
            }

            //total number of rows count     
            recordsTotal = designationList.Count();

            //Paging     
            var data = designationList.Skip(skip).Take(pageSize).ToList();

            //Returning Json Data    
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }

    }
}