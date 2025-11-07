using System;
using System.Linq;
using OpenQA.Selenium;
using WPSF.NUnitSelenium.Tests.Utils;

namespace WPSF.NUnitSelenium.Tests.Pages
{
    public class SearchResults
    {
        private readonly IWebDriver _driver;

        // Ka¿dy produkt na listach jest linkiem do /product/{id}
        private readonly By _productAnchors = By.CssSelector("a[href^='/product/']");

        public SearchResults(IWebDriver d) => _driver = d;

        // Czekamy do 5 s a¿ pojawi siê przynajmniej jeden produkt
        public bool Any() => _driver.Exists(_productAnchors, 5);

        // Dopasowanie po tytule w <h5> (zgodnie z search_results.html)
        public bool ContainsByName(string name)
        {
            var needle = (name ?? "").Trim();
            if (string.IsNullOrEmpty(needle)) return false;

            try
            {
                var anchors = _driver.FindElements(_productAnchors);
                return anchors.Any(a =>
                {
                    try
                    {
                        var h5 = a.FindElement(By.CssSelector("h5"));
                        var text = (h5.Text ?? "").Trim();
                        return text.IndexOf(needle, StringComparison.OrdinalIgnoreCase) >= 0;
                    }
                    catch { return false; }
                });
            }
            catch { return false; }
        }
    }
}
