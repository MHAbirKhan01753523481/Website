using System;
using System.Collections.Generic;
using System.Linq;
using Alphasoft.UnitOfWork;
using Alphasoft.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;
using Alphasoft.Models;

namespace Alphasoft.Controllers
{
    public class JobsController : Controller
    {
        private readonly IUnitOfWork _work;
        public JobsController(IUnitOfWork work)
        {
            _work = work;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CreateView()
        {
            Job job = new Job();
            return PartialView("_Create", job);
        }
        public IActionResult Create(Job job)
        {
            if (ModelState.IsValid)
            {
                _work.Job.Add(job);
                _work.Complete();
                ModelState.Clear();
                job = new Job();
                return PartialView("_Create", job);
            }
            return PartialView("_Create", job);
        }

        public IActionResult EditView(int id)
        {
            var job = _work.Job.Get(id);
            return PartialView("_Edit", job);
        }
        public IActionResult Edit(Job job)
        {
            if (ModelState.IsValid)
            {
                _work.Job.Update(job);
                _work.Complete();

                return PartialView("_Edit", job);
            }
            return PartialView("_Edit", job);
        }
        public IActionResult Delete(int id)
        {
            var job = _work.Job.Get(id);
            _work.Job.Remove(job);
            _work.Complete();
            return Json("Delete Successfuilly");
        }
        public IActionResult LoadJob()
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
            var jobs = _work.Job.GetAll();
            var jobsList = new List<JobVm>();
            //Sorting    
            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDir))
            {
                jobs = jobs.AsQueryable().OrderBy(sortColumn + " " + sortColumnDir).ToList();
            }
            else
            {
                jobs = jobs.OrderByDescending(x => x.Id).ToList();
            }
            //Search    
            if (!string.IsNullOrEmpty(searchValue))
            {
                jobs = jobs.Where(x => x.Location.Contains(searchValue)).ToList();
            }
            foreach (var item in jobs)
            {
                jobsList.Add(new JobVm
                {
                    Id = item.Id,
                    Title = item.Title,
                    Order=item.Order,
                    Description = item.Description,
                    Qualification=item.Qualification,
                    Location=item.Location,
                    JobCreateDate=item.JobCreateDate,
                    DeadLineTo=item.DeadLine.ToString("dddd dd MMMMM yyyy"),
                    JobCreateDateTo=item.JobCreateDate.ToString("dddd dd MMMMM yyyy"),

                }) ;
            }
            //total number of rows count     
            recordsTotal = jobsList.Count();
            //Paging     
            var data = jobsList.Skip(skip).Take(pageSize).ToList();
            //Returning Json Data    
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }
    }
}