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
    public class AboutUsesController : Controller
    {
        private IUnitOfWork _work;
        private ApplicationDbContext _context;
        private IImagePath _imagePath;
        public AboutUsesController(IUnitOfWork work, ApplicationDbContext context, IImagePath imagePath)
        {
            _work = work;
            _context = context;
            _imagePath = imagePath;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CreateView()
        {
            AboutUs aboutUs = new AboutUs();
            return PartialView("_Create", aboutUs);
        }
        public IActionResult Create(IFormFile image1,IFormFile image2, IFormFile image3, AboutUs aboutus)
        {
            if (ModelState.IsValid)
            {
                if (image1 != null)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(image1.ContentDisposition).FileName.Trim('"').Replace(" ", string.Empty);
                    var path = _imagePath.GetImagePath(fileName, "AboutUs", aboutus.Id.ToString());
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        image1.CopyTo(stream);
                    }
                    aboutus.WhoWeAreImageOne = _imagePath.GetImagePathForDb(path);
                }
                if (image2 != null)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(image2.ContentDisposition).FileName.Trim('"').Replace(" ", string.Empty);
                    var path = _imagePath.GetImagePath(fileName, "AboutUs", aboutus.Id.ToString());
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        image2.CopyTo(stream);
                    }
                    aboutus.WhoWeAreImageTwo = _imagePath.GetImagePathForDb(path);
                }
                if (image3 != null)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(image3.ContentDisposition).FileName.Trim('"').Replace(" ", string.Empty);
                    var path = _imagePath.GetImagePath(fileName, "AboutUs", aboutus.Id.ToString());
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        image3.CopyTo(stream);
                    }
                    aboutus.WhoWeAreImageThree = _imagePath.GetImagePathForDb(path);
                }
                _work.AboutUs.Add(aboutus);
                _work.Complete();
                ModelState.Clear();
                aboutus = new AboutUs();
                return PartialView("_Create", aboutus);
            }

            return PartialView("_Create", aboutus);
        }
        public IActionResult EditView(int id)
        {
            var aboutus = _work.AboutUs.Get(id);
            return PartialView("_Edit", aboutus);
        }
        public IActionResult Edit(IFormFile image1, IFormFile image2, IFormFile image3, AboutUs aboutus)
        {
            var model = _work.AboutUs.Get(aboutus.Id);
            if (ModelState.IsValid)
            {
                if (image1 != null)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(image1.ContentDisposition).FileName.Trim('"').Replace(" ", string.Empty);
                    var path = _imagePath.GetImagePath(fileName, "AboutUs", model.Id.ToString());
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        image1.CopyTo(stream);
                    }
                    model.WhoWeAreImageOne = _imagePath.GetImagePathForDb(path);
                }
                if (image2 != null)
                {
                    var fileName1 = ContentDispositionHeaderValue.Parse(image2.ContentDisposition).FileName.Trim('"').Replace(" ", string.Empty);
                    var path1 = _imagePath.GetImagePath(fileName1, "AboutUs", model.Id.ToString());
                    using (var stream = new FileStream(path1, FileMode.Create))
                    {
                        image2.CopyTo(stream);
                    }
                    model.WhoWeAreImageTwo = _imagePath.GetImagePathForDb(path1);
                }
                if (image3 != null)
                {
                    var fileName2 = ContentDispositionHeaderValue.Parse(image3.ContentDisposition).FileName.Trim('"').Replace(" ", string.Empty);
                    var path2 = _imagePath.GetImagePath(fileName2, "AboutUs", model.Id.ToString());
                    using (var stream = new FileStream(path2, FileMode.Create))
                    {
                        image3.CopyTo(stream);
                    }
                    model.WhoWeAreImageThree = _imagePath.GetImagePathForDb(path2);
                }
                model.AboutMainSologan = aboutus.AboutMainSologan;
                model.OurMissionDescription = aboutus.OurMissionDescription;
                model.OurVisionDescription = aboutus.OurVisionDescription;
                model.WhyUsDescription = aboutus.WhyUsDescription;
                model.WhoWeAreDescription = aboutus.WhoWeAreDescription;
                model.WhyUsDescription = aboutus.WhyUsDescription;
                model.Loacation = aboutus.Loacation;
                model.Address = aboutus.Address;
                model.CallUs = aboutus.CallUs;
                model.CallUsNumber = aboutus.CallUsNumber;
                model.BusinessHourse = aboutus.BusinessHourse;
                model.BusinessHourseTime = aboutus.BusinessHourseTime;
                model.ClosedTime = aboutus.ClosedTime;
                _work.AboutUs.Update(model);
                _work.Complete();
                return PartialView("_Edit", model);
            }
            return PartialView("_Edit", model);
        }

        public IActionResult Delete(int id)
        {
            var aboutus = _work.AboutUs.Get(id);
            _work.AboutUs.Remove(aboutus);
            _work.Complete(); 
            return Json("Delete Successfully");
        }


        public IActionResult LoadAboutUs()
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

            var aboutus = _work.AboutUs.GetAll();

            var aboutusList = new List<AboutUsViewModel>();

            //Sorting    
            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDir))
            {
                aboutus = aboutus.AsQueryable().OrderBy(sortColumn + " " + sortColumnDir).ToList();
            }
            else
            {
                aboutus = aboutus.OrderByDescending(x => x.Id).ToList();
            }

            //Search    
            if (!string.IsNullOrEmpty(searchValue))
            {
                aboutus = aboutus.Where(x => x.AboutMainSologan.Contains(searchValue)).ToList();
            }

            foreach (var item in aboutus)
            {
                string photoURL = "";
                if (!string.IsNullOrEmpty(item.WhoWeAreImageOne))
                {
                    photoURL = _imagePath.GetFilePathAsSourceUrl(item.WhoWeAreImageOne);
                }
                else
                {
                    photoURL = _imagePath.GetFilePathAsSourceUrl("/images/AboutUs/");
                }
                if (!string.IsNullOrEmpty(item.WhoWeAreImageTwo))
                {
                    photoURL = _imagePath.GetFilePathAsSourceUrl(item.WhoWeAreImageTwo);
                }
                else
                {
                    photoURL = _imagePath.GetFilePathAsSourceUrl("/images/AboutUs");
                }
                aboutusList.Add(new AboutUsViewModel
                {
                    Id = item.Id,
                    AboutMainSologan = item.AboutMainSologan,
                    OurMissionDescription = item.OurMissionDescription,
                    OurVisionDescription = item.OurVisionDescription,
                    WhyUsDescription = item.WhyUsDescription,
                    WhoWeAreDescription = item.WhoWeAreDescription,
                    WhoWeAreImageOne = item.WhoWeAreImageOne,
                    WhoWeAreImageTwo = item.WhoWeAreImageTwo,
                });
            }

            //total number of rows count     
            recordsTotal = aboutusList.Count();


            //Paging     
            var data = aboutusList.Skip(skip).Take(pageSize).ToList();

            //Returning Json Data    
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }
    }
}