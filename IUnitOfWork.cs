using Alphasoft.IRepositories;
using Alphasoft.IServices;
using Alphasoft.Models;
using Alphasoft.Repositories;
using System;

namespace Alphasoft.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {

        IProductDetails ProductDetails { get; }
        IProductRepository Products { get; }
        IProductCategoryRepository ProductCategories { get; }
        IServiceRepository Services { get; }
        IServiceCategoryRepository ServiceCategories { get; }
        IDepartmentRepository Departments { get; }
        IMenuRepository Menus { get; }
        ISubMenuRepository SubMenus { get; }
        IOurTeamRepository OurTeams { get;}
        IDesignationRepository Designations { get; }
        IFaqRepository Faq { get; }
        IClientRepository Client { get; }
        IBlogsRepository Blogs { get; }
        IContactUsRepository ContactUs { get; }
        ICompanyRepository Companies { get; }
        IBannerRepository Banner { get; }
        IWhyChooseUsRepository ChooseUs { get; }
        IClientProductRepository ClientProducts { get; }
        IAboutUsRepository AboutUs { get; }
        IHostingPlanRepository HostingPlan { get; }
        ICustomerReviewRepository CustomerReview { get; }
        ICareerRepository Career { get; }
        IJobRepository Job { get; }
        ISoftwareRepository Softwares { get; }
        ISoftwareCategoriesRepository SoftwareCategories { get; }
        IFeaturesRepository Features { get; }
        IBlogCommentRepository BlogComment { get; }
        IQueryHelper QueryHelper { get; }

        int Complete();
       
    }
}
