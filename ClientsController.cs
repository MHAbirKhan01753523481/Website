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
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using Alphasoft.IServices;
using System.IO;

namespace Alphasoft.Controllers
{

    public class ClientsController : Controller
    {

        private readonly IUnitOfWork _work;
        private readonly IImagePath _imagePath;
        public ClientsController(IUnitOfWork work, IImagePath imagePath)
        {
            _work = work;
            _imagePath = imagePath;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CreateView()
        {
            Client client = new Client();
            return PartialView("_Create", client);
        }

        public IActionResult Create(IFormFile file, Client client)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"').Replace(" ", string.Empty);
                    var path = _imagePath.GetImagePath(fileName, "Clients", client.Name.Replace(" ", string.Empty));
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    client.Logo = _imagePath.GetImagePathForDb(path);
                }

                _work.Client.Add(client);

                _work.Complete();

                ModelState.Clear();

                client = new Client();

                return PartialView("_Create", client);
            }

            return PartialView("_Create", client);
        }

        public IActionResult EditView(int id)
        {

            var client = _work.Client.Get(id);


            return PartialView("_Edit", client);
        }
        public IActionResult Edit(IFormFile file, Client client)
        {
            if (ModelState.IsValid)
            {

                if (file != null)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"').Replace(" ", string.Empty);
                    var path = _imagePath.GetImagePath(fileName, "Clients", client.Name.Replace(" ", string.Empty));
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    client.Logo = _imagePath.GetImagePathForDb(path);
                }
                _work.Client.Update(client);
                _work.Complete();
                return PartialView("_Edit", client);
            }
            return PartialView("_Edit", client);
        }

        public IActionResult Delete(int id)
        {
            if (id != 0)
            {
                var client = _work.Client.Get(id);
                _work.Client.Remove(client);
                _work.Complete();
            }
            return Json("Delete Successfully");
        }
        public IActionResult LoadClient()
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

            var client = _work.Client.GetAll();

            var clientList = new List<ClientViewModel>();

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
                client = client.Where(x => x.Name.Contains(searchValue)).ToList();
            }

            foreach (var item in client)
            {
                clientList.Add(new ClientViewModel
                {
                    Id = item.Id,
                    Logo = item.Logo,
                    Name = item.Name,
                    Email = item.Email,
                    Address = item.Address,
                    Phone = item.Phone,
                   
                    ClientSaysAboutUs = item.ClientSaysAboutUs,

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