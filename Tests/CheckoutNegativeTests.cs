using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using WPSF.NUnitSelenium.Tests.Utils;
using WPSF.NUnitSelenium.Tests.Pages;

namespace WPSF.NUnitSelenium.Tests.Tests.Negative
{
    [TestFixture]
    public class CheckoutNegativeTests : TestBase
    {
        private string BaseUrl => ConfigHelper.GetString("BaseUrl", "http://localhost:5000");

        [TestCase(TestName = "TC-09"), Order(1), Retry(1), CancelAfter(60000), Category("Checkout")]
        public void Checkout_MissingBLik_ShowsValidation_NoFinalize()
        {
            // Precondition: zalogowany i w koszyku mamy co najmniej 1 produkt
            Driver.Navigate().GoToUrl($"{BaseUrl}/login");
            Driver.FindElement(By.CssSelector("input[name='email'], #email")).SendKeys("test@gmail.com");
            Driver.FindElement(By.CssSelector("input[name='password'], #password")).SendKeys("test12");
            Driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // dodaj z kategorii „Aparaty” (działa stabilnie)
            Driver.Navigate().GoToUrl(BaseUrl);
            var category = new Category(Driver);
            category.GoToAparaty();
            category.AddToCartFromCategoryByName("Nikon D7500");

            // przejście do koszyka i do checkout
            Driver.Navigate().GoToUrl($"{BaseUrl}/cart");
            var goCheckoutBtn = Driver.FindElement(By.CssSelector("a[href*='/checkout'], button[name='checkout'], [data-test='go-checkout']"));
            goCheckoutBtn.Click();

            // na checkout: NIE wypełniaj pola BLIK – od razu „Zamów”
            var submitOrder = Driver.FindElement(By.CssSelector("button.btn.btn-primary.w-100.mt-3[type='submit']"));
            submitOrder.Click();

            // 5) Oczekujemy walidacji pola BLIK i BRAKU finalizacji
            var blik = Driver.FindElement(By.CssSelector("input#blik_code[name='blik_code']"));

            // krótki wait aż przeglądarka nałoży invalid state na BLIK po submit
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));
            wait.Until(_ =>
            {
                // sprawdzamy HTML5 validity
                var isInvalid = (bool)((IJavaScriptExecutor)Driver)
                    .ExecuteScript("return arguments[0].validity.valueMissing || arguments[0].validity.patternMismatch;", blik);
                return isInvalid;
            });

            // odczytaj natywny komunikat walidacji (może się różnić między przeglądarkami/językami)
            var validationMessage = (string)((IJavaScriptExecutor)Driver)
                .ExecuteScript("return arguments[0].validationMessage;", blik);

            // brak potwierdzenia zamówienia
            bool isConfirmed = Driver.FindElements(By.CssSelector(".order-confirmation, [data-test='order-success'], .alert-success")).Count > 0;

            Assert.Multiple(() =>
            {
                // akceptujemy dowolny niepusty komunikat
                Assert.That(string.IsNullOrWhiteSpace(validationMessage), Is.False, "Powinien pojawić się komunikat walidacji dla pola BLIK.");
                Assert.That(isConfirmed, Is.False, "Finalizacja nie powinna się powieść przy pustym polu BLIK.");
            });

        }
    }
}
