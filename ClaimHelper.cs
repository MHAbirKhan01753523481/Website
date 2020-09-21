using Alphasoft.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alphasoft.Helpers
{
    public class ClaimHelper
    {
        public List<string> GetClaims()
        {
            List<string> roles = new List<string>()
            {
                #region Home Claims
                HomeClaims.Create,
                HomeClaims.Read,
                HomeClaims.Update,
                HomeClaims.Delete
                #endregion
            };
            return roles;
        }
    }
}
