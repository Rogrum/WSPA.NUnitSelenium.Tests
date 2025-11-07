using System.Linq;
using OpenQA.Selenium;
using WPSF.NUnitSelenium.Tests.Utils;

namespace WPSF.NUnitSelenium.Tests.Pages
{
    public class ProductCard
    {
        private readonly IWebDriver _driver;
        public ProductCard(IWebDriver d) => _driver = d;

        private readonly By _cards = By.CssSelector(".product-card, [data-test='product-card']");
        private readonly By _addToCartButtons = By.CssSelector("form[action$='/add_to_cart'] button[type='submit'], button[data-test='add-to-cart']");
        private readonly By _cardFirstLink = By.CssSelector(".product-card a[href*='/product'], [data-test='product-card'] a[href*='/product']");
        private readonly By _toast = By.CssSelector(".toast-success, .alert-success, [data-test='added-to-cart']");
        private readonly By _cartCount = By.CssSelector("[data-test='cart-count']");

        public void AddFirstVisibleToCart()
        {
            if (_driver.Exists(_cards, 5) && _driver.Exists(_addToCartButtons, 1))
            {
                _driver.ClickWithScrollAndRetry(_addToCartButtons);
            }
            else
            {
                if (_driver.Exists(_cardFirstLink, 5))
                {
                    _driver.ClickWithScrollAndRetry(_cardFirstLink);
                }
                if (_driver.Exists(_addToCartButtons, 5))
                {
                    _driver.ClickWithScrollAndRetry(_addToCartButtons);
                }
            }
            try
            {
                if (_driver.Exists(_toast, 2)) _driver.UntilVisible(_toast, 2);
                if (_driver.Exists(_cartCount, 1)) _driver.UntilVisible(_cartCount, 2);
            }
            catch {}
        }
    }
}
