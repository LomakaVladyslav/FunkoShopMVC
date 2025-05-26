using Xunit;
using FunkoShopDomain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using FunkoShopInfrastructure;
using System.ComponentModel.DataAnnotations;

namespace FunkoShop.Tests
{
    public class DatabaseFixture : IDisposable
    {
        public FunkoShopContext Context { get; private set; }

        public DatabaseFixture()
        {
            var options = new DbContextOptionsBuilder<FunkoShopContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            Context = new FunkoShopContext(options);
            Context.Database.EnsureCreated();
            SeedData();
        }

        private void SeedData()
        {
            var countries = new[]
            {
                new Country { Name = "United States", Code = "USA" },
                new Country { Name = "Ukraine", Code = "UKR" }
            };
            Context.Countries.AddRange(countries);

            var categories = new[]
            {
                new Category { Name = "Movies", Description = "Movie characters" },
                new Category { Name = "Games", Description = "Video game characters" }
            };
            Context.Categories.AddRange(categories);

            var user = new User
            {
                Email = "test@user.com",
                Password = "password123",
                Username = "testuser"
            };
            Context.Users.Add(user);
            Context.SaveChanges();

            var figure = new Figure
            {
                Name = "Batman",
                Price = 29.99m,
                CategoryId = categories.First().Id,
                CountryId = countries.First().Id,
                ImageUrl = "batman.jpg"
            };
            Context.Figures.Add(figure);

            var order = new Order
            {
                UserId = user.Id,
                Status = "Pending",
                TotalPrice = 59.98m
            };
            Context.Orders.Add(order);
            Context.SaveChanges();

            var orderItem = new OrderItem
            {
                OrderId = order.Id,
                FigureId = figure.Id,
                Quantity = 2,
                Price = 29.99m
            };
            Context.OrderItems.Add(orderItem);
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }

    public class UserTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public UserTests(DatabaseFixture fixture) => _fixture = fixture;

        [Fact]
        [Trait("Category", "Basic")]
        public void CanAddUser()
        {
            var newUser = new User
            {
                Email = "unique@user.com",
                Password = "newpass",
                Username = "uniqueuser"
            };

            _fixture.Context.Users.Add(newUser);
            var result = _fixture.Context.SaveChanges();

            Assert.Equal(1, result);
            Assert.True(_fixture.Context.Users.Any(u => u.Email == "unique@user.com"));
        }

        [Theory]
        [InlineData("valid@email.com", "strongpass", "user1")]
        [InlineData("another@test.ua", "password", "user2")]
        [Trait("Category", "Parameterized")]
        public void ValidateUserEmailFormats(string email, string password, string username)
        {
            var user = new User { Email = email, Password = password, Username = username };

            _fixture.Context.Users.Add(user);
            var exception = Record.Exception(() => _fixture.Context.SaveChanges());
            Assert.Null(exception);

            Assert.Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", user.Email);
        }

       
    }

    public class CountryTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public CountryTests(DatabaseFixture fixture) => _fixture = fixture;

        [Fact]
        [Trait("Category", "Basic")]
        public void CountryCodeLengthMustBeThree()
        {
            var country = new Country { Name = "Test Country", Code = "TTT" };

            _fixture.Context.Countries.Add(country);
            _fixture.Context.SaveChanges();

            Assert.Equal(3, country.Code.Length);
        }
    }

    public class FigureTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public FigureTests(DatabaseFixture fixture) => _fixture = fixture;

        [Fact]
        [Trait("Category", "Collection")]
        public void AllFiguresHaveRequiredProperties()
        {
            var figures = _fixture.Context.Figures.ToList();

            Assert.All(figures, f => {
                Assert.False(string.IsNullOrEmpty(f.Name));
                Assert.True(f.Price > 0);
                Assert.NotNull(f.CategoryId);
                Assert.NotNull(f.CountryId);
            });
        }

        [Fact]
        [Trait("Category", "Comprehensive")]
        public void FigureDetailsAreValid()
        {
            var figure = _fixture.Context.Figures.FirstOrDefault(f => f.Name == "Batman");

            Assert.NotNull(figure);
            Assert.Equal("Batman", figure.Name);
            Assert.True(figure.Price > 0);
            Assert.NotNull(figure.ImageUrl);
            Assert.EndsWith(".jpg", figure.ImageUrl);
        }
    }

    public class CategoryTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public CategoryTests(DatabaseFixture fixture) => _fixture = fixture;

        [Theory]
        [InlineData("Movies")]
        [InlineData("Games")]
        [Trait("Category", "Parameterized")]
        public void CategoryNamesArePresent(string expectedName)
        {
            var categoryNames = _fixture.Context.Categories.Select(c => c.Name).ToList();

            Assert.Contains(expectedName, categoryNames);
            Assert.All(categoryNames, name => Assert.False(string.IsNullOrWhiteSpace(name)));
        }
    }

    public class OrderTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public OrderTests(DatabaseFixture fixture) => _fixture = fixture;
    }

    public class StringFormatTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public StringFormatTests(DatabaseFixture fixture) => _fixture = fixture;

        [Fact]
        [Trait("Category", "String")]
        public void CountryCodesAreUpperCase()
        {
            var countries = _fixture.Context.Countries.ToList();

            Assert.All(countries, c =>
                Assert.Matches(@"^[A-Z]{3}$", c.Code));
        }
    }
}
