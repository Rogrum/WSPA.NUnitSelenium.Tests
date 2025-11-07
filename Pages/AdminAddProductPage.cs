using OpenQA.Selenium;
using WPSF.NUnitSelenium.Tests.Utils;

namespace WPSF.NUnitSelenium.Tests.Pages
{
    public class AdminAddProductPage
    {
        private readonly IWebDriver _driver;

        private readonly By _name = By.CssSelector("#name, input[name='name']");
        private readonly By _brand = By.CssSelector("#brand, input[name='brand']");
        private readonly By _price = By.CssSelector("#price, input[name='price']");
        private readonly By _stock = By.CssSelector("#stock_quantity, input[name='stock_quantity'], input[name*='stock']");
        private readonly By _description = By.CssSelector("#description, textarea[name='description']");
        private readonly By _category = By.CssSelector("#category, select[name='category']");
        private readonly By _image = By.CssSelector("#image, input[type='file'][name='image'], input[type='file']");
        private readonly By _submit = By.CssSelector("form.product-form button[type='submit'], form[action*='add_product'] button[type='submit'], button[type='submit'][data-test='save-product']");
        private readonly By _success = By.CssSelector(".alert-success, [data-test='admin-product-added'], .toast-success, .callout-success");

        public AdminAddProductPage(IWebDriver d) => _driver = d;

        public void Open(string baseUrl) => _driver.Navigate().GoToUrl(baseUrl.TrimEnd('/') + "/admin/add_product");

        public void Fill(string name, string brand, string price, string stock, string desc, string category, string imagePath)
        {
            _driver.UntilVisible(_name).ClearAndType(name);
            _driver.UntilVisible(_brand).ClearAndType(brand);
            _driver.UntilVisible(_price).ClearAndType(price);
            _driver.UntilVisible(_stock).ClearAndType(stock);
            _driver.UntilVisible(_description).ClearAndType(desc);
            try { _driver.UntilVisible(_category,2).SendKeys(category); } catch {}
            try { _driver.FindElement(_image).SendKeys(imagePath); } catch {}
        }

        public void Submit() => _driver.ClickWithScrollAndRetry(_submit, retries: 2, waitSeconds: 6);

        public bool Success() 
        { 
            try { return _driver.UntilVisible(_success, 6).Displayed; } catch { return false; } 
        }
    }
}
