using NUnit.Framework;
using WPSF.NUnitSelenium.Tests.Pages;
using WPSF.NUnitSelenium.Tests.Utils;

namespace WPSF.NUnitSelenium.Tests.Tests
{
    [TestFixture]
    public class ReviewTest : TestBase
    {
        private string BaseUrl => ConfigHelper.GetString("BaseUrl", "http://localhost:5000");

        [TestCase(TestName = "TC-07"), Order(1), Retry(1), CancelAfter(45000), Category("Review")]
        public void Review_AddsSuccessfully()
        {
            const string productName = "Nikon D7500";

            // 1) Logowanie
            var login = new LoginPage(Driver);
            login.Open(BaseUrl);
            login.Login("test@gmail.com", "test12"); // podmień na właściwe dane

            // 2) Aparaty -> detal produktu
            Driver.Navigate().GoToUrl(BaseUrl);
            var category = new Category(Driver);
            category.GoToAparaty();
            category.OpenProductDetails(productName);

            // 3) Opinia – tylko rating + treść
            var review = new ReviewSection(Driver);
            review.AddReview(
                rating: 5,
                content: "Bardzo dobra jakość obrazu i ergonomia. Polecam!"
            );

            Assert.Pass("Opinia została dodana poprawnie.");
        }
    }
}
