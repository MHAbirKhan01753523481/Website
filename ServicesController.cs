using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Alphasoft.Data;
using Alphasoft.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Alphasoft.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Alphasoft.UnitOfWork;

namespace Alphasoft.Controllers
{
    public class ServicesController : Controller
    {
        private readonly IUnitOfWork _work;

        public ServicesController(IUnitOfWork work)
        {
            _work = work;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateView()
        {
            Service service = new Service();

            ViewData["ServiceCategories"] = new SelectList(_work.ServiceCategories.GetAll(), "Id", "Name");

            return PartialView("_Create", service);
        }

        public IActionResult Create(Service service)
        {
            if (ModelState.IsValid)
            {
                _work.Services.Add(service);

                _work.Complete();

                ModelState.Clear();

                service = new Service();

                ViewData["ServiceCategories"] = new SelectList(_work.ServiceCategories.GetAll(), "Id", "Name");

                return PartialView("_Create", service);
            }

            return PartialView("_Create", service);
        }

        public IActionResult EditView(int id)
        {
            var service = _work.Services.Get(id);

            ViewData["ServiceCategories"] = new SelectList(_work.ServiceCategories.GetAll(), "Id", "Name");

            return PartialView("_Edit", service);
        }

        public IActionResult Edit(Service service)
        {
            if (ModelState.IsValid)
            {
                _work.Services.Update(service);

                _work.Complete();

                return PartialView("_Edit", service);
            }
            return PartialView("_Edit", service);
        }

        public IActionResult Delete(int id)
        {
            var service = _work.Services.Get(id);

            _work.Services.Remove(service);

            _work.Complete();

            return Json("Deleted Successfully!");
        }

        public IActionResult LoadServices()
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

            var serviceCategories = _work.Services.GetAllWithServiceCategory();

            var serviceCategoryList = new List<ServicesViewModel>();

            //Sorting    
            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDir))
            {
                serviceCategories = serviceCategories.AsQueryable().OrderBy(sortColumn + " " + sortColumnDir).ToList();
            }
            else
            {
                serviceCategories = serviceCategories.OrderByDescending(x => x.Id).ToList();
            }

            //Search    
            if (!string.IsNullOrEmpty(searchValue))
            {
                serviceCategories = serviceCategories.Where(x => x.Name.Contains(searchValue) || x.ServiceCategory.Name.Contains(searchValue)).ToList();
            }

            foreach (var item in serviceCategories)
            {
                serviceCategoryList.Add(new ServicesViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Image = item.Image,
                    Price = item.Price,
                    Description = item.Description,
                    ShortDescription = item.ShortDescription,
                    ServiceCategoryName = item.ServiceCategory.Name,
                    Order=item.Order,
                    
                });
            }

            //total number of rows count     
            recordsTotal = serviceCategoryList.Count();

            //Paging     
            var data = serviceCategoryList.Skip(skip).Take(pageSize).ToList();

            //Returning Json Data    
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }
    }
}