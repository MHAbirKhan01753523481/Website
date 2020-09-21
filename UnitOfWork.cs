using Alphasoft.Data;
using Alphasoft.IRepositories;
using Alphasoft.IServices;
using Alphasoft.Repositories;
using Alphasoft.Services;

namespace Alphasoft.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            ProductDetails = new ReservationDetailsRepository(_context);
            Products = new ProductRepository(_context);

            ProductCategories = new ProductCategoryRepository(_context);

            Services = new ServiceRepository(_context);

            ServiceCategories = new ServiceCategoryRepository(_context);

            Departments = new DepartmentRepository(_context);

            Menus = new MenuRepository(_context);

            SubMenus = new SubMenuRepository(_context);

            OurTeams = new OurTeamRepository(_context);

            Designations = new DesignationRepository(_context);

            Faq = new FaqRepository(_context);

            Client= new ClientRepository(_context);

            Blogs = new BlogRepository (_context);
           
            ContactUs = new ContactUsRepository(_context);

            Companies = new CompanyRepository(_context);

            Banner = new BannerRepository(_context);

            ChooseUs = new WhyChooseUsRepository(_context);

            ClientProducts = new ClientProductRepository(_context);

            AboutUs = new AboutUsRepository(_context);

            HostingPlan = new HostingPlanRepository(_context);

            CustomerReview = new CustomerReviewRepository(_context);

            Career = new CareerRepository(_context);

            Job = new JobRepository(_context);

            Softwares = new SoftwareRepository(_context);

            SoftwareCategories = new SoftwareCategoriesRepository(_context);

            Features = new FeaturesRepository(_context);

            BlogComment = new BlogCommentRepository(_context);

            QueryHelper = new QueryHelper();

        }
        public IProductDetails ProductDetails { get; private set; }
        public IFeaturesRepository Features { get; private set; }
        public IProductRepository Products { get; private set; }
        public IProductCategoryRepository ProductCategories { get; private set; }
        public IServiceRepository Services { get; private set; }
        public IServiceCategoryRepository ServiceCategories { get; private set; }
        public IDepartmentRepository Departments { get; private set; }
        public IMenuRepository Menus { get; private set; }
        public ISubMenuRepository SubMenus { get; private set; }
        public IOurTeamRepository OurTeams { get; private set; }
        public IFaqRepository Faq { get; private set; }
        public IClientRepository Client { get; private set; }
        public IBlogsRepository Blogs { get; private set; }
        public IContactUsRepository ContactUs { get; private set; }
        public IDesignationRepository Designations { get; private set; }
        public ICompanyRepository Companies { get; private set; }
        public IBannerRepository Banner { get; private set; }
        public IWhyChooseUsRepository ChooseUs { get; private set; }
        public IClientProductRepository ClientProducts { get; private set; }
        public IAboutUsRepository AboutUs { get; private set; }
        public IHostingPlanRepository HostingPlan { get; private set; }
        public ICustomerReviewRepository CustomerReview { get; private set; }
        public ICareerRepository Career { get; private set; }
        public IJobRepository Job { get; private set; }
        public ISoftwareRepository Softwares { get; private set; }
        public ISoftwareCategoriesRepository SoftwareCategories { get; private set; }
        public IBlogCommentRepository BlogComment { get; private set; }
        public IQueryHelper QueryHelper { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
