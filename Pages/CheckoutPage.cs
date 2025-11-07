using OpenQA.Selenium;
using WPSF.NUnitSelenium.Tests.Utils;

namespace WPSF.NUnitSelenium.Tests.Pages
{
    public class CheckoutPage
    {
        private readonly IWebDriver _driver;
        private readonly By _fullName = By.CssSelector("#full_name, input[name='full_name'], input[name='fullname'], input[name='name'], input[name*='full'], input[placeholder*='Imię'], input[placeholder*='imi'], input[placeholder*='name' i]");
        private readonly By _address = By.CssSelector("#address, input[name='address'], input[name*='adres'], input[placeholder*='adres' i]");
        private readonly By _city = By.CssSelector("#city, input[name='city'], input[name*='miasto'], input[placeholder*='miasto' i]");
        private readonly By _postal = By.CssSelector("#postal_code, input[name='postal_code'], input[name='postcode'], input[name*='kod'], input[placeholder*='kod' i]");
        private readonly By _blik = By.CssSelector("#blik_code, input[name='blik_code'], input[name*='blik']");
        private readonly By _submit = By.CssSelector("form[action$='/checkout'] button[type='submit'], button[type='submit'][name*='order'], button[data-test='place-order']");
        private readonly By _confirmation = By.CssSelector("[data-test='order-confirmation'], .alert-success, .order-confirmation, [data-test*='thank']");

        public CheckoutPage(IWebDriver d) => _driver = d;

        public void Fill(string fullName, string address, string city, string postal, string? blik = null)
        {
            var f = _driver.UntilVisible(_fullName, 8); f.ClearAndType(fullName);
            var a = _driver.UntilVisible(_address, 4); a.ClearAndType(address);
            try { var c = _driver.UntilVisible(_city, 2); c.ClearAndType(city); } catch {}
            try { var p = _driver.UntilVisible(_postal, 2); p.ClearAndType(postal); } catch {}
            if (!string.IsNullOrWhiteSpace(blik))
            {
                try { var b = _driver.UntilVisible(_blik, 1); b.ClearAndType(blik); } catch {}
            }
        }

        public void Submit() => _driver.ClickWithScrollAndRetry(_submit, retries: 2, waitSeconds: 6);

        public bool IsConfirmed()
        {
            try { return _driver.UntilVisible(_confirmation, 8).Displayed; } catch { return false; }
        }
    }
}
