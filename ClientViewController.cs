using Alphasoft.Data;
using Alphasoft.Models;
using Alphasoft.Models.RMSTable;
using Alphasoft.UnitOfWork;
using Alphasoft.ViewModels;
using Alphasoft.ViewModels.ClientViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
namespace Alphasoft.Controllers
{
    public class ClientViewController : Controller
    {
        private readonly IUnitOfWork _work;
        private readonly ApplicationDbContext _context;

        public List<Faq> FaqList { get; private set; }
        public Blog BlogsItem { get; private set; }

        public ClientViewController(IUnitOfWork work, ApplicationDbContext context)
        {
            _work = work;
            _context = context;
        }

        public IActionResult Product(int id)
        {
            var result = _context.BlogComments.ToList();
            ViewBag.comment = result.Count();
            ClientProductVM clientProduct = new ClientProductVM
            {
                blogsComment=_work.BlogComment.GetAllBlogs(),

                Company = _work.Companies.FirstOrDefault(),

                Product = _work.Products.GetProductWithCategory(id),

                Products = _work.Products.GetAll().Where(x => x.Id != id).OrderBy(x=>x.Order).ToList(),

                ProductCategories = _work.ProductCategories.GetAll().OrderBy(x=>x.Order).ToList(),

                ClientProjects = _work.ClientProducts.ClientProjectsByProduct(id)
            };

            return View(clientProduct);
        }

        public IActionResult Faq()
        {

            FaqVm faqvm = new FaqVm
            {
                Faqs = _work.Faq.GetAll().OrderBy(x => x.Order).ToList(),

            };
            return View(faqvm);
        }

        public IActionResult Blogs()
        {
            BlogsVM blogsVM = new BlogsVM
            {
                Blogs = _work.Blogs.GetAll().OrderBy(x=>x.Order).ToList(),
                singleblogs=_work.Blogs.GetWithSingleImage(),
                lastImage=_work.Blogs.GetWithLatImage(),
            };

            return View(blogsVM);
        }
       public IActionResult BlogsDetails(int id)
        {
            var result = _context.BlogComments.ToList();
            ViewBag.comment = result.Count();
      
            BlogsVM blogsDetailsvm = new BlogsVM
            {
              BlogsItem=_work.Blogs.Get(id),
              blogcomment=_work.BlogComment.GetAll(),
              commentCount=_work.BlogComment.GetWithAllComment()
            };
            return View(blogsDetailsvm);
        }

        public IActionResult RatingView()
        {
            return PartialView("_Rating");
        }

        [HttpPost]
        public IActionResult UpdateRatingReview(BlogComment ratingComment)
        {
            if (ModelState.IsValid)
            {
                var ratingObj = _work.BlogComment.Get(ratingComment.Id);
                //if (ratingObj.REmail == ratingComment.REmail)
                //{
                //    return Json(false);
                //}
                if (ratingObj != null)
                {
                    ratingObj.RName = ratingComment.RName == ratingComment.RName ? ratingComment.RName : ratingObj.RName;
                    //ratingObj.REmail = ratingComment.REmail == ratingComment.REmail ? ratingComment.REmail : ratingObj.REmail;
                    ratingObj.Rating = ratingComment.Rating == ratingComment.Rating ? ratingComment.Rating : ratingObj.Rating;
                    ratingObj.Review = ratingComment.Review != null ? ratingComment.Review : ratingObj.Review;
                    _work.Complete();

                    if (ratingObj.Id > 0)
                    {
                        return Json(true);
                    }
                }
            }
            return Json(false);
        }

        public IActionResult UpdateCommentDetails(int id)
        {
            var reply = _work.BlogComment.Get(id);
         
            return PartialView("_ReplyComment", reply);
        }

        public IActionResult UpdateComment(BlogComment blogComment)
        {

            var reply = _work.BlogComment.Get(blogComment.Id);
            if (reply !=null)
            {
                reply.RName = blogComment.RName !=null ? blogComment.RName:reply.RName;
                reply.REmail = blogComment.REmail !=null ? blogComment.REmail:reply.REmail;
                reply.Reply = blogComment.Reply !=null ? blogComment.Reply:reply.Reply;
                _work.BlogComment.Update(reply);
                _work.Complete();

                return PartialView("_ReplyComment", reply);

            }
            return PartialView("_ReplyComment", blogComment);

        }
        public IActionResult BlogsCommentCreateView(int id)
        {
            BlogsVM blogsDetailsvm = new BlogsVM
            {
                BlogsItem = _work.Blogs.Get(id),
                blogcomment = _work.BlogComment.GetAll(),
                commentCount = _work.BlogComment.GetWithAllComment()
              
            };

            return PartialView("_CommentView", blogsDetailsvm);
        }

