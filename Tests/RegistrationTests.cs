using NUnit.Framework;
using WPSF.NUnitSelenium.Tests.Pages;
using WPSF.NUnitSelenium.Tests.Utils;
using Data = WPSF.NUnitSelenium.Tests.Data;
using OpenQA.Selenium;

namespace WPSF.NUnitSelenium.Tests.Tests
{
    [TestFixture]
    public class RegistrationTests : TestBase
    {
        private string BaseUrl => ConfigHelper.GetString("BaseUrl", "http://localhost:5000");

        [Test, Order(1), Retry(1), CancelAfter(25000)]
        [TestCaseSource(typeof(Data.CommonData), nameof(Data.CommonData.RegistrationValid))]
        public void Registration_Success(string fullName, string email, string password, string confirm)
        {
            var page = new RegisterPage(Driver);
            page.Open(BaseUrl);
            page.Register(fullName, email, password, confirm);
            Assert.That(page.SuccessText(), Is.Not.Empty, "Powinien pojawić się komunikat sukcesu po rejestracji");
        }

        [Test, Order(2), Retry(1), CancelAfter(20000)]
        [TestCaseSource(typeof(Data.CommonData), nameof(Data.CommonData.RegistrationInvalid))]
        public void Registration_Fail(string fullName, string email, string password, string confirm)
        {
            var page = new RegisterPage(Driver);
            page.Open(BaseUrl);
            page.Register(fullName, email, password, confirm);
            var anyAlert = !string.IsNullOrEmpty(page.ErrorText());
            var emailInput = Driver.FindElement(By.CssSelector("#email, input[name='email']"));
            var validation = emailInput.GetAttribute("validationMessage") ?? "";
            Assert.That(anyAlert || !string.IsNullOrEmpty(validation), Is.True,
                "Dla pustych pól oczekujemy alertu błędu lub komunikatu walidacji HTML5.");
        }
    }
}
