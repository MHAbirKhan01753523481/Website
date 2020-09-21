using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alphasoft.Models;
using Alphasoft.UnitOfWork;
using Alphasoft.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;
namespace Alphasoft.Controllers
{
    public class FaqsController : Controller
    {
        private readonly IUnitOfWork _work;

        public FaqsController(IUnitOfWork work)
        {
            _work = work;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateView()
        {
            Faq faq = new Faq();

            return PartialView("_Create", faq);
        }
        public IActionResult Create(Faq faq)
        {

            if (ModelState.IsValid)
            {
                _work.Faq.Add(faq);
                _work.Complete();
                ModelState.Clear();
                faq = new Faq();
                return PartialView("_Create", faq);
            }
            return PartialView("_Create",faq);
        }

        public IActionResult EditView(int id)
        {
                var faq = _work.Faq.Get(id);
            return PartialView("_Edit", faq);
        }

        public IActionResult Edit(Faq faq)
        {
            if (ModelState.IsValid)
            {
                _work.Faq.Update(faq);
                _work.Complete();
                return PartialView("_Edit", faq);
            }
            return PartialView("_Edit", faq);
        }
        public IActionResult Delete(int id)
        {
            if (id!=0)
            {
                var faq = _work.Faq.Get(id);
                _work.Faq.Remove(faq);
                _work.Complete();
            }
           
            return Json("Delete Successfully");
        }

        public IActionResult FaqLoad()
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

            var faqs = _work.Faq.GetAll();

            var faqList = new List<FaqViewModel>();

            //Sorting    
            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDir))
            {
                faqs = faqs.AsQueryable().OrderBy(sortColumn + " " + sortColumnDir).ToList();
            }
            else
            {
                faqs = faqs.OrderByDescending(x => x.Id).ToList();
            }

            //Search    
            if (!string.IsNullOrEmpty(searchValue))
            {
                faqs = faqs.Where(x => x.Answer.Contains(searchValue)).ToList();
            }

            foreach (var item in faqs)
            {
                faqList.Add(new FaqViewModel
                {
                 Id=item.Id,
                 Question=item.Question,
                 Answer=item.Answer,
                  
                });
            }

            //total number of rows count     
            recordsTotal = faqList.Count();

            //Paging     
            var data = faqList.Skip(skip).Take(pageSize).ToList();

            //Returning Json Data    
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }

    }
}