        public IActionResult BlogsCommentView()
        {
            ViewData["BlogId"] = new SelectList(_work.Blogs.GetAll(), "Id", "Id");

           // ViewData["BlogId"] = new SelectList(_work.Products.GetAll(), "Id", "Id");

            BlogsVM blogsDetailsvm = new BlogsVM();
           
            return PartialView("_CommentView", blogsDetailsvm);
        }

        public IActionResult BlogsCommentCreate(BlogsVM blogsVM)
        {
             
       

            if (ModelState.IsValid)
            {
                BlogComment blogComment = new BlogComment
                {
               
                    FullName = blogsVM.FullName,
                    Email = blogsVM.Email,
                    Comment = blogsVM.Comment,
                    BlogId=blogsVM.BlogId,
                };
                _work.BlogComment.Add(blogComment);
                _work.Complete();

                ViewData["BlogId"] = new SelectList(_work.Blogs.GetAll(), "Id", "Id");
                return PartialView("_CommentView", blogsVM);
            }
            return PartialView("_CommentView", blogsVM);
        }
        public IActionResult OurTeam()
        {
            OurTeamVM teamVm = new OurTeamVM
            {
                Teams = _work.OurTeams.GetAllWithDepartmentAndDesignation().OrderBy(x=>x.Order).ToList()
            };

            return View(teamVm);
        }

        public IActionResult TeamViewDetails(int id)
        {
            OurTeamVM teamVm = new OurTeamVM
            {
                Team = _work.OurTeams.GetWithDepartmentDesignation(id),
                Teams = _work.OurTeams.GetAllWithDepartmentAndDesignation()
            };

            teamVm.Teams = teamVm.Teams.Where(x => x.Id != id && x.DepartmentId == teamVm.Team.DepartmentId).ToList();

            return View(teamVm);
        }
        public IActionResult AboutUs()
        {
            AboutUsVm aboutUsVm = new AboutUsVm
            {

                Company = _work.Companies.FirstOrDefault(),

                aboutUs = _work.AboutUs.GetWithAboutUs(),

                about = _work.AboutUs.GetWithAbout(),

                OurTeamvm = _work.OurTeams.TeamWithUs(),

                Teams = _work.OurTeams.GetAllWithDepartmentAndDesignation(),

                TotalSoftware = _work.Softwares.GetAll(),

                clientSayus=_work.Client.GetAll().TakeLast(3).ToList(),
              

            };
           

            aboutUsVm.teamVmList = _work.OurTeams.GetAllWithDepartmentAndDesignation().OrderBy(x => x.Order).Select(x => new OurTeamVM() { Name=x.Name,DepartmentName = x.Department.Name.ToString(),DesignationName=x.Designation.Name.ToString(),Image=x.Image,Description=x.Description,Facebook=x.Facebook,Twitter=x.Twitter,LinkedIn=x.LinkedIn}).ToList();
            return View(aboutUsVm);
        }

        public IActionResult Clients()
        {
            var clients = _work.Client.GetAll();
            return View(clients);
        }
        public IActionResult Reservations(int id)
        {
            var result = DateTime.Now.ToString("dddd dd MMMMM yyyy");
            ViewBag.times = result;
       
            var result1 = DateTime.Now.ToString("dddd dd MMMMM yyyy");
            ViewBag.times1 = result1;

            ProductsViewModel productsViewModel = new ProductsViewModel
            {
                Products = _work.Products.GetAllWithCategory().OrderBy(x=>x.Order).ToList(),
                category=_work.ProductCategories.GetAll().ToList(),
                singleblogs = _work.Blogs.GetWithSingleImage(),
            };

            productsViewModel.PopularProducts = _work.QueryHelper.GetPopularProducts(productsViewModel.Products);
            productsViewModel.CategoryProducts = _work.QueryHelper.GetCategoryProducts(productsViewModel.Products);

            if (id != 0)
            {
                productsViewModel.Products = _work.QueryHelper.GetCategoryWiseProducts(productsViewModel.Products, id);
            }

            return View(productsViewModel);
        }

        public IActionResult Services()
        {
            ServiceVM serviceVM = new ServiceVM
            {
                Services = _work.Services.GetAll(),
                ServiceCategories = _work.ServiceCategories.GetAll(),
                 Client = _work.Client.GetAll(),
                 blogs=_work.Blogs.GetAll(),
            };
            return View(serviceVM);
        }

        public IActionResult Career()
        {
            return View();
        }

        public IActionResult Section_1()
        {
            var whyChooseUs = _work.ChooseUs.GetAll();
            return PartialView("_Section1", whyChooseUs);
        }

        public IActionResult Section_2()
        {
            var ourClients = _work.Client.GetAll();
            return PartialView("_Section2", ourClients);
        }

        public IActionResult Section_3()
        {
            var popularProducts = _work.Products.GetAllWithCategory();

            popularProducts = popularProducts.Where(x => x.IsPopular == true).ToList();

            return PartialView("_Section3", popularProducts);
        }

        public IActionResult Section_4()
        {
            var hostingPlans = _work.HostingPlan.GetAll();
            return PartialView("_Section4", hostingPlans);
        }


        public IActionResult Section_5()
        {
            var ourProducts = _work.Products.GetAllWithCategory();

            return PartialView("_Section5", ourProducts);
        }

        public IActionResult Section_6()
        {
            return PartialView("_Section6");
        }

        //public IActionResult Section_7()
        //{
        //    return PartialView("_Section7");
        //}
        public IActionResult ContractUsCreate(ContactUs contact)
        {
            if (ModelState.IsValid)
            {
                _work.ContactUs.Add(contact);
                _work.Complete();

                ModelState.Clear();
                contact = new ContactUs();
                return PartialView("_Section6", contact);
            }

            return PartialView("_Section6", contact);
        }
        public IActionResult ProductContarctUs(ContactUs contact)
        {
            if (ModelState.IsValid)
            {
                _work.ContactUs.Add(contact);
                _work.Complete();

                ModelState.Clear();
                contact = new ContactUs();
                return View(contact);
            }

            return View(contact);
        }

        public IActionResult CategoryProducts(int id)
        {
            var products = _work.Products.GetCategoryWiseProducts(id);
            return PartialView("_CategoryWiseProducts", products);
        }

        public IActionResult GetReservation(int id)
        {
            var products = _work.Products.GetCategoryWiseProducts(id);
            return PartialView("_ReservationProduct", products);
        }




        public IActionResult CategoryWiseProduct(int id)
        {
            ProductsViewModel productsViewModel = new ProductsViewModel
            {
                Products = _work.Products.GetAllWithCategory(),
                CategoryWiseProducts = _work.Products.GetCategoryWiseProducts(id)
            };

            productsViewModel.PopularProducts = _work.QueryHelper.GetPopularProducts(productsViewModel.Products);
            productsViewModel.CategoryProducts = _work.QueryHelper.GetCategoryProducts(productsViewModel.Products);

            return View(productsViewModel);
        }

      

        public IActionResult GetCategoryWiseProducts(int id, int number = 8)
        {
            List<Product> products = new List<Product>();

            if (id == 0)
            {
                products = _work.Products.GetAllWithCategory().TakeLast(number).ToList();
                return PartialView("_GetCategoryWiseProducts", products);
            }

            products = _work.Products.GetCategoryWiseProducts(id).TakeLast(number).ToList();
            return PartialView("_GetCategoryWiseProducts", products);
        }
       
        public IActionResult CategoryWiseSoftware(int id)
        {
            SoftwareViewModel SoftwareViewVM = new SoftwareViewModel
            {
                SingleSoftware = _work.Softwares.GetAllWithFeatureAndCategory(id),
                Client = _work.Client.GetAll()
            };
            return View(SoftwareViewVM);

        }
        public IActionResult Pronali()
        {
            PronaliViewModel viewModel = new PronaliViewModel
            {
                Blogs = _work.Blogs.GetAll().OrderBy(x => x.Order).ToList(),
                whyChouseus = _work.ChooseUs.FirstOrDefault(),

                features = _work.Features.GetAll(),
                softwareCategory = _work.SoftwareCategories.GetAll(),
                software = _work.Softwares.GetAll(),
                client = _work.Client.GetAll(),
                service = _work.Services.FirstOrDefault(),
                aboutUs=_work.AboutUs.FirstOrDefault(),
                company=_work.Companies.FirstOrDefault(),
                servicAll=_work.Services.GetAll()
            };
            return View(viewModel);
        }

