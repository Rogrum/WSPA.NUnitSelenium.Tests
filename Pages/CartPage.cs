using OpenQA.Selenium;
using WPSF.NUnitSelenium.Tests.Utils;

namespace WPSF.NUnitSelenium.Tests.Pages
{
    public class CartPage
    {
        private readonly IWebDriver _driver;
        private readonly By _itemRow = By.CssSelector(".cart-item, [data-test='cart-item']");
        private readonly By _checkoutLink = By.CssSelector("a[href$='/checkout'], [data-test='go-checkout']");

        public CartPage(IWebDriver d) => _driver = d;

        public void Open(string baseUrl) => _driver.Navigate().GoToUrl(baseUrl.TrimEnd('/') + "/cart");

        public bool HasItems()
        {
            try { return _driver.FindElements(_itemRow).Count > 0; }
            catch { return false; }
        }

        public void GoToCheckout() => _driver.ClickWithScrollAndRetry(_checkoutLink);
    }
}
