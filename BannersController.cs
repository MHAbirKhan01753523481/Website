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
using System.Net.Http.Headers;
using Alphasoft.IServices;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Alphasoft.Controllers
{
    public class BannersController : Controller
    {
        private readonly IImagePath _imagePath;
    
        private readonly IUnitOfWork _work;
        public BannersController(IUnitOfWork work,IImagePath imagepath)
        {
          
            _work = work;
            _imagePath= imagepath;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateView()
        {
            Banner banner = new Banner();
            return PartialView("_Create", banner);
        }
        public IActionResult Create(IFormFile image, Banner banner)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(image.ContentDisposition).FileName.Trim('"').Replace(" ", string.Empty);
                    var path = _imagePath.GetImagePath(fileName, "Banners", banner.Id.ToString());
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        image.CopyTo(stream);
                    }
                    banner.Image = _imagePath.GetImagePathForDb(path);
                }
                _work.Banner.Add(banner);
                _work.Complete();
                ModelState.Clear();
                banner = new Banner();
                return PartialView("_Create", banner);
            }

            return PartialView("_Create", banner);
        }

     
        public IActionResult EditView(int id)
        {        
            var banner = _work.Banner.Get(id);
            return PartialView("_Edit", banner);
        }
        public IActionResult Edit(IFormFile image, Banner banner)
        {
            if (ModelState.IsValid)
            {
                if (image!= null)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(image.ContentDisposition).FileName.Trim('"').Replace(" ", string.Empty);
                    var path = _imagePath.GetImagePath(fileName, "Banners", banner.Id.ToString());
                    using (var stream = new FileStream(path, FileMode.Create))
                    {  
                            image.CopyTo(stream);
                    }
                    banner.Image = _imagePath.GetImagePathForDb(path);
                  
                }
                if (image == null)
                {
                    var bannerimage = _work.Banner.Get(banner.Id);

                    bannerimage.SolganOne = banner.SolganOne;
                    bannerimage.SolganTwo = banner.SolganTwo;
                    bannerimage.SolganThree = banner.SolganThree;
                    bannerimage.Order = banner.Order;

                    _work.Banner.Update(bannerimage);
                    _work.Complete();
                    return PartialView("_Edit", banner);
                }
                _work.Banner.Update(banner);
                _work.Complete();
              
                return PartialView("_Edit", banner);
            }

            return PartialView("_Edit", banner);
        }
        public IActionResult Delete(int id)
        {
            var banner = _work.Banner.Get(id);
            _work.Banner.Remove(banner);
            _work.Complete();
            return Json("Delete Successfully");
        }
        public IActionResult LoadBanner()
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

            var banners = _work.Banner.GetAll();

            var BannerList = new List<BannerViewModel>();

            foreach (var item in banners)
            {
                string photoURL = "";
                if (!string.IsNullOrEmpty(item.Image))
                {
                    photoURL = _imagePath.GetFilePathAsSourceUrl(item.Image);
                }
                else
                {
                    photoURL = _imagePath.GetFilePathAsSourceUrl("/images/Banners/");
                }
                BannerList.Add(new BannerViewModel
                {
                    Id = item.Id,
                    Image = item.Image,
                    SolganOne = item.SolganOne,
                    SolganTwo = item.SolganTwo,
                    SolganThree = item.SolganThree,
                    Order = item.Order,
                });
            }
            //Sorting    
            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDir))
            {
                banners = banners.AsQueryable().OrderBy(sortColumn + " " + sortColumnDir).ToList();
            }
            else
            {
                banners = banners.OrderByDescending(x => x.Id).ToList();
            }

            //Search    
            if (!string.IsNullOrEmpty(searchValue))
            {
                banners = banners.Where(x => x.SolganOne.Contains(searchValue) || x.SolganTwo.Contains(searchValue) || x.SolganThree.Contains(searchValue)).ToList();
            }

           

            //total number of rows count     
            recordsTotal = BannerList.Count();

            //Paging     
            var data = BannerList.Skip(skip).Take(pageSize).ToList();

            //Returning Json Data    
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }

    }
}