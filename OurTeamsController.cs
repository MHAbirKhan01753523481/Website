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
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using Alphasoft.IServices;
using System.IO;

namespace Alphasoft.Controllers
{
    public class OurTeamsController : Controller
    {
        private readonly IImagePath _imagePath;

        private readonly IUnitOfWork _work;
        public OurTeamsController(IUnitOfWork work, IImagePath imagepath)
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
            OurTeam ourteam = new OurTeam();
            ViewData["Departments"] = new SelectList(_work.Departments.GetAll(), "Id", "Name");
            ViewData["Designation"] = new SelectList(_work.Designations.GetAll(), "Id", "Name");
          
            return PartialView("_Create", ourteam);
        }
        public IActionResult Create(IFormFile image, OurTeam ourTeam)
        {
            ViewData["Departments"] = new SelectList(_work.Departments.GetAll(), "Id", "Name");
            ViewData["Designation"] = new SelectList(_work.Designations.GetAll(), "Id", "Name");
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(image.ContentDisposition).FileName.Trim('"').Replace(" ", string.Empty);
                    var path = _imagePath.GetImagePath(fileName, "OurTeam", ourTeam.Name.Replace(" ", string.Empty));
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        image.CopyTo(stream);
                    }
                    ourTeam.Image = _imagePath.GetImagePathForDb(path);
                }

                _work.OurTeams.Add(ourTeam);
                _work.Complete();
                ModelState.Clear();
                ourTeam = new OurTeam();
                return PartialView("_Create", ourTeam);
            }
            
            return PartialView("_Create", ourTeam);
        }

        public IActionResult EditView(int id)
        {
            ViewData["Departments"] = new SelectList(_work.Departments.GetAll(), "Id", "Name");
            ViewData["Designation"] = new SelectList(_work.Designations.GetAll(), "Id", "Name");
            var ourTeam = _work.OurTeams.Get(id);
          

            return PartialView("_Edit", ourTeam);
        }
        public IActionResult Edit(IFormFile image, OurTeam ourTeam)
        {
          
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(image.ContentDisposition).FileName.Trim('"').Replace(" ", string.Empty);
                    var path = _imagePath.GetImagePath(fileName, "OurTeam", ourTeam.Name.Replace(" ", string.Empty));
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        image.CopyTo(stream);
                    }
                    ourTeam.Image = _imagePath.GetImagePathForDb(path);
                }
                if (image == null)
                {
                    var teamImage = _work.OurTeams.Get(ourTeam.Id);
                    teamImage.Name = ourTeam.Name;
                    teamImage.DepartmentId = ourTeam.DepartmentId;
                    teamImage.DesignationId = ourTeam.DesignationId;
                    teamImage.Description = ourTeam.Description;
                    teamImage.Facebook = ourTeam.Facebook;
                    teamImage.Twitter=ourTeam.Twitter;
                    _work.OurTeams.Update(teamImage);
                    _work.Complete();
                    return PartialView("_Edit", ourTeam);

                }
                _work.OurTeams.Update(ourTeam);
                _work.Complete();
                return PartialView("_Edit", ourTeam);
            }
            return PartialView("_Edit", ourTeam);
        }
        public IActionResult Delete(int id)
        {
            if (id !=0)
            {
                var ourTeam = _work.OurTeams.Get(id);
                _work.OurTeams.Remove(ourTeam);
                _work.Complete();
            }
            return Json("Delete Successfully");
        }
        public IActionResult LoadOurTeam()
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

            var ourteam = _work.OurTeams.GetAllWithDepartmentAndDesignation();
       
            var ourTeamList = new List<OurTeamViewModel>();

            //Sorting    
            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDir))
            {
                ourteam = ourteam.AsQueryable().OrderBy(sortColumn + " " + sortColumnDir).ToList();
            }
            else
            {
                ourteam = ourteam.OrderByDescending(x => x.Id).ToList();
            }

            //Search    
            if (!string.IsNullOrEmpty(searchValue))
            {
                ourteam = ourteam.Where(x => x.Name.Contains(searchValue)).ToList();
            }

            foreach (var item in ourteam)
            {
                string photoURL = "";
                if (!string.IsNullOrEmpty(item.Image))
                {
                    photoURL = _imagePath.GetFilePathAsSourceUrl(item.Image);
                }
                else
                {
                    photoURL = _imagePath.GetFilePathAsSourceUrl("/images/OurTeam/MHAnwarHossainAbirannnoreara.jpg");
                }
                ourTeamList.Add(new OurTeamViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Order=item.Order,
                    Image = item.Image,
                    Description = item.Description,
                    Facebook = item.Facebook,
                    Twitter = item.Twitter,
                    LinkedIn = item.LinkedIn,
                    DepartmentName =item.Department.Name,
                   DesignationName=item.Designation.Name,
                 
                 
                });
            }

            //total number of rows count     
            recordsTotal = ourTeamList.Count();


            //Paging     
            var data = ourTeamList.Skip(skip).Take(pageSize).ToList();

            //Returning Json Data    
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }


    }
}