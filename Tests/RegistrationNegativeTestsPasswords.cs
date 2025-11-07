using NUnit.Framework;
using WPSF.NUnitSelenium.Tests.Pages;
using WPSF.NUnitSelenium.Tests.Utils;
using Data = WPSF.NUnitSelenium.Tests.Data;

namespace WPSF.NUnitSelenium.Tests.Tests.Negative
{
    [TestFixture]
    public class RegistrationNegativeTestsPasswords : TestBase
    {
        private string BaseUrl => ConfigHelper.GetString("BaseUrl", "http://localhost:5000");

        [Test, Order(1), Retry(1), CancelAfter(30000), Category("Register")]
        [TestCaseSource(typeof(Data.CommonData), nameof(Data.CommonData.RegistrationInvalidPasswords))]
        public void Register_MismatchedPasswords_ShowsError(string fullName, string email, string password, string confirm)
        {
            var page = new RegisterPage(Driver);
            page.Open(BaseUrl);
            page.Register(fullName, email, password, confirm);

            // Oczekujemy błędu (alert na stronie) — dla mismatch zwykle server-side
            Assert.That(page.ErrorText(), Is.Not.Empty,
                "Powinien pojawić się komunikat błędu dla niezgodnych haseł.");
        }
    }
}
