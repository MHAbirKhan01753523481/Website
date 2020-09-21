using Alphasoft.Constants;
using Alphasoft.Helpers;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Alphasoft.Data
{
    public class SeedData
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly RoleHelper _roleHelper;
        private readonly ClaimHelper _claimHelper;

        public SeedData(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _roleHelper = new RoleHelper();
            _claimHelper = new ClaimHelper();
        }

        public async Task CreateDefaultUsers()
        {
            ApplicationUser SuperAdmin = await _userManager.FindByNameAsync("sa");

            if (SuperAdmin == null)
            {
                SuperAdmin = new ApplicationUser()
                {
                    UserName = "sa",
                    Email = "sa@alphasoft.com.bd"
                };
                _userManager.CreateAsync(SuperAdmin, "1234").Wait();
            }
        }

        public async Task CreateRoles()
        {
            var roles = _roleHelper.GetRoles();

            foreach (var item in roles)
            {
                bool isCreated = await _roleManager.RoleExistsAsync(item.Name);

                if (!isCreated)
                {
                    await _roleManager.CreateAsync(item);
                }
            }
        }

        public async Task AddPermissionsToRole(List<string> controllers, string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            var claims = _claimHelper.GetClaims();

            foreach (var item in controllers)
            {
                claims = claims.Where(x => x.Contains(item)).ToList();
            }

            foreach (var claim in claims)
            {
                await _roleManager.AddClaimAsync(role, new Claim(claim, role.Id));
            }
        }

        public async Task Seed()
        {
            _context.Database.EnsureCreated();

            await CreateDefaultUsers();
            await CreateRoles();
            await AddPermissionsToRole(_roleHelper.GetControllersForSA(), RoleConstants.SuperAdmin);
            await AddPermissionsToRole(_roleHelper.GetControllersForAdmin(), RoleConstants.Admin);
        }
    }
}
