using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alphasoft.UnitOfWork;
using Alphasoft.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Mvc.Rendering;
using Alphasoft.Models;

namespace Alphasoft.Controllers
{
    public class SubMenusController : Controller
    {
        private readonly IUnitOfWork _work;

        public SubMenusController(IUnitOfWork work)
        {
            _work = work;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult CreateView()
        {
            ViewData["Menus"] = new SelectList(_work.Menus.GetAll(), "Id", "Name");
            SubMenu subMenus = new SubMenu();
            return PartialView("_Create", subMenus);
        }

        public IActionResult Create(SubMenu subMenu)
        {
            ViewData["Menus"] = new SelectList(_work.Menus.GetAll(), "Id", "Name");

            if (ModelState.IsValid)
            {
                _work.SubMenus.Add(subMenu);
                _work.Complete();

                ModelState.Clear();
                subMenu = new SubMenu();
                return PartialView("_Create", subMenu);
            }
            

            return PartialView("_Create", subMenu);
        }

        public IActionResult EditView(int id)
        {
            ViewData["Menus"] = new SelectList(_work.Menus.GetAll(), "Id", "Name");
            var submenu = _work.SubMenus.Get(id);
            return PartialView("_Edit", submenu);
        }

        public IActionResult Edit(SubMenu subMenu)
        {
            if (ModelState.IsValid)
            {
                _work.SubMenus.Update(subMenu);
                _work.Complete();
                return PartialView("_Edit", subMenu);
            }
            ViewData["Menus"] = new SelectList(_work.Menus.GetAll(), "Id", "Name");
            return PartialView("_Edit", subMenu);

        }
        public IActionResult Delete(int id)
        {
            if (id != 0)
            {
                var submenu = _work.SubMenus.Get(id);
                _work.SubMenus.Remove(submenu);
                _work.Complete();
            }

            return Json("Delete Successfully");
        }

        public IActionResult LoadSubMenus()
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

            var submenus = _work.SubMenus.GetAllWithMenu();

            var submenusList = new List<SubMenuViewModel>();

            //Sorting    
            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDir))
            {
                submenus = submenus.AsQueryable().OrderBy(sortColumn + " " + sortColumnDir).ToList();
            }
            else
            {
                submenus = submenus.OrderByDescending(x => x.Id).ToList();
            }

            //Search    
            if (!string.IsNullOrEmpty(searchValue))
            {
                submenus = submenus.Where(x => x.Name.Contains(searchValue) || x.Menu.Name.Contains(searchValue)).ToList();
            }

            foreach (var item in submenus)
            {
                submenusList.Add(new SubMenuViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    ControllerName = item.ControllerName,
                    ActionName = item.ActionName,
                    IsActive = item.IsActive,
                    MenuName = item.Menu.Name,
                });
            }

            //total number of rows count     
            recordsTotal = submenusList.Count();

            //Paging     
            var data = submenusList.Skip(skip).Take(pageSize).ToList();

            //Returning Json Data    
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }
    }
}