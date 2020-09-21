using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alphasoft.Data;
using Alphasoft.IServices;
using Alphasoft.UnitOfWork;
using Alphasoft.ViewModels.DataTableVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alphasoft.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly IImagePath _imagePath;

        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _work;
        public ReservationsController(IUnitOfWork work, IImagePath imagepath, ApplicationDbContext context)
        {
            _context = context;
            _work = work;
            _imagePath = imagepath;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Delete(int id)
        {
          
            var reservation = _context.Reservations.Find(id);
           
                _context.Reservations.Remove(reservation);
                _context.SaveChanges();
                return Json(true);
        


           
        }


        public IActionResult LoadReservation()
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

            var reservationList = _context.ReservationDetails.Include(x => x.Reservation).ToList();

            List<vmReservationDetails> reservationItem =new List<vmReservationDetails>();

            foreach (var item in reservationList)
            {
                //string photoURL = "";
                //if (!string.IsNullOrEmpty(item.Reservation.))
                //{
                //    photoURL = _imagePath.GetFilePathAsSourceUrl(item.WhoWeAreImageOne);
                //}
                //else
                //{
                //    photoURL = _imagePath.GetFilePathAsSourceUrl("/images/AboutUs/");
                //}
                reservationItem.Add(new vmReservationDetails
                {
                    Id=item.Id,
                    FirstName=item.Reservation.FirstName,
                    LastName=item.Reservation.LastName,
                    
                    Mobile=item.Reservation.Mobile,
                    Address=item.Reservation.Address,
                    Email=item.Reservation.Email,
                    Adult=item.Reservation.Adult,
                    Child=item.Reservation.Child,
                    Children=item.Reservation.Children,
                    City=item.Reservation.City,
                    Country=item.Reservation.Country,
                    //ChoseTimeStartTo=item.Reservation.ChoseTimeStart.ToString("dddd dd MMMMM yyyy"),
                    //ChoseTimeEndTo=item.Reservation.ChoseTimeEnd.ToString("dddd dd MMMMM yyyy"),
                    Name=item.Name,
                    Quantity=item.Quantity,
                    Price=item.Price,
                   TotalPrice=item.TotalPrice,

                });
            }


            //Sorting    
            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDir))
            {
                //reservationItem = reservationItem.AsQueryable().OrderBy(sortColumn + " " + sortColumnDir).ToList();
            }
            else
            {
                reservationItem = reservationItem.OrderByDescending(x => x.Id).ToList();
            }

            //Search    
            if (!string.IsNullOrEmpty(searchValue))
            {
                reservationItem = reservationItem.Where(x => x.Name.Contains(searchValue)).ToList();
            }

           
            //total number of rows count     
            recordsTotal = reservationItem.Count();

            //Paging     
            var data = reservationItem.Skip(skip).Take(pageSize).ToList();

            //Returning Json Data    
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
          
        }


    }
}