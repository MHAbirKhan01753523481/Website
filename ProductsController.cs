using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Alphasoft.IServices;
using Alphasoft.Models;
using Alphasoft.UnitOfWork;
using Alphasoft.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Alphasoft.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IUnitOfWork _work;
        private readonly IImagePath _imagePath;

        public ProductsController(IUnitOfWork work, IImagePath imagePath)
        {
            _work = work;
            _imagePath = imagePath;
        }



        public IActionResult Index()
        {
            return View();
        }

       
        //public IActionResult ProductList()
        //{
        //    return PartialView("_ProductList");
        //}
        //public IActionResult GetDropDown()
        //{
        //    var result = _work.ProductCategories.GetAll().ToList();
        //    return Json(result);
        //}

        public IActionResult CreateView()
        {
            Product product = new Product();

            ViewData["ProductCategories"] = new SelectList(_work.ProductCategories.GetAll(), "Id", "Name");

            return PartialView("_Create", product);
        }

        [HttpPost]
        public IActionResult Create(IFormFile image, Product product)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(image.ContentDisposition).FileName.Trim('"').Replace(" ", string.Empty);

                    var path = _imagePath.GetImagePath(fileName, "Products", product.Name.Replace(" ", string.Empty));

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                         image.CopyToAsync(stream);
                    }

                    product.Image = _imagePath.GetImagePathForDb(path);
                }

                _work.Products.Add(product);

                _work.Complete();

                ModelState.Clear();

                product = new Product();

                //ViewData["ProductCategories"] = new SelectList(_work.ProductCategories.GetAll(), "Id", "Name");

                return PartialView("_Create", product);
            }

            return PartialView("_Create", product);
        }

        public IActionResult EditView(int id)
        {
            Product product = _work.Products.Get(id);

            ViewData["ProductCategories"] = new SelectList(_work.ProductCategories.GetAll(), "Id", "Name");

            return PartialView("_Edit", product);
        }

        public IActionResult Edit(IFormFile image, Product productmodel)
        {
            var product = _work.Products.Get(productmodel.Id);

            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(image.ContentDisposition).FileName.Trim('"').Replace(" ", string.Empty);

                    var path = _imagePath.GetImagePath(fileName, "Products", productmodel.Name.Replace(" ", string.Empty));

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        image.CopyTo(stream);
                    }

                    product.Image = _imagePath.GetImagePathForDb(path);
                }

                product.Name = productmodel.Name;
                product.PurchasePrice = productmodel.PurchasePrice;
                product.RetailPrice = productmodel.RetailPrice;
                product.WholeSellPrice = productmodel.WholeSellPrice;
                product.Discount = productmodel.Discount;
                product.Description = productmodel.Description;
                product.ShortDescription = productmodel.ShortDescription;
                product.ProductCategoryId = productmodel.ProductCategoryId;
                product.IsPopular = productmodel.IsPopular;

                _work.Products.Update(product);

                _work.Complete();

                return PartialView("_Edit", product);
            }
            ViewData["ProductCategories"] = new SelectList(_work.ProductCategories.GetAll(), "Id", "Name");
            return PartialView("_Edit", product);
        }

        public IActionResult Delete(int id)
        {
            var employee = _work.Products.Get(id);

            _work.Products.Remove(employee);

            _work.Complete();

            return Json("Deleted Successfully!");
        }

        public IActionResult LoadProducts()
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

            var products = _work.Products.GetAllWithCategory();

            var productList = new List<ProductListViewModel>();

            foreach (var item in products)
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
                productList.Add(new ProductListViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Order = item.Order,
                    CategoryName = item.ProductCategory.Name,
                    PurchasePrice = item.PurchasePrice,
                    RetailPrice = item.RetailPrice,
                    WholeSellPrice = item.WholeSellPrice,
                    Discount = item.Discount,
                    Description = item.Description,
                    ShortDescription = item.ShortDescription,
                    ReleaseDate = item.ReleaseDate,
                    Image = item.Image,
                    IsPopular = item.IsPopular
                });
            }
            //Sorting    
            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDir))
            {
                products = products.AsQueryable().OrderBy(sortColumn + " " + sortColumnDir).ToList();
            }
            else
            {
                products = products.OrderByDescending(x => x.Id).ToList();
            }

            //Search    
            if (!string.IsNullOrEmpty(searchValue))
            {
                products = products.Where(x => x.Name.Contains(searchValue) || x.ProductCategory.Name.Contains(searchValue)).ToList();
            }

           

            //total number of rows count     
            recordsTotal = productList.Count();

            //Paging     
            var data = productList.Skip(skip).Take(pageSize).ToList();

            //Returning Json Data    
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }
    }
}