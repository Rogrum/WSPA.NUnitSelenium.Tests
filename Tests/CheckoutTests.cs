using NUnit.Framework;
using WPSF.NUnitSelenium.Tests.Pages;
using WPSF.NUnitSelenium.Tests.Utils;
using Data = WPSF.NUnitSelenium.Tests.Data;

namespace WPSF.NUnitSelenium.Tests.Tests
{
    [TestFixture]
    public class CheckoutTests : TestBase
    {
        private string BaseUrl => ConfigHelper.GetString("BaseUrl", "http://localhost:5000");

        [Test, Order(1), Retry(1), CancelAfter(45000), Category("Checkout")]
        [TestCaseSource(typeof(Data.CommonData), nameof(Data.CommonData.Addresses))]
        public void Checkout_Basic_AddFromCategory(string fullName, string address, string city, string postal, string blik)
        {
            const string productName = "Nikon D7500";

            // 1) Logujemy się jako użytkownik
            var login = new LoginPage(Driver);
            login.Open(BaseUrl);
            login.Login("test@gmail.com", "test12"); // albo "admin@example.com", "admin123" – jeśli checkout wymaga admina

            // 2) Dodajemy produkt z kategorii „Aparaty”
            Driver.Navigate().GoToUrl(BaseUrl);
            var category = new Category(Driver);
            category.GoToAparaty();
            category.AddToCartFromCategoryByName(productName);

            // 3) Przechodzimy do koszyka
            var cart = new CartPage(Driver);
            cart.Open(BaseUrl);
            Assert.That(cart.HasItems(), Is.True, "Koszyk powinien zawierać pozycje po dodaniu.");

            // 4) Checkout
            cart.GoToCheckout();

            var checkout = new CheckoutPage(Driver);
            checkout.Fill(fullName, address, city, postal, blik);
            checkout.Submit();

            Assert.That(checkout.IsConfirmed(), Is.True, "Powinna pojawić się informacja o potwierdzeniu zamówienia.");
        }
    }
}
