using VSMS.Infrastructure.Data.Common;
using VSMS.Infrastructure.Data.Models;

namespace VSMS.Core.Services
{
    public class HelpService
    {
        private readonly Repository repo;
        public HelpService(Repository _repo)
        { repo = _repo; }


        public async Task<bool> CreateResetRequest(string userName)
        {
            if (repo.All<ResetRequests>().Any(r => r.Username == userName))
            { return false; }

            var request = new ResetRequests()
            {
                Date = DateTime.Now.ToString(),
                Username = userName
            };

            await repo.AddAsync(request);
            try
            {
                await repo.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            { return false; }
        }
    }
}
