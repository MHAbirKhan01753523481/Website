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
using Alphasoft.IServices;
using System.IO;

namespace Alphasoft.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IUnitOfWork _work;

        private readonly IImagePath _imagePath;
        public BlogsController(IUnitOfWork work, IImagePath imagepath)
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
            Blog blog = new Blog();

            return PartialView("_Create", blog);
        }
        public IActionResult Create(IFormFile image, Blog blog)
        {

            if (ModelState.IsValid)
            {
               

                if (image !=null)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(image.ContentDisposition).FileName.Trim('"').Replace(" ", string.Empty);
                    var path = _imagePath.GetImagePath(fileName, "Blogs", blog.Id.ToString());
                    using (var stream=new FileStream(path, FileMode.Create))
                    {
                        image.CopyTo(stream);
                    }
                    blog.Image = _imagePath.GetImagePathForDb(path);
                   
                }
                _work.Blogs.Add(blog);
                _work.Complete();
                ModelState.Clear();
                blog = new Blog();

                return PartialView("_Create", blog);
            }
           
            return PartialView("_Create", blog);
        }

        public IActionResult EditView(int id)
        {
            var blog = _work.Blogs.Get(id);
            return PartialView("_Edit", blog);
        }


        public IActionResult Edit(IFormFile image,Blog blog)
        {
            var blogsImage = _work.Blogs.Get(blog.Id);

            if (ModelState.IsValid)
            {

                if (image!=null)
                {
                    var fileName=ContentDispositionHeaderValue.Parse(image.ContentDisposition).FileName.Trim('"').Replace(" ",string.Empty);
                    var path = _imagePath.GetImagePath(fileName, "Blogs", blogsImage.Id.ToString());
                    using(var stream=new FileStream(path, FileMode.Create))
                    {
                        image.CopyTo(stream);
                    }
                    blogsImage.Image = _imagePath.GetImagePathForDb(path);
                }
                blogsImage.Id = blog.Id;
                blogsImage.Title = blog.Title;
                blogsImage.Creator = blog.Creator;
                blogsImage.Date = blog.Date;
                blogsImage.Description = blog.Description;
                blogsImage.Order = blog.Order;
                blogsImage.VideoUrl = blog.VideoUrl;

                _work.Blogs.Update(blogsImage);
                _work.Complete();
              
                return PartialView("_Edit", blogsImage);
            }

            return PartialView("_Edit", blogsImage);
        }

        public IActionResult Delete(int id)
        {
          
                var blog = _work.Blogs.Get(id);
                _work.Blogs.Remove(blog);
                _work.Complete();
            return Json("Delete Successfully");
        }
        public IActionResult LoadBlog()
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

            var blog = _work.Blogs.GetAll();

            var blogList = new List<BlogsViewModel>();

            //Sorting    
            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDir))
            {
                blog = blog.AsQueryable().OrderBy(sortColumn + " " + sortColumnDir).ToList();
            }
            else
            {
                blog = blog.OrderByDescending(x => x.Id).ToList();
            }

            //Search    
            if (!string.IsNullOrEmpty(searchValue))
            {
                blog = blog.Where(x => x.Title.Contains(searchValue)).ToList();
            }

            foreach (var item in blog)
            {
                string photoURL = "";
                if (!string.IsNullOrEmpty(item.Image))
                {
                    photoURL = _imagePath.GetFilePathAsSourceUrl(item.Image);
                }
                else
                {
                    photoURL = _imagePath.GetFilePathAsSourceUrl("/images/blogs/");
                }

                blogList.Add(new BlogsViewModel
                {
                    Id = item.Id,
                    Creator=item.Creator,
                    VideoUrl=item.VideoUrl,
                    Date=item.Date,
                  
                    Title=item.Title,
                    Description=item.Description,
                    ChangeDate=item.Date.ToString("MM/dd/yyyy HH:mm:ss")
                });
            }

            //total number of rows count     
            recordsTotal = blogList.Count();

            //Paging     
            var data = blogList.Skip(skip).Take(pageSize).ToList();

            //Returning Json Data    
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }


    }
}