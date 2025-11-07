using NUnit.Framework;
using WPSF.NUnitSelenium.Tests.Pages;
using WPSF.NUnitSelenium.Tests.Utils;
using OpenQA.Selenium;
using Data = WPSF.NUnitSelenium.Tests.Data;

namespace WPSF.NUnitSelenium.Tests.Tests
{
    [TestFixture, Order(1)]
    public class LoginTests : TestBase
    {
        private string BaseUrl => ConfigHelper.GetString("BaseUrl", "http://localhost:5000");

        [Test, Order(1), Retry(1), CancelAfter(25000)]
        [TestCaseSource(typeof(Data.CommonData), nameof(Data.CommonData.LoginValidAdmin))]
        public void Login_Success_Admin(string email, string password)
        {
            var page = new LoginPage(Driver);
            page.Open(BaseUrl);
            page.Login(email, password);
            Assert.That(page.SuccessText(), Is.Not.Empty, "Powinien pojawić się komunikat sukcesu po zalogowaniu (admin).");
        }

        [Test, Order(2), Retry(1), CancelAfter(25000)]
        [TestCaseSource(typeof(Data.CommonData), nameof(Data.CommonData.LoginValidUser))]
        public void Login_Success_User(string email, string password)
        {
            var page = new LoginPage(Driver);
            page.Open(BaseUrl);
            page.Login(email, password);
            Assert.That(page.SuccessText(), Is.Not.Empty, "Powinien pojawić się komunikat sukcesu po zalogowaniu (użytkownik).");
        }

        [Test, Order(3), Retry(1), CancelAfter(25000)]
        [TestCaseSource(typeof(Data.CommonData), nameof(Data.CommonData.LoginInvalid))]
        public void Login_Fail(string email, string password)
        {
            var page = new LoginPage(Driver);
            page.Open(BaseUrl);
            page.Login(email, password);

            var hasGlobal = !string.IsNullOrEmpty(page.ErrorText());
            var emailVal = page.EmailInput().GetAttribute("validationMessage") ?? "";
            var passVal  = page.PasswordInput().GetAttribute("validationMessage") ?? "";

            Assert.That(hasGlobal || !string.IsNullOrEmpty(emailVal) || !string.IsNullOrEmpty(passVal), Is.True,
                "Dla złych lub pustych danych oczekujemy globalnego błędu albo walidacji HTML5 na polach.");
        }
    }
}
