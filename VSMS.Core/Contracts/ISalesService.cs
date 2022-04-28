using VSMS.Core.ViewModels;

namespace VSMS.Core.Contracts
{
    public interface ISalesService
    {
        public Task RegisterSale(string JSONinput, string userId);

        public Task<List<MySalesViewModel>> GetUserSales(string userId, string userName);

        public Task<List<MySalesViewModel>> GetSales();
    }
}
