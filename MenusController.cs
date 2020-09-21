using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alphasoft.UnitOfWork;
using Alphasoft.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;
using Alphasoft.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Alphasoft.Constants;

namespace Alphasoft.Controllers
{
    public class MenusController : Controller
    {
        private readonly IUnitOfWork _work;

        public MenusController(IUnitOfWork work)
        {
            _work = work;
        }
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult CreateView()
        {
            Menu menu = new Menu();

            ViewData["Menus"] = new SelectList(_work.Menus.GetAll(), "Id", "Name");

            return PartialView("_Create", menu);
        }

        public IActionResult Create(Menu menu)
        {
            ViewData["Menus"] = new SelectList(_work.Menus.GetAll(), "Id", "Name");

            if (ModelState.IsValid)
            {
                _work.Menus.Add(menu);
                _work.Complete();

                ModelState.Clear();
                menu = new Menu();
                return PartialView("_Create", menu);
            }

            return PartialView("_Create", menu);
        }

        public IActionResult EditView(int id)
        {
            var menu = _work.Menus.Get(id);

            ViewData["Menus"] = new SelectList(_work.Menus.GetAll(), "Id", "Name");

            return PartialView("_Edit", menu);
        }

        public IActionResult Edit(Menu menu)
        {
            if (ModelState.IsValid)
            {
                _work.Menus.Update(menu);
                _work.Complete();
                return PartialView("_Edit", menu);
            }
            return PartialView("_Edit", menu);
        }

        public IActionResult Delete(int id)
        {

            if (id != 0)
            {
                var menu = _work.Menus.Get(id);
                _work.Menus.Remove(menu);
                _work.Complete();
            }

            return Json("Delete Successfully!!!");
        }

        public IActionResult LoadMenu()
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

            var menues = _work.Menus.GetAll();

            var menuesList = new List<MenuViewModel>();

            //Sorting    
            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDir))
            {
                menues = menues.AsQueryable().OrderBy(sortColumn + " " + sortColumnDir).ToList();
            }
            else
            {
                menues = menues.OrderByDescending(x => x.Id).ToList();
            }

            //Search    
            if (!string.IsNullOrEmpty(searchValue))
            {
                menues = menues.Where(x => x.Name.Contains(searchValue)).ToList();
            }

            foreach (var item in menues)
            {
                menuesList.Add(new MenuViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Order=item.Order,
                    ControllerName = item.ControllerName,
                    ActionName = item.ActionName,
                    IsActive = item.IsActive,
                    ParentId = item.ParentId,
                    DropdownType = string.IsNullOrEmpty(item.DropdownType) ? string.Empty : ((DropdownList)(int.Parse(item.DropdownType))).ToString()
                });
            }

            //total number of rows count     
            recordsTotal = menuesList.Count();

            //Paging     
            var data = menuesList.Skip(skip).Take(pageSize).ToList();

            //Returning Json Data    
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }

    }
}
