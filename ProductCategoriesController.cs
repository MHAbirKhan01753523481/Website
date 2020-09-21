using System;
using System.Collections.Generic;
using System.Linq;
using Alphasoft.Data;
using Alphasoft.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;
using Alphasoft.Models;
using Alphasoft.IServices;

namespace Alphasoft.Controllers
{
    public class ProductCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IImagePath _imagePath;

        public ProductCategoriesController (ApplicationDbContext context, IImagePath imagePath)
        {
            _context = context;
            _imagePath = imagePath;
        }
        //public ProductCategoriesController(ApplicationDbContext context)
        //{
        //    _context = context;
        //}
        public IActionResult Index()
        {
            return View();
        }
        //public IActionResult CategoryList()
        //{
        //    return PartialView("_CategoryList");
        //}

        public IActionResult CreateView()
        {
            ProductCategory productCategories = new ProductCategory();

            return PartialView("_Create", productCategories);
        }
        public IActionResult Create(ProductCategory productcategories)
        {
            if (ModelState.IsValid)
            {
                _context.ProductCategories.Add(productcategories);
                _context.SaveChanges();

                ModelState.Clear();
                productcategories = new ProductCategory();
                return PartialView("_Create", productcategories);
            }
            return PartialView("_Create", productcategories);
        }

        public IActionResult EditView(int id)
        {
            var productCategory = _context.ProductCategories.Find(id);

            return PartialView("_Edit", productCategory);
        }

        public IActionResult Edit(ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                _context.ProductCategories.Update(productCategory);
                _context.SaveChanges();
                return PartialView("_Edit", productCategory);
            }
            return PartialView("_Edit", productCategory);
        }
        public IActionResult Delete(int id)
        {
            var productCategory = _context.ProductCategories.Find(id);
            _context.ProductCategories.Remove(productCategory);
            _context.SaveChanges();
            return Json("Delete Successfully");
        }
        

        public IActionResult LoadproductCategories()
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

            var productCategoies =_context.ProductCategories.ToList();

            var ProductCategorytList = new List<ProductCategoryViewModel>();

            //Sorting    
            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDir))
            {
                productCategoies = productCategoies.AsQueryable().OrderBy(sortColumn + " " + sortColumnDir).ToList();
            }
            else
            {
                productCategoies = productCategoies.OrderByDescending(x => x.Id).ToList();
            }

            //Search    
            if (!string.IsNullOrEmpty(searchValue))
            {
                productCategoies = productCategoies.Where(x => x.Name.Contains(searchValue)).ToList();
            }

            foreach (var item in productCategoies)
            {
                string photoURL = "";

                if (!string.IsNullOrEmpty(item.Image))
                {
                    photoURL = _imagePath.GetFilePathAsSourceUrl(item.Image);
                }
                else
                {
                    photoURL = _imagePath.GetFilePathAsSourceUrl("/images/Products/");
                }
                ProductCategorytList.Add(new ProductCategoryViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                

                });
            }

            //total number of rows count     
            recordsTotal = ProductCategorytList.Count();

            //Paging     
            var data = ProductCategorytList.Skip(skip).Take(pageSize).ToList();

            //Returning Json Data    
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }
    }
}