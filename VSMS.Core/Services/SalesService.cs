using System.Text.Json;
using VSMS.Core.ViewModels;
using VSMS.Infrastructure.Data.Common;
using VSMS.Infrastructure.Data.Models;

namespace VSMS.Core.Services
{
    public class SalesService
    {
        private readonly Repository repo;
        public SalesService(Repository _repo)
        { repo = _repo; }
        private class PrimitiveSale
        {
            public string? soldProductName { get; set; }
            public int soldProductAmout { get; set; }
            public decimal soldProductTotalPrice { get; set; }
            public decimal AtPrice { get; set; }
        }
        public async Task RegisterSale(string JSONinput, string userId)
        {
            var result = new List<PrimitiveSale>();
            try
            { result = JsonSerializer.Deserialize<List<PrimitiveSale>>(JSONinput); }
            catch (Exception)
            { throw new ArgumentException("JSON input is not valid!"); }
            

            var tempSale = new Sales
            {
                DateTime = DateTime.UtcNow.ToString("M/dd/yyyy h:mm:ss") + " UTC",
                UserId = $"{userId}",
                Total = 0
            };

            tempSale.SalesProducts = new List<SalesProducts>();

            foreach (var entry in result)
            {
                var product = repo.All<Products>().Where(p => p.Name == entry.soldProductName).FirstOrDefault();
                if (product == null)
                { throw new ArgumentException($"The product: {entry.soldProductName} does not exist in the Database!");}

                tempSale.SalesProducts.Add(new SalesProducts
                {
                    SaleId = tempSale.Id,
                    ProductId = product.Id,
                    Quantity = entry.soldProductAmout,
                    AtPrice = entry.AtPrice
                });

                tempSale.Total += entry.soldProductTotalPrice;
            }

            foreach (var item in tempSale.SalesProducts)
            {
                var product = repo.All<Products>().Where(p => p.Id == item.ProductId).FirstOrDefault();
                product.Quantity -= item.Quantity;
            }

            await repo.AddAsync(tempSale);
            await repo.SaveChangesAsync();
        }

        public async Task<List<MySalesViewModel>> GetUserSales(string userId, string userName)
        {
            var userSales = repo.All<SalesProducts>().Where(s => s.Sale.UserId == userId)
                .ToList();

            var result = new List<MySalesViewModel>();

            foreach (var item in userSales)
            {
                var product = repo.All<Products>().Where(p => p.Id == item.ProductId).FirstOrDefault();
                var sale = repo.All<Sales>().Where(s => s.Id == item.SaleId).FirstOrDefault();
                result.Add(new MySalesViewModel
                {
                    DateTime = sale.DateTime,
                    ProductName = product.Name,
                    Quantity = (int)item.Quantity,
                    AtPrice = item.AtPrice,
                    TotalPrice = decimal.Round((item.Quantity * item.AtPrice), 2, MidpointRounding.AwayFromZero),
                    Seller = userName
                });
            }
            return result;
        }

        public async Task<List<MySalesViewModel>> GetSales()
        {
            var sales = repo.All<SalesProducts>().ToList();

            var result = new List<MySalesViewModel>();

            foreach (var item in sales)
            {
                var product = repo.All<Products>().Where(p => p.Id == item.ProductId).FirstOrDefault();
                var sale = repo.All<Sales>().Where(s => s.Id == item.SaleId).FirstOrDefault();
                result.Add(new MySalesViewModel
                {
                    DateTime = sale.DateTime,
                    ProductName = product.Name,
                    Quantity = (int)item.Quantity,
                    AtPrice = item.AtPrice,
                    TotalPrice = decimal.Round((item.Quantity * product.Price), 2, MidpointRounding.AwayFromZero),
                    Seller = sale.UserId
                });
            }
            return result;
        }
    }
}
