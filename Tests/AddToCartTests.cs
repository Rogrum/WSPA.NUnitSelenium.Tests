using NUnit.Framework;
using WPSF.NUnitSelenium.Tests.Pages;
using WPSF.NUnitSelenium.Tests.Utils;

namespace WPSF.NUnitSelenium.Tests.Tests
{
    [TestFixture]
    public class AddToCartTests : TestBase
    {
        private string BaseUrl => ConfigHelper.GetString("BaseUrl", "http://localhost:5000");

        [Test, Order(1), Retry(1), CancelAfter(45000), Category("Cart")]
        public void AddToCart_FromCategory_Aparaty_NikonD7500()
        {
            const string productName = "Nikon D7500";

            // Wejście na stronę i przejście do kategorii
            Driver.Navigate().GoToUrl(BaseUrl);
            var category = new Category(Driver);
            category.GoToAparaty();
            category.AddToCartFromCategoryByName(productName);

            // Weryfikacja w koszyku
            var cart = new CartPage(Driver);
            cart.Open(BaseUrl);
            Assert.That(cart.HasItems(), Is.True, "Koszyk powinien zawierać pozycję po dodaniu z kategorii.");
        }
    }
}
