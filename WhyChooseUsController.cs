using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alphasoft.Data;
using Alphasoft.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;
using Alphasoft.Models;
using Alphasoft.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Alphasoft.IServices;
using System.Net.Http.Headers;
using System.IO;

namespace Alphasoft.Controllers
{
    public class WhyChooseUsController : Controller
    {
        private readonly IUnitOfWork _work;

        private readonly IImagePath _imagePath;

        public WhyChooseUsController(IUnitOfWork work, IImagePath imagePath)
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
            ChooseUs chose = new ChooseUs();
            return PartialView("_Create", chose);
        }
        public IActionResult Create(IFormFile image,ChooseUs choseUs)
        {
            if (ModelState.IsValid)
            {
             
                if (image !=null)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(image.ContentDisposition).FileName.Trim('"').Replace(" ", string.Empty);
                    var path = _imagePath.GetImagePath(fileName, "ChoseUs", choseUs.Id.ToString());
                    using (var stream=new FileStream(path,FileMode.Create))
                    {
                        image.CopyTo(stream);
                    }
                    choseUs.Image = _imagePath.GetImagePathForDb(path);
                }
                _work.ChooseUs.Add(choseUs);
                _work.Complete();
                ModelState.Clear();
                choseUs = new ChooseUs();
                return PartialView("_Create", choseUs);
            }
            return PartialView("_Create", choseUs);
        }

        public IActionResult EditView(int id)
        {
            var choseus =_work.ChooseUs.Get(id);
            return PartialView("_Edit",choseus);

        }
        public IActionResult Edit(IFormFile image, ChooseUs choseUs)
        {
            var chouseUsImage = _work.ChooseUs.Get(choseUs.Id);
            if (ModelState.IsValid)
            {

                if (image!=null)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(image.ContentDisposition).FileName.Trim('"').Replace(" ", string.Empty);
                    var path = _imagePath.GetImagePath(fileName, "ChouseUs", chouseUsImage.Id.ToString());
                    using (var stream=new FileStream(path, FileMode.Create))
                    {
                        image.CopyTo(stream);
                    }
                    chouseUsImage.Image = _imagePath.GetImagePathForDb(path);

                }
                chouseUsImage.Id = choseUs.Id;
                chouseUsImage.Title = choseUs.Title;
                chouseUsImage.ShortDescription = choseUs.ShortDescription;
                chouseUsImage.Order = choseUs.Order;
                chouseUsImage.IsActive = choseUs.IsActive;
                _work.ChooseUs.Update(chouseUsImage);
                _work.Complete();

                return PartialView("_Edit", chouseUsImage);
            }
            return PartialView("_Edit", chouseUsImage);
        }
        public IActionResult Delete(int id)
        {
            var choseus =_work.ChooseUs.Get(id);
            _work.ChooseUs.Remove(choseus);
            _work.Complete();
            return Json("Delete Successfully");
        }
        public IActionResult LoadChoseUs()
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

            var choseus = _work.ChooseUs.GetAll();

            var choseusList = new List<ChoseUsViewModel>();

            //Sorting    
            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDir))
            {
                choseus = choseus.AsQueryable().OrderBy(sortColumn + " " + sortColumnDir).ToList();
            }
            else
            {
                choseus = choseus.OrderByDescending(x => x.Id).ToList();
            }

            //Search    
            if (!string.IsNullOrEmpty(searchValue))
            {
                choseus = choseus.Where(x => x.Title.Contains(searchValue)).ToList();
            }

            foreach (var item in choseus)
            {
                choseusList.Add(new ChoseUsViewModel
                {
                    Id = item.Id,
                    Title=item.Title,
                    ShortDescription=item.ShortDescription,
                    Description=item.Description,
                  
                    IsActive=item.IsActive,

                });
            }

            //total number of rows count     
            recordsTotal = choseusList.Count();

            //Paging     
            var data = choseusList.Skip(skip).Take(pageSize).ToList();

            //Returning Json Data    
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }
    }
}