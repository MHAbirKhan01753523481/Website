using Alphasoft.Constants;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Alphasoft.Helpers
{
    public class RoleHelper
    {
        public List<IdentityRole> GetRoles()
        {
            List<IdentityRole> roles = new List<IdentityRole>()
            {
                new IdentityRole { Name = RoleConstants.SuperAdmin },
                new IdentityRole { Name = RoleConstants.Admin },
                new IdentityRole { Name = RoleConstants.HR },
                new IdentityRole { Name = RoleConstants.General }
            };
            return roles;
        }

        public List<string> GetControllersForSA()
        {
            List<string> controllers = new List<string>()
            {
                "Home"
            };

            return controllers;
        }

        public List<string> GetControllersForAdmin()
        {
            List<string> controllers = new List<string>()
            {
                "Home"
            };

            return controllers;
        }
    }
}