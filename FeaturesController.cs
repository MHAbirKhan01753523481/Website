using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alphasoft.UnitOfWork;
using Alphasoft.ViewModels.ClientViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;
using Alphasoft.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO;
using Alphasoft.IServices;

namespace Alphasoft.Controllers
{
    public class FeaturesController : Controller
    {
        private readonly IImagePath _imagePath;

        private readonly IUnitOfWork _work;
        public FeaturesController(IUnitOfWork work, IImagePath imagepath)
        {

            _work = work;
            _imagePath = imagepath;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CreateView()
        {
            ViewData["Name"] = new SelectList(_work.Softwares.GetAll(), "Id", "Name");
            Features features = new Features();

            return PartialView("_Create", features);

        }
        public IActionResult Create(IFormFile image, Features features)
        {
           
            if (ModelState.IsValid)
            {

                if (image != null)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(image.ContentDisposition).FileName.Trim('"').Replace(" ", string.Empty);
                    var path = _imagePath.GetImagePath(fileName, "Banners", features.Id.ToString());
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        image.CopyTo(stream);
                    }
                    features.IconImage= _imagePath.GetImagePathForDb(path);
                }

                _work.Features.Add(features);
                _work.Complete();
                return PartialView("_Create", features);
            }
            ViewData["Name"] = new SelectList(_work.Softwares.GetAll(), "Id", "Name");
            return PartialView("_Create", features);
        }

        public IActionResult EditView(int id)
        {
            ViewData["Name"] = new SelectList(_work.Softwares.GetAll(), "Id", "Name");
            var features = _work.Features.Get(id);
            return PartialView("_Edit", features);
        }
        public IActionResult Edit(IFormFile image, Features features)
        {
            var featuresImage = _work.Features.Get(features.Id);
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(image.ContentDisposition).FileName.Trim('"').Replace(" ", string.Empty);
                    var path = _imagePath.GetImagePath(fileName, "Fetures", featuresImage.Id.ToString());
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        image.CopyTo(stream);
                    }
                    featuresImage.IconImage = _imagePath.GetImagePathForDb(path);
                }
                featuresImage.Id = features.Id;
                featuresImage.Name = features.Name;
                featuresImage.ShortDescription = features.ShortDescription;
                featuresImage.SoftwareId = features.SoftwareId;
                featuresImage.Icon=features.Icon;
                _work.Features.Update(featuresImage);
                _work.Complete();

                return PartialView("_Edit", featuresImage);
            }
            return PartialView("_Edit", featuresImage);
        }

        public IActionResult Delete(int id)
        {
            var features = _work.Features.Get(id);

            _work.Features.Remove(features);
            _work.Complete();

            return Json("Delete Successfully");
        }
        public IActionResult LoadFeatures()
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

            var features = _work.Features.GetAll();

            var featuresList = new List<FeaturesViewModel>();

            //Sorting    
            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDir))
            {
                features = features.AsQueryable().OrderBy(sortColumn + " " + sortColumnDir).ToList();
            }
            else
            {
                features = features.OrderByDescending(x => x.Id).ToList();
            }

            //Search    
            if (!string.IsNullOrEmpty(searchValue))
            {
                features = features.Where(x => x.Name.Contains(searchValue) || x.Icon.Contains(searchValue)).ToList();
            }

            foreach (var item in features)
            {
                featuresList.Add(new FeaturesViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Icon = item.Icon,
                    IconImage=item.IconImage,
                    ShortDescription = item.ShortDescription,
                  
                });
            }

            //total number of rows count     
            recordsTotal = featuresList.Count();


            //Paging     
            var data = featuresList.Skip(skip).Take(pageSize).ToList();

            //Returning Json Data    
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }
    }
}