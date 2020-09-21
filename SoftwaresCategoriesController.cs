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

namespace Alphasoft.Controllers
{
    public class SoftwaresCategoriesController : Controller
    {
        private readonly IUnitOfWork _work;
        private readonly IImagePath _imagePath;
        public SoftwaresCategoriesController(IUnitOfWork work, IImagePath imagePath)
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
            SoftwareCategory softwareCategory = new SoftwareCategory();
            return PartialView("_Create", softwareCategory);
        }
        public IActionResult Create(IFormFile image, SoftwareCategory softwareCategory)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(image.ContentDisposition).FileName.Trim('"').Replace(" ", string.Empty);
                    var path = _imagePath.GetImagePath(fileName, "Softwarecategory", softwareCategory.Name.Replace(" ", string.Empty));
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        image.CopyTo(stream);
                    }
                    softwareCategory.Image = _imagePath.GetImagePathForDb(path);
                }
               
                _work.SoftwareCategories.Add(softwareCategory);
                _work.Complete();
                ModelState.Clear();
                softwareCategory = new SoftwareCategory();
                return PartialView("_Create", softwareCategory);
            }
            return PartialView("_Create", softwareCategory);
        }


        public IActionResult EditView(int id)
        {
            var company = _work.SoftwareCategories.Get(id);
            return PartialView("_Edit", company);
        }

        public IActionResult Edit(IFormFile image, SoftwareCategory model)
        {
            var softwareCategory = _work.SoftwareCategories.Get(model.Id);

            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(image.ContentDisposition).FileName.Trim('"').Replace(" ", string.Empty);
                    var path = _imagePath.GetImagePath(fileName, "SoftwareCategory", softwareCategory.Name.Replace(" ", string.Empty));
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        image.CopyTo(stream);
                    }
                    softwareCategory.Image = _imagePath.GetImagePathForDb(path);
                }

                softwareCategory.Name = model.Name;
                softwareCategory.Order = model.Order;
                _work.SoftwareCategories.Update(softwareCategory);
                _work.Complete();
                return PartialView("_Edit", softwareCategory);
            }
            return PartialView("_Edit", softwareCategory);
        }

        public IActionResult Delete(int id)
        {
            var softwareCategory = _work.SoftwareCategories.Get(id);
            _work.SoftwareCategories.Remove(softwareCategory);
            _work.Complete();
            return Json("Delete Successfully");
        }
        public IActionResult LoadSoftwareCategories()
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

            var software = _work.SoftwareCategories.GetAll();

            var softwareList = new List<SoftwareCategoriesViewModel>();

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
                software = software.Where(x => x.Name.Contains(searchValue)).ToList();
            }

            foreach (var item in software)
            {
                softwareList.Add(new SoftwareCategoriesViewModel
                {
                    Id = item.Id,
                    Image = item.Image,
                    Name = item.Name,
                    Thumbnail=item.Thumbnail,
                    Order = item.Order
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