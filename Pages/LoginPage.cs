using OpenQA.Selenium;
using WPSF.NUnitSelenium.Tests.Utils;

namespace WPSF.NUnitSelenium.Tests.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;
        public const string RelativeUrl = "/login";

        private readonly By _email = By.CssSelector("#email, input[name='email']");
        private readonly By _password = By.CssSelector("#password, input[name='password']");
        private readonly By _submit = By.CssSelector("form[action$='/login'] button[type='submit'], button[type='submit']");
        private readonly By _success = By.CssSelector(".alert-success, [data-test='success']");
        private readonly By _error   = By.CssSelector(".alert-danger, .alert-error, [data-test='error']");

        public LoginPage(IWebDriver driver) => _driver = driver;
        public void Open(string baseUrl) => _driver.Navigate().GoToUrl(baseUrl.TrimEnd('/') + RelativeUrl);

        public void Login(string email, string password)
        {
            _driver.UntilVisible(_email).ClearAndType(email);
            _driver.UntilVisible(_password).ClearAndType(password);
            _driver.ClickWithScrollAndRetry(_submit);
        }

        public string SuccessText() { try { return _driver.UntilVisible(_success, 5).Text.Trim(); } catch { return string.Empty; } }
        public string ErrorText()   { try { return _driver.UntilVisible(_error, 2).Text.Trim(); } catch { return string.Empty; } }

        public IWebElement EmailInput() => _driver.FindElement(_email);
        public IWebElement PasswordInput() => _driver.FindElement(_password);
    }
}
