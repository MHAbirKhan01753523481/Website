using System.Collections.Generic;
using Alphasoft.Models;

namespace Alphasoft.IRepositories
{
    public interface IOurTeamRepository : IRepository<OurTeam>
    {
        List<OurTeam> GetAllWithDepartmentAndDesignation();
        OurTeam GetWithDepartmentDesignation(int id);
        List<OurTeam> TeamWithUs();
    }
}
