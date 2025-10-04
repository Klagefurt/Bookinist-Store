using Bookinist.DAL.Context;
using Bookinist.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookinist_Store.Data
{
    class DbInitializer
    {
        private readonly BookinistDB _db;
        private readonly ILogger<DbInitializer> _logger;

        public DbInitializer(BookinistDB dB , ILogger<DbInitializer> logger)
        {
            _db = dB;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            var timer = Stopwatch.StartNew();
            _logger.LogInformation("Db initializing...");

            _logger.LogInformation("Existing Db deleting...");
            await _db.Database.EnsureDeletedAsync()/*.ConfigureAwait(false)*/;
            _logger.LogInformation("Existing Db deleted in {0} ms", timer.ElapsedMilliseconds);

            // _db.Database.EnsureCreated();

            _logger.LogInformation("Db migrating...");
            await _db.Database.MigrateAsync();
            _logger.LogInformation("Db migrated in {0} ms", timer.ElapsedMilliseconds);

            if (await _db.Books.AnyAsync()) return;

            await InitializeCategories();
            await InitializeBooks();
            await InitializeSellers();
            await InitializeBuyers();
            await InitializeDeals();

            _logger.LogInformation("Db initializing is completed in {0} s", timer.Elapsed.TotalSeconds);
        }

        private const int _BooksCount = 10;
        private Book[] _Books;
        private async Task InitializeBooks()
        {
            var timer = Stopwatch.StartNew();
            _logger.LogInformation("Books initialization...");

            var rnd = new Random();
            _Books = Enumerable.Range(1, _BooksCount)
                .Select(i => new Book
                {
                    Name = $"Book {i}",
                    Category = rnd.NextItem(_Categories),
                })
                .ToArray();

            await _db.Books.AddRangeAsync(_Books);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Books initialization is completed in {0} ms", timer.ElapsedMilliseconds);
        }

        private const int _CategoriesCount = 10;
        private Category[] _Categories;
        private async Task InitializeCategories()
        {
            var timer = Stopwatch.StartNew();
            _logger.LogInformation("Categories initialization...");

            _Categories = new Category[_CategoriesCount];
            for (var i = 0; i < _CategoriesCount; i++)
                _Categories[i] = new Category { Name = $"Category {i + 1}" };

            await _db.Categories.AddRangeAsync(_Categories);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Categories initialization is completed in {0} ms", timer.ElapsedMilliseconds);
        }

        private const int _SellersCount = 10;
        private Seller[] _Sellers;
        private async Task InitializeSellers()
        {
            var timer = Stopwatch.StartNew();
            _logger.LogInformation("Sellers initialization...");

            _Sellers = Enumerable.Range(1, _SellersCount)
                .Select(i => new Seller
                {
                    Name = $"Seller-Name {i}",
                    Surname = $"Seller-Surname {i}",
                    Patronymic = $"Seller-Patronymic {i}"
                })
                .ToArray();

            await _db.Sellers.AddRangeAsync(_Sellers);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Sellers initialization is completed in {0} ms", timer.ElapsedMilliseconds);
        }

        private const int _BuyersCount = 10;
        private Buyer[] _Buyers;
        private async Task InitializeBuyers()
        {
            var timer = Stopwatch.StartNew();
            _logger.LogInformation("Buyers initialization...");

            _Buyers = Enumerable.Range(1, _BuyersCount)
                .Select(i => new Buyer
                {
                    Name = $"Buyer-Name {i}",
                    Surname = $"Buyer-Surname {i}",
                    Patronymic = $"Buyer-Patronymic {i}"
                })
                .ToArray();

            await _db.Buyers.AddRangeAsync(_Buyers);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Buyers initialization is completed in {0} ms", timer.ElapsedMilliseconds);
        }

        private const int _DealsCount = 500;
        private async Task InitializeDeals()
        {
            var timer = Stopwatch.StartNew();
            _logger.LogInformation("Deals initialization...");

            var rnd = new Random();

            var deals = Enumerable.Range(1, _DealsCount)
                .Select(i => new Deal
                {
                    Book = rnd.NextItem(_Books),
                    Buyer = rnd.NextItem(_Buyers),
                    Seller = rnd.NextItem(_Sellers),
                    Price = (decimal)rnd.NextDouble(rnd.NextDouble() * 4000 + 700)
                });

            await _db.Deals.AddRangeAsync(deals);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Deals initialization is completed in {0} ms", timer.ElapsedMilliseconds);
        }
    }
}