        public IActionResult ContactUs()
        {

            ContactUsViewModel contact = new ContactUsViewModel
            {
                aboutUs = _work.AboutUs.GetAll(),
                Services = _work.Services.GetAll(),
                about = _work.AboutUs.GetWithAbout(),
                companyInfo = _work.Companies.FirstOrDefault(),
                bannerList = _work.Banner.GetAll().Take(3).ToList(),
            };
            return View(contact);
        }

        public IActionResult SaveContactIndex()
        {

            return View();
        }
        public IActionResult SaveContact(ContactUsViewModel contactUsViewModel)
        {
            if (ModelState.IsValid)
            {
                ContactUs contact = new ContactUs()
                {
                    Name=contactUsViewModel.Name,
                    Email=contactUsViewModel.Email,
                    Subject=contactUsViewModel.Subject,
                    Message=contactUsViewModel.Message,
                    Phone=contactUsViewModel.Phone
                };

                _work.ContactUs.Add(contact);
                _work.Complete();

                return Json(true);
            }
            return Json(false);
        }

        //public IActionResult ProductView(int id)
        //{
        //    ProductsViewModel productsViewModel = new ProductsViewModel
        //    {
        //        Products = _work.Products.GetAllWithCategory().OrderBy(x => x.Order).ToList(),
        //    };

        //    productsViewModel.PopularProducts = _work.QueryHelper.GetPopularProducts(productsViewModel.Products);
        //    productsViewModel.CategoryProducts = _work.QueryHelper.GetCategoryProducts(productsViewModel.Products);

        //    if (id != 0)
        //    {
        //        productsViewModel.Products = _work.QueryHelper.GetCategoryWiseProducts(productsViewModel.Products, id);
        //    }

        //    return View(productsViewModel);
        //}
        //public IActionResult ProductAdd(int id)
        //{
        //    var result = _context.BlogComments.ToList();
        //    ViewBag.comment = result.Count();
        //    ClientProductVM clientProduct = new ClientProductVM
        //    {
        //        blogsComment = _work.BlogComment.GetAllBlogs(),

        //        Company = _work.Companies.FirstOrDefault(),

        //        Product = _work.Products.GetProductWithCategory(id),

        //        Products = _work.Products.GetAll().Where(x => x.Id != id).OrderBy(x => x.Order).ToList(),

        //        ProductCategories = _work.ProductCategories.GetAll().OrderBy(x => x.Order).ToList(),

        //        ClientProjects = _work.ClientProducts.ClientProjectsByProduct(id)
        //    };

        //    return View(clientProduct);
        //}

        //public IActionResult Reservation(int id)
        //{
        //    ProductsViewModel productsViewModel = new ProductsViewModel
        //    {
        //        Products = _work.Products.GetAllWithCategory().OrderBy(x => x.Order).ToList(),
        //    };

        //    productsViewModel.PopularProducts = _work.QueryHelper.GetPopularProducts(productsViewModel.Products);
        //    productsViewModel.CategoryProducts = _work.QueryHelper.GetCategoryProducts(productsViewModel.Products);

        //    if (id != 0)
        //    {
        //        productsViewModel.Products = _work.QueryHelper.GetCategoryWiseProducts(productsViewModel.Products, id);
        //    }

        //    return View(productsViewModel);
        //}
        //public IActionResult ProductSoft(int id)
        //{
        //    var result = _context.BlogComments.ToList();
        //    ViewBag.comment = result.Count();
        //    ClientProductVM clientProduct = new ClientProductVM
        //    {
        //        blogsComment = _work.BlogComment.GetAllBlogs(),

        //        Company = _work.Companies.FirstOrDefault(),

        //        Product = _work.Products.GetProductWithCategory(id),

        //        Products = _work.Products.GetAll().Where(x => x.Id != id).OrderBy(x => x.Order).ToList(),

        //        ProductCategories = _work.ProductCategories.GetAll().OrderBy(x => x.Order).ToList(),

        //        ClientProjects = _work.ClientProducts.ClientProjectsByProduct(id)
        //    };

        //    return View(clientProduct);
        //}
        //public IActionResult GetDropDownReservationList()
        //{
        //    var result = _context.Table.ToList();

        //    return Json(result);
        //}



       public IActionResult ReservationPartial()
        {

            return PartialView("_ReservationAddToCardPopup");
        }

        private int AvailableTable(DateTime Entry )
        {
            DateTime start;
            if( Entry != null)
            {
                start = Entry;
            }
            else 
            {
                start =DateTime.Now;
            }

            //var number = _context.ReservedTables.Where(c => c.BookingEndTime <= start).ToList();
            //int table = 0 ;
            
            //for (int i = 0; i < number.Count; i++)
            //{
            //    table += number[i].ReservedTableNumber;
            //}
            var BookedTable = _context.ReservedTables.Where(c => c.BookingEndTime >= start).ToList();
            int Booked = 0;
            for (int i = 0; i < BookedTable.Count; i++)
            {
                Booked += BookedTable[i].ReservedTableNumber;
            }
            var All = _context.Table.ToList();
            return All[0].AllChir - Booked;
        }

        
        public IActionResult GetAvailableTable(DateTime date )
        {
            int Table = 0; 
            Table = AvailableTable(date) ;
            return Json(Table);
        }



        public IActionResult SaveReserVation(ProductsViewModel productsView)
        {
         


            if (ModelState.IsValid)
            {
                Reservation reservation = new Reservation()
                {
                    ReservationType=productsView.ReservationType,
                    Adult=productsView.Adult,
                    Child=productsView.Child,
                    Date =productsView.DateTo !=null ? productsView.Date :DateTime.Now,

                    //ChoseTimeStart =productsView.ChoseTimeStartTo !=null ? productsView.ChoseTimeStart :DateTime.Now,
                    //ChoseTimeEnd=productsView.ChoseTimeEndTo !=null ? productsView.ChoseTimeEnd :DateTime.Now,
                    FirstName=productsView.FirstName,
                    LastName=productsView.LastName,
                    DeliveryAddress=productsView.DeliveryAddress,
                    Address=productsView.Address,
                    City=productsView.City,
                    Country=productsView.Country,
                    ZipCode=productsView.ZipCode,
                    Email=productsView.Email,
                    Mobile=productsView.Mobile,
                  
                };
                _context.Reservations.Add(reservation);
                _context.SaveChanges();
                bool IsSaved = false;

                var tableNumber = _context.TableDetails.OrderByDescending(c => c.Id).Take(1).ToList();
                int NumberofTable = 0;
                if(tableNumber.Count == 0 )
                {
                    var AllTable = _context.Table.ToList();
                    NumberofTable = AllTable[0].AllChir;
                }
                else
                {
                    NumberofTable = tableNumber[0].AvaiableSeat;
                }
                //var Available = _context.ReservedTables.OrderByDescending(c=>c.Id).Take(1).ToList();
                //int TotalTable = AvailableTable(System.DateTime.Now);
                ReservedTable reservedTable = new ReservedTable
                {
                    BookingStartTime = productsView.ChoseTimeStartTo != null ? productsView.ChoseTimeStart : DateTime.Now,
                    BookingEndTime = productsView.ChoseTimeEndTo != null ? productsView.ChoseTimeEnd : DateTime.Now,
                    ReservedTableNumber = productsView.BookingTableNumber,
                    TableId = 1,
                };
                _context.ReservedTables.Add(reservedTable);
                _context.SaveChanges();
                TableDetails tableDetails = new TableDetails()
                {
                    ChoseTimeStart = productsView.ChoseTimeStartTo != null ? productsView.ChoseTimeStart : DateTime.Now,

                    ChoseTimeEnd = productsView.ChoseTimeEndTo != null ? productsView.ChoseTimeEnd : DateTime.Now,

                    OrderSeat = productsView.OrderSeat,

                    AvaiableSeat = NumberofTable - productsView.BookingTableNumber,
                    TableId = 1,

                };
                _context.TableDetails.Add(tableDetails);
                 IsSaved = _context.SaveChanges() > 0;

                for (int i = 0; i < productsView.ProductNames.Count; i++)
                {
                    ReservationDetail reservationDetail = new ReservationDetail
                    {
                        ReservationId = reservation.Id,
                        Name = productsView.ProductNames[i],
                        Price = productsView.ProductPrices[i],
                        Quantity = productsView.ProductQuantities[i],
                        TotalPrice= productsView.TotalPrices,
                    };
                    _context.ReservationDetails.Add(reservationDetail);
                    IsSaved = _context.SaveChanges() > 0 ;
                }



                return Json(IsSaved == true ? true: false);
            }
            return Json(false);
        }

    }
}