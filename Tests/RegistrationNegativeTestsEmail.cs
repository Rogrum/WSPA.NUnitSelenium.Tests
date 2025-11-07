using NUnit.Framework;
using WPSF.NUnitSelenium.Tests.Pages;
using WPSF.NUnitSelenium.Tests.Utils;
using Data = WPSF.NUnitSelenium.Tests.Data;
using OpenQA.Selenium;

namespace WPSF.NUnitSelenium.Tests.Tests.Negative
{
    [TestFixture]
    public class RegistrationNegativeTestsEmail : TestBase
    {
        private string BaseUrl => ConfigHelper.GetString("BaseUrl", "http://localhost:5000");

        [Test, Order(1), Retry(1), CancelAfter(30000), Category("Register")]
        [TestCaseSource(typeof(Data.CommonData), nameof(Data.CommonData.RegistrationInvalidEmails))]
        public void Register_InvalidEmail_ShowsValidation(string fullName, string email, string password, string confirm)
        {
            var page = new RegisterPage(Driver);
            page.Open(BaseUrl);
            page.Register(fullName, email, password, confirm);

            // HTML5 walidacja e-maila (validationMessage) lub alert błędu (server-side)
            var emailInput = Driver.FindElement(By.CssSelector("#email, input[name='email']"));
            var validation = emailInput.GetAttribute("validationMessage") ?? string.Empty;

            Assert.That(!string.IsNullOrEmpty(validation) || !string.IsNullOrEmpty(page.ErrorText()),
                Is.True, "Powinien pojawić się komunikat walidacji dla błędnego adresu e-mail.");
        }
    }
}
