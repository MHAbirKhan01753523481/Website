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
using Alphasoft.Services;
using System.IO;
using Alphasoft.IServices;

namespace Alphasoft.Controllers
{
    public class CompaniesController : Controller
    {

        private readonly IUnitOfWork _work;
        private readonly IImagePath _imagePath;

        public CompaniesController(IUnitOfWork work, IImagePath imagePath)
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
            Company company = new Company();
            return PartialView("_Create", company);
        }

        public IActionResult Create(IFormFile logo, IFormFile favicon, Company company)
        {
            if (ModelState.IsValid)
            {
                if (logo != null)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(logo.ContentDisposition).FileName.Trim('"').Replace(" ", string.Empty);
                    var path = _imagePath.GetImagePath(fileName, "Company", company.Name.Replace(" ", string.Empty));
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        logo.CopyTo(stream);
                    }
                    company.Logo = _imagePath.GetImagePathForDb(path);
                }
                if (favicon != null)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(favicon.ContentDisposition).FileName.Trim('"').Replace(" ", string.Empty);
                    var path = _imagePath.GetImagePath(fileName, "Company", company.Name.Replace(" ", string.Empty));
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        favicon.CopyTo(stream);
                    }
                    company.Favicon = _imagePath.GetImagePathForDb(path);
                }


                _work.Companies.Add(company);
                _work.Complete();
                ModelState.Clear();
                company = new Company();
                return PartialView("_Create", company);
            }

            return PartialView("_Create", company);
        }


        public IActionResult EditView(int id)
        {
            var company = _work.Companies.Get(id);
            return PartialView("_Edit", company);
        }

        public IActionResult Edit(IFormFile logo, IFormFile favicon, Company model)
        {
            var company = _work.Companies.Get(model.Id);

            if (ModelState.IsValid)
            {
                if (logo != null)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(logo.ContentDisposition).FileName.Trim('"').Replace(" ", string.Empty);
                    var path = _imagePath.GetImagePath(fileName, "Company", company.Name.Replace(" ", string.Empty));
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        logo.CopyTo(stream);
                    }
                    company.Logo = _imagePath.GetImagePathForDb(path);
                }
                if (favicon != null)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(favicon.ContentDisposition).FileName.Trim('"').Replace(" ", string.Empty);
                    var path = _imagePath.GetImagePath(fileName, "Company", company.Name.Replace(" ", string.Empty));
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        favicon.CopyTo(stream);
                    }
                    company.Favicon = _imagePath.GetImagePathForDb(path);
                }

                company.Name = model.Name;
                company.Email = model.Email;
                company.Slogan = model.Slogan;
                company.Phone = model.Phone;
                company.Address = model.Address;
                company.Facebook = model.Facebook;
                company.LinkedIn = model.LinkedIn;
                company.Twitter = model.Twitter;
                company.Youtube = model.Youtube;
                company.ShortDescription = model.ShortDescription;
                company.Description = model.Description;
                company.GoogleMapLocation = model.GoogleMapLocation;

                _work.Companies.Update(company);
                _work.Complete();
                return PartialView("_Edit", company);
            }

            return PartialView("_Edit", company);
        }

        public IActionResult Delete(int id)
        {
            if (id != 0)
            {
                var company = _work.Companies.Get(id);
                _work.Companies.Remove(company);
                _work.Complete();

            }
            return Json("Delete Successfully");
        }
        public IActionResult LoadCompany()
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

            var company = _work.Companies.GetAll();

            var companyList = new List<CompanyViewModel>();

            //Sorting    
            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDir))
            {
                company = company.AsQueryable().OrderBy(sortColumn + " " + sortColumnDir).ToList();
            }
            else
            {
                company = company.OrderByDescending(x => x.Id).ToList();
            }

            //Search    
            if (!string.IsNullOrEmpty(searchValue))
            {
                company = company.Where(x => x.Name.Contains(searchValue)).ToList();
            }

            foreach (var item in company)
            {
                companyList.Add(new CompanyViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Logo = item.Logo,
                    Slogan = item.Slogan,
                    Address = item.Address,
                    Phone = item.Phone,
                    Email = item.Email,
                    Facebook = item.Facebook,
                    LinkedIn = item.LinkedIn,
                    Twitter = item.Twitter,
                    Youtube = item.Youtube,
                    Favicon = item.Favicon,
                    ShortDescription = item.ShortDescription,
                    Description = item.Description,
                    GoogleMapLocation = item.GoogleMapLocation
                });
            }

            //total number of rows count     
            recordsTotal = companyList.Count();

            //Paging     
            var data = companyList.Skip(skip).Take(pageSize).ToList();

            //Returning Json Data    
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }
    }
}