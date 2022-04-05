using VSMS.Infrastructure.Data.Common;
using VSMS.Infrastructure.Data.Models;

namespace VSMS.Core.Services
{
    public class HelpService
    {
        private readonly Repository repo;
        public HelpService(Repository _repo)
        { repo = _repo; }


        public bool CreateResetRequest(string userName)
        {
            if (repo.All<ResetRequests>().Any(r => r.Username == userName))
            { return false; }

            var request = new ResetRequests()
            {
                Date = DateTime.Today.ToString(),
                Username = userName
            };

            repo.Add(request);
            try
            {
                repo.SaveChanges();
                return true;
            }
            catch (Exception)
            {return false;}    
        }
    }
}
