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
        }
        public void RegisterSale(string JSONinput,string userId)
        {
            
            var result = JsonSerializer.Deserialize<List<PrimitiveSale>>(JSONinput);

            var tempSale = new Sales
            {
                DateTime = DateTime.Now,
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
                    Quantity = entry.soldProductAmout
                });

                tempSale.Total+=entry.soldProductTotalPrice;
            }

            repo.Add(tempSale);
            repo.SaveChanges();
        }

        public List<MySalesViewModel> GetUserSales(string userId)
        {
            var userSales = repo.All<Sales>().Where(s => s.UserId == userId)
                .Include("SalesProducts").ToList();

            var result = new List<MySalesViewModel>();
            
            foreach (var item in userSales)
            {
                var productId = item.SalesProducts.Select(x => x.ProductId).FirstOrDefault();
                var product =repo.All<Products>().Where(p=>p.Id== productId).FirstOrDefault();
                result.Add(new MySalesViewModel
                {
                    DateTime=item.DateTime,
                    ProductName = product.Name,
                    Quantity = (int)item.SalesProducts.Select(sp=>sp.Quantity).FirstOrDefault(),
                    TotalPrice = item.Total
                });
            }

            return result;
        }

        public void GetSales()
        {

        }
    }
}
