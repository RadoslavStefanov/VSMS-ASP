using Microsoft.EntityFrameworkCore;
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
        public void RegisterSale(string JSONinput,string userId)
        {
            
            var result = JsonSerializer.Deserialize<List<PrimitiveSale>>(JSONinput);

            var tempSale = new Sales
            {
                DateTime = DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss"),
                UserId = $"{userId}",
                Total = 0
            };

            tempSale.SalesProducts = new List<SalesProducts>();

            foreach (var entry in result)
            {
                tempSale.SalesProducts.Add(new SalesProducts
                {
                    SaleId = tempSale.Id,
                    ProductId = repo.All<Products>().Where(p => p.Name == entry.soldProductName).FirstOrDefault().Id,
                    Quantity = entry.soldProductAmout,
                    AtPrice = entry.AtPrice
                });

                tempSale.Total+=entry.soldProductTotalPrice;
            }

            foreach (var item in tempSale.SalesProducts)
            {
                var product = repo.All<Products>().Where(p => p.Id == item.ProductId).FirstOrDefault();
                product.Quantity-=item.Quantity;
                repo.SaveChanges();
            }

            repo.Add(tempSale);
            repo.SaveChanges();
        }

        public List<MySalesViewModel> GetUserSales(string userId,string userName)
        {
            var userSales = repo.All<SalesProducts>().Where(s=>s.Sale.UserId == userId)
                .ToList();

            var result = new List<MySalesViewModel>();
            
            foreach (var item in userSales)
            {
                var product = repo.All<Products>().Where(p => p.Id == item.ProductId).FirstOrDefault();
                var sale = repo.All<Sales>().Where(s=>s.Id==item.SaleId).FirstOrDefault();
                result.Add(new MySalesViewModel
                {
                    DateTime = sale.DateTime,
                    ProductName = product.Name,
                    Quantity = (int)item.Quantity,
                    AtPrice= item.AtPrice,
                    TotalPrice = decimal.Round((item.Quantity * item.AtPrice), 2, MidpointRounding.AwayFromZero),
                    Seller = userName
                });
            }
            return result;
        }

        public List<MySalesViewModel> GetSales()
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
            return  result;
        }
    }
}
