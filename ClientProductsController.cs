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

namespace Alphasoft.Controllers
{
    public class ClientProductsController : Controller
    {
        private readonly IUnitOfWork _work;

        public ClientProductsController(IUnitOfWork work)
        {
            _work = work;

        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CreateView()
        {
            ViewData["ClientId"] = new SelectList(_work.Client.GetAll(), "Id", "Name");
            ViewData["ProductId"] = new SelectList(_work.Products.GetAll(), "Id", "Name");
            ClientProduct clientproject = new ClientProduct();
            return PartialView("_Create", clientproject);
        }
        public IActionResult Create(ClientProduct clientProject)
        {
            if (ModelState.IsValid)
            {
                _work.ClientProducts.Add(clientProject);
                _work.Complete();
                return PartialView("_Create", clientProject);
            }
            ViewData["ClientId"] = new SelectList(_work.Client.GetAll(), "Id", "Name");
            ViewData["ProductId"] = new SelectList(_work.Products.GetAll(), "Id", "Name");
            return PartialView("_Create", clientProject);
        }
        public IActionResult EditView(int id)
        {
            ViewData["ClientId"] = new SelectList(_work.Client.GetAll(), "Id", "Name");
            ViewData["ProductId"] = new SelectList(_work.Products.GetAll(), "Id", "Name");
            var clientproject = _work.ClientProducts.Get(id);


            return PartialView("_Edit", clientproject);
        }
        public IActionResult Edit(ClientProduct clientProject)
        {
            if (ModelState.IsValid)
            {
                _work.ClientProducts.Update(clientProject);
                _work.Complete();
                return PartialView("_Edit", clientProject);
            }
            ViewData["ClientId"] = new SelectList(_work.Client.GetAll(), "Id", "Name");
            ViewData["ProductId"] = new SelectList(_work.Products.GetAll(), "Id", "Name");
            return PartialView("_Edit", clientProject);
        }
        public IActionResult Delete(int id)
        {
            if (id != 0)
            {
                var clientProject = _work.ClientProducts.Get(id);
                _work.ClientProducts.Remove(clientProject);
                _work.Complete();
            }
            return Json("Delete Successfully");
        }
        public IActionResult LoadClientProjects()
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

            var client = _work.ClientProducts.GetAllWithClientProduct();

            var clientList = new List<ClientsProjectsViewModel>();

            //Sorting    
            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDir))
            {
                client = client.AsQueryable().OrderBy(sortColumn + " " + sortColumnDir).ToList();
            }
            else
            {
                client = client.OrderByDescending(x => x.Id).ToList();
            }
            //Search    
            if (!string.IsNullOrEmpty(searchValue))
            {
                try
                {
                    int searchKey = int.Parse(searchValue);

                    client = client.Where(x => x.ProductId == searchKey).ToList();
                }
                catch (Exception)
                {
                    throw new NotImplementedException();
                }
            }
            foreach (var item in client)
            {
                clientList.Add(new ClientsProjectsViewModel
                {
                    Id = item.Id,
                    ClientName = item.Client.Name,
                    ProductName = item.Product.Name,
                });
            }
            //total number of rows count     
            recordsTotal = clientList.Count();

            //Paging     
            var data = clientList.Skip(skip).Take(pageSize).ToList();

            //Returning Json Data    
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }


    }
}