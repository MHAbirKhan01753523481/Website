using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alphasoft.Data;
using Alphasoft.Models.RMSTable;
using Alphasoft.Models.RMSTable.vmRms;
using Microsoft.AspNetCore.Mvc;

namespace Alphasoft.Controllers
{

    public class TablesController : Controller
    {
        private ApplicationDbContext _context;
        public TablesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        //public IActionResult Create(vmTable vmTables)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Table table = new Table()
        //        {
        //            Name=vmTables.Name
        //        };
        //        _context.Table.Add(table);
        //        _context.SaveChanges();
        //        return Json(true);
        //    }
        //    return Json(false);
        //}
    }
}