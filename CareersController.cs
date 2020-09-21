using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alphasoft.UnitOfWork;
using Alphasoft.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;
using Alphasoft.Models;

namespace Alphasoft.Controllers
{
    public class CareersController : Controller
    {
        private readonly IUnitOfWork _work;
        public CareersController(IUnitOfWork  work)
        {
            _work = work;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CreateView()
        {
            Career career = new Career();
            return PartialView("_CreateView", career);
        }
        public IActionResult Create(Career careers)
        {
            if (ModelState.IsValid)
            {
                _work.Career.Add(careers);
                _work.Complete();
                ModelState.Clear();
                careers = new Career();
                return PartialView("_CreateView", careers);
            }
            return PartialView("_CreateView", careers);
        }
        public IActionResult EditView(int id)
        {
            var career = _work.Career.Get(id);
            return PartialView("_Edit", career);
        }
        public IActionResult Edit(Career careers)
        {
            if (ModelState.IsValid)
            {
                _work.Career.Update(careers);
                _work.Complete();
                return PartialView("_Edit", careers);
            }
            return PartialView("_Edit", careers);
        }
        public IActionResult Delete(int id)
        {
            var career = _work.Career.Get(id);
            _work.Career.Remove(career);
            _work.Complete();
            return Json("Delete successfully");
        }
        public IActionResult LoadCareers()
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

            var career = _work.Career.GetAll();

            var careerList = new List<CareerVm>();

            //Sorting    
            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDir))
            {
                career = career.AsQueryable().OrderBy(sortColumn + " " + sortColumnDir).ToList();
            }
            else
            {
                career = career.OrderByDescending(x => x.Id).ToList();
            }

            //Search    
            if (!string.IsNullOrEmpty(searchValue))
            {
                career = career.Where(x => x.OportunityDescription.Contains(searchValue)).ToList();
            }

            foreach (var item in career)
            {
                careerList.Add(new CareerVm
                {
                    Id = item.Id,
                    OportunityDescription=item.OportunityDescription,
                    OurBenifitDescription=item.OurBenifiteDescription,
                    OurCultureDescription=item.OurCultureDescription,
                });
            }

            //total number of rows count     
            recordsTotal = careerList.Count();


            //Paging     
            var data = careerList.Skip(skip).Take(pageSize).ToList();

            //Returning Json Data    
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }
    }
}