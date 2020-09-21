using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alphasoft.Data;
using Alphasoft.UnitOfWork;
using Alphasoft.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;
using Alphasoft.Models;

namespace Alphasoft.Controllers
{
    public class HostingPlansController : Controller
    {
        private readonly IUnitOfWork _work;
        private readonly ApplicationDbContext _context;
        public HostingPlansController(IUnitOfWork work, ApplicationDbContext context)
        {
            _work = work;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CreateView()
        {
            HostingPlan hostingplan = new HostingPlan();
            return PartialView("_Create", hostingplan);
        }
        public IActionResult Create(HostingPlan hostingPlan)
        {
            if (ModelState.IsValid)
            {
                _work.HostingPlan.Add(hostingPlan);
                _work.Complete();
                ModelState.Clear();
                hostingPlan = new HostingPlan();
                return PartialView("_Create", hostingPlan);
            }
            return PartialView("_Create", hostingPlan);
        }
        public IActionResult EditView(int id)
        {
            var hostingPlan = _work.HostingPlan.Get(id);
            return PartialView("_Edit", hostingPlan);
        }
        public IActionResult Edit(HostingPlan hostingPlan)
        {
            if (ModelState.IsValid)
            {
                _work.HostingPlan.Update(hostingPlan);
                _work.Complete();
                return PartialView("_Edit", hostingPlan);
            }
            return PartialView("_Edit", hostingPlan);
        }
        public IActionResult Delete(int id)
        {
            var hostingPlan = _work.HostingPlan.Get(id);
            _work.HostingPlan.Remove(hostingPlan);
            _work.Complete();
            return Json("Delete Successfully");
        }
        public IActionResult LoadHostingPlan()
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

            var blog = _work.HostingPlan.GetAll();

            var blogList = new List<HostingPlanViewModel>();

            //Sorting    
            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDir))
            {
                blog = blog.AsQueryable().OrderBy(sortColumn + " " + sortColumnDir).ToList();
            }
            else
            {
                blog = blog.OrderByDescending(x => x.Id).ToList();
            }

            //Search    
            if (!string.IsNullOrEmpty(searchValue))
            {
                blog = blog.Where(x => x.Name.Contains(searchValue)|| x.Space.Contains(searchValue)).ToList();
            }

            foreach (var item in blog)
            {
                blogList.Add(new HostingPlanViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Space = item.Space,
                    Bandwidth = item.Bandwidth,
                    Domain = item.Domain,
                    SubDomain = item.SubDomain,
                    Alias = item.Alias,
                    Email = item.Email,
                    CPanel = item.CPanel,
                    YearlyPrice = item.YearlyPrice,
                    MonthlyPrice = item.MonthlyPrice,
                    PriceUnit = item.PriceUnit,

                });
            }

            //total number of rows count     
            recordsTotal = blogList.Count();

            //Paging     
            var data = blogList.Skip(skip).Take(pageSize).ToList();

            //Returning Json Data    
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }
    }
}