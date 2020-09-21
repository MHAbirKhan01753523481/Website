using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alphasoft.UnitOfWork;
using Alphasoft.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;
using Alphasoft.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO;
using Alphasoft.IServices;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Alphasoft.Controllers
{
    public class SoftwaresController : Controller
    {
        private readonly IUnitOfWork _work;
        private readonly IImagePath _imagePath;
        public SoftwaresController(IUnitOfWork work, IImagePath imagePath)
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
            ViewData["Name"] = new SelectList(_work.SoftwareCategories.GetAll(), "Id", "Name");
            Software software = new Software();
            return PartialView("_Create", software);
        }
        public IActionResult Create(IFormFile icon, IFormFile image, Software software)
        {
            if (ModelState.IsValid)
            {
                if (icon != null)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(icon.ContentDisposition).FileName.Trim('"').Replace(" ", string.Empty);
                    var path = _imagePath.GetImagePath(fileName, "Software", software.Title.Replace(" ", string.Empty));
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        icon.CopyTo(stream);
                    }
                    software.Icon = _imagePath.GetImagePathForDb(path);
                }
                if (image != null)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(image.ContentDisposition).FileName.Trim('"').Replace(" ", string.Empty);
                    var path = _imagePath.GetImagePath(fileName, "Software", software.Title.Replace(" ", string.Empty));
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        image.CopyTo(stream);
                    }
                    software.Image = _imagePath.GetImagePathForDb(path);
                }
                ViewData["Name"] = new SelectList(_work.SoftwareCategories.GetAll(), "Id", "Name");
                _work.Softwares.Add(software);
                _work.Complete();
                ModelState.Clear();
                software = new Software();
                return PartialView("_Create", software);
            }

            return PartialView("_Create", software);
        }


        public IActionResult EditView(int id)
        {
            ViewData["Name"] = new SelectList(_work.SoftwareCategories.GetAll(), "Id", "Name");
            var software = _work.Softwares.Get(id);
            return PartialView("_Edit", software);
        }

        public IActionResult Edit(IFormFile icon, IFormFile image, Software model)
        {
            var software = _work.Softwares.Get(model.Id);

            if (ModelState.IsValid)
            {
                if (icon != null)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(icon.ContentDisposition).FileName.Trim('"').Replace(" ", string.Empty);
                    var path = _imagePath.GetImagePath(fileName, "Software", software.Title.Replace(" ", string.Empty));
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        icon.CopyTo(stream);
                    }
                    software.Icon = _imagePath.GetImagePathForDb(path);
                }
                if (image != null)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(image.ContentDisposition).FileName.Trim('"').Replace(" ", string.Empty);
                    var path = _imagePath.GetImagePath(fileName, "Software", software.Title.Replace(" ", string.Empty));
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        image.CopyTo(stream);
                    }
                    software.Image = _imagePath.GetImagePathForDb(path);
                }
                software.Name = model.Name;
                software.Title = model.Title;
                software.Description = model.Description;
                software.ShortDescription = model.ShortDescription;
                software.SoftwareCategoryId = model.SoftwareCategoryId;
                software.Order = model.Order;
                _work.Softwares.Update(software);
                _work.Complete();
                return PartialView("_Edit", software);
            }
            ViewData["Name"] = new SelectList(_work.SoftwareCategories.GetAll(), "Id", "Name");
            return PartialView("_Edit", software);
        }

        public IActionResult Delete(int id)
        {
            var software = _work.Softwares.Get(id);
            _work.Softwares.Remove(software);
            _work.Complete();
            return Json("Delete Successfully");
        }
        public IActionResult LoadSoftware()
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

            var software = _work.Softwares.GetAllWithSoftware();

            var softwareList = new List<SoftwareViewModel>();

            //Sorting    
            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDir))
            {
                software = software.AsQueryable().OrderBy(sortColumn + " " + sortColumnDir).ToList();
            }
            else
            {
                software = software.OrderByDescending(x => x.Id).ToList();
            }

            //Search    
            if (!string.IsNullOrEmpty(searchValue))
            {
                software = software.Where(x => x.Title.Contains(searchValue)).ToList();
            }

            foreach (var item in software)
            {
                softwareList.Add(new SoftwareViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Title = item.Title,
                    Order = item.Order,
                    Icon = item.Icon,
                    Image = item.Image,
                    ShortDescription = item.ShortDescription,
                    Description = item.Description,
                    SoftwareCategoryName = item.SoftwareCategory.Name,
                });
            }

            //total number of rows count     
            recordsTotal = softwareList.Count();


            //Paging     
            var data = softwareList.Skip(skip).Take(pageSize).ToList();

            //Returning Json Data    
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }
    }
}