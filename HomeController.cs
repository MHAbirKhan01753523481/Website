using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Alphasoft.Models;
using Microsoft.AspNetCore.Identity;
using Alphasoft.Data;
using Alphasoft.ViewModels;
using Alphasoft.UnitOfWork;
using System.Text.RegularExpressions;

namespace Alphasoft.Controllers
{
    public class HomeController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _work;

        public HomeController(UserManager<ApplicationUser> userManager, IUnitOfWork work)
        {
            _userManager = userManager;
            _work = work;
        }

        public IActionResult Index()
        {

            HomeViewModel homeViewModel = new HomeViewModel();

            homeViewModel.BannerList = _work.Banner.GetAll();

            var chooseUsList = _work.ChooseUs.GetAll();

            //foreach (var item in chooseUsList)
            //{
            //    ChooseUs chooseUs = new ChooseUs
            //    {
            //        Id = item.Id,
            //        Title = item.Title,
            //        Description = item.Description,
            //        Order = item.Order,
            //        Image = item.Image,
            //       ShortDescription = Regex.Replace(item.ShortDescription, "<[^>]*>", "")
            //       // ShortDescription=item.ShortDescription,
            //    };
            //    homeViewModel.ChooseUsList.Add(chooseUs);
           // }

            homeViewModel.ClientList = _work.Client.GetAll();

            homeViewModel.ProductList = _work.Products.GetAllWithCategory();

            homeViewModel.HostingPlans = _work.HostingPlan.GetAll();

            return View(homeViewModel);
        }


        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Logoff()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            await _userManager.UpdateSecurityStampAsync(user);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult ContactUsFormShow()
        {
            return PartialView("_ContactUsForm");
        }

        public IActionResult AjaxErrorView()
        {
            return PartialView("_AjaxErrorView");
        }
    }
}