using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alphasoft.UnitOfWork;
using Alphasoft.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;
using Alphasoft.Models;

namespace Alphasoft.Controllers
{
    public class ContactusController : Controller
    {

        private readonly IUnitOfWork _work;

        public ContactusController (IUnitOfWork work)
        {
            _work = work;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult CreateView()
        {
            ContactUs contact = new ContactUs();

            return PartialView("_Create", contact);
        }

        public IActionResult Create(ContactUs contact)
        {
            if (ModelState.IsValid)
            {
                _work.ContactUs.Add(contact);
                _work.Complete();

                ModelState.Clear();
                contact = new ContactUs();
                return PartialView("_Create", contact);
            }

            return PartialView("_Create",contact);
        }


        public IActionResult EditView(int id)
        {
            var contactus = _work.ContactUs.Get(id);


          return  PartialView("_Edit", contactus);
        }
        public IActionResult Edit(ContactUs contact)
        {
            if (ModelState.IsValid)
            {
                _work.ContactUs.Update(contact);
                _work.Complete();
                return PartialView("_Edit", contact);
            }
            return PartialView("_Edit", contact);
        }

        public IActionResult Delete(int id)
        {
            if (id !=0)
            {
                var contact = _work.ContactUs.Get(id);
                _work.ContactUs.Remove(contact);
                _work.Complete();
            }
            

            return Json("Delete Successfully");
        }
        public IActionResult LoadContract()
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

            var contractus = _work.ContactUs.GetAll();

            var contactusList = new List<ContactUsViewModel>();

            //Sorting    
            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDir))
            {
                contractus = contractus.AsQueryable().OrderBy(sortColumn + " " + sortColumnDir).ToList();
            }
            else
            {
                contractus = contractus.OrderByDescending(x => x.Id).ToList();
            }

            //Search    
            if (!string.IsNullOrEmpty(searchValue))
            {
                contractus = contractus.Where(x => x.Name.Contains(searchValue) || x.Phone.Contains(searchValue)).ToList();
            }

            foreach (var item in contractus)
            {
                contactusList.Add(new ContactUsViewModel
                {
                    Id = item.Id,
                  
                    Name = item.Name,
                    Phone = item.Phone,
                    Email = item.Email,
                    Message=item.Message,

                });
            }

            //total number of rows count     
            recordsTotal = contactusList.Count();

            //Paging     
            var data = contactusList.Skip(skip).Take(pageSize).ToList();

            //Returning Json Data    
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }
    }
}