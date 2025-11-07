using OpenQA.Selenium;
using WPSF.NUnitSelenium.Tests.Utils;

namespace WPSF.NUnitSelenium.Tests.Pages
{
    public class HeaderSearch
    {
        private readonly IWebDriver _driver;

        private readonly By _input = By.CssSelector("form[action$='/search'] input[name='q'], input[name='q']");
        private readonly By _submit = By.CssSelector("form[action$='/search'] button[type='submit'], button[type='submit'][data-test='search']");

        public HeaderSearch(IWebDriver d) => _driver = d;

        public void Search(string term)
        {
            _driver.UntilVisible(_input).ClearAndType(term);
            _driver.ClickWithScrollAndRetry(_submit);
        }
    }
}
