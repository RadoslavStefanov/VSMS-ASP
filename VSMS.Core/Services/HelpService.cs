using VSMS.Core.ViewModels;
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
            if (userName == null || userName.Length < 3)
            { throw new ArgumentException("Invalid user!"); }

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
            {return false;}    
        }

        public List<ResetRequestsViewModel> GetAllRequests()
        {
            var requests = repo.All<ResetRequests>().ToList();
            var result = new List<ResetRequestsViewModel>();

            foreach (var request in requests)
            {
                result.Add(new ResetRequestsViewModel
                {
                    id = request.Id,
                    Date = request.Date,
                    Username = request.Username,
                });
            }
            return result;
        }
    }
}
