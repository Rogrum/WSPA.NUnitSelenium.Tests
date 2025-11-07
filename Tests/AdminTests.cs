using NUnit.Framework;
using WPSF.NUnitSelenium.Tests.Pages;
using WPSF.NUnitSelenium.Tests.Utils;
using System.IO;
using Data = WPSF.NUnitSelenium.Tests.Data;

namespace WPSF.NUnitSelenium.Tests.Tests
{
    [TestFixture, Order(2)]
    public class AdminTests : TestBase
    {
        private string BaseUrl => ConfigHelper.GetString("BaseUrl", "http://localhost:5000");

        [Test, Order(0), Retry(1), CancelAfter(25000), Category("Admin"), Category("Smoke")]
        [TestCaseSource(typeof(Data.CommonData), nameof(Data.CommonData.LoginValidAdmin))]
        public void Admin_Login_Smoke(string email, string password)
        {
            var login = new LoginPage(Driver);
            login.Open(BaseUrl);
            login.Login(email, password);
            Assert.That(login.SuccessText(), Is.Not.Empty, "Admin powinien móc się zalogować.");
        }

        [Test, Order(1), Retry(1), CancelAfter(50000)]
        [TestCaseSource(typeof(Data.CommonData), nameof(Data.CommonData.AdminAddProductCases))]
        public void Admin_Add_Product(string email, string password, string name, string brand, string price, string stock, string desc, string category, string imageRelPath)
        {
            var login = new LoginPage(Driver);
            login.Open(BaseUrl);
            login.Login(email, password);

            var admin = new AdminAddProductPage(Driver);
            admin.Open(BaseUrl);

            var imgFullPath = Path.Combine(TestContext.CurrentContext.WorkDirectory, imageRelPath.Replace("/", System.IO.Path.DirectorySeparatorChar.ToString()));
            admin.Fill(name, brand, price, stock, desc, category, imgFullPath);
            admin.Submit();

            // If no explicit alert — verify presence via search
            var okByAlert = admin.Success();

            var search = new HeaderSearch(Driver);
            search.Search(name);
            var results = new SearchResults(Driver);
            var okByPresence = results.Any() && results.ContainsByName(name);

            Assert.That(okByAlert || okByPresence, Is.True, "Produkt powinien być dodany (alert sukcesu lub widoczny w wynikach wyszukiwania).");
        }
    }
}
