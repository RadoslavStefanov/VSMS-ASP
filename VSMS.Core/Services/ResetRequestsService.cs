using VSMS.Core.ViewModels;
using VSMS.Infrastructure.Data.Common;
using VSMS.Infrastructure.Data.Models;

namespace VSMS.Core.Services
{
    public class ResetRequestsService
    {
        private readonly Repository repo;
        public ResetRequestsService(Repository _repo)
        { repo = _repo; }


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
