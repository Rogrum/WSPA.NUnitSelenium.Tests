using NUnit.Framework;
using WPSF.NUnitSelenium.Tests.Pages;
using WPSF.NUnitSelenium.Tests.Utils;

namespace WPSF.NUnitSelenium.Tests.Tests
{
    [TestFixture]
    public class SearchTests : TestBase
    {
        private string BaseUrl => ConfigHelper.GetString("BaseUrl", "http://localhost:5000");

        [Test, Order(1), Retry(1), CancelAfter(30000), Category("Search")]
        [TestCase("Nikon D7500")]
        public void Search_ByPhrase_FindsProduct_ByH5(string term)
        {
            // Start z homepage
            Driver.Navigate().GoToUrl(BaseUrl);

            var search = new HeaderSearch(Driver);
            search.Search(term);

            var results = new SearchResults(Driver);
            Assert.That(results.Any(), Is.True, "Powinny pojawić się wyniki wyszukiwania.");
            Assert.That(results.ContainsByName(term), Is.True, $"Lista wyników powinna zawierać produkt „{term}”.");
        }
    }
}
