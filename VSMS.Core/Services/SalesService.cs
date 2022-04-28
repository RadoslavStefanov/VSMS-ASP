using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using VSMS.Core.ViewModels;
using VSMS.Infrastructure.Data;
using VSMS.Infrastructure.Data.Common;
using VSMS.Infrastructure.Data.Models;

namespace VSMS.Core.Services
{
    public class SalesService
    {

        private readonly DbContext context;

        private readonly Repository repo;
        public SalesService(Repository _repo, VSMSDbContext _context)
        { repo = _repo; context = _context; }

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
                { throw new ArgumentException($"The product: {entry.soldProductName} does not exist in the Database!"); }
                if (entry.soldProductAmout <= 0)
                { throw new ArgumentException($"Cannot make sale with a quantity of 0 or less!"); }
                if (entry.AtPrice < 0)
                { throw new ArgumentException($"Cannot make sale with a price less than 0!"); }

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
            var user = repo.All<IdentityUser>().Where(u => u.Id == userId).FirstOrDefault();
            if (user == null) { throw new ArgumentException("User not found in Database!"); }

            var utcToday = changeSeparator(DateTime.UtcNow.ToString("M/dd/yyyy"));
            var query = @$"Select * FROM SalesProducts LEFT JOIN Sales AS s ON SalesProducts.SaleId = s.Id Where SUBSTRING(s.DateTime,1,CHARINDEX(' ',s.DateTime)) = '{utcToday}' AND s.UserId = '{user.Id}'";
            var userSales = context.Set<SalesProducts>().FromSqlRaw(query).ToList();


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
                    Seller = userName,
                    kgPerPiece = product.Kilograms
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
                    Seller = sale.UserId,
                    kgPerPiece = product.Kilograms
                });
            }
            return result;
        }

        public string GetTodayIncomeForUser(string userId)
        {
            var user = repo.All<IdentityUser>().Where(u => u.Id == userId).FirstOrDefault();
            if (user == null) { throw new ArgumentException("User not found in Database!"); }

            var utcToday = changeSeparator(DateTime.UtcNow.ToString("M/dd/yyyy"));

            var userSales = repo.All<Sales>()
                .Where(s => s.UserId == user.Id)
                .ToList()
                .Where(s => s.DateTime.Substring(0, s.DateTime.IndexOf(" ")) == utcToday)
                .ToList();

            var total = 0.0m;

            foreach (var item in userSales)
            {total += item.Total;}

            return String.Format("{0:0.00}", total); ;
        }

        private char getSeparator(string input)
        {
            var result = 'n';

            foreach (var entry in input.ToCharArray())
            {
                if(!Char.IsDigit(entry))
                { return entry; }
            }

            return result;
        }


        private string changeSeparator(string input)
        {
            var separator = getSeparator(input);
            if (separator != '/')
            {
                var result = input.Split(separator);
                return $"{result[0]}/{result[1]}/{result[2]}";
            }
            
            return input;
        }
    }
}
