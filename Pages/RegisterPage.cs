using OpenQA.Selenium;
using WPSF.NUnitSelenium.Tests.Utils;

namespace WPSF.NUnitSelenium.Tests.Pages
{
    public class RegisterPage
    {
        private readonly IWebDriver _driver;
        public const string RelativeUrl = "/register";

        private readonly By _fullName = By.CssSelector("#full_name, input[name='full_name'], input[name='fullname'], input[name='name']");
        private readonly By _email = By.CssSelector("#email, input[name='email']");
        private readonly By _password = By.CssSelector("#password, input[name='password']");
        private readonly By _confirm = By.CssSelector("#password_confirm, input[name='password_confirm'], input[name='confirm_password']");
        private readonly By _submit = By.CssSelector("form[action$='/register'] button[type='submit'], button[type='submit']");
        private readonly By _success = By.CssSelector(".alert-success, [data-test='success']");
        private readonly By _error   = By.CssSelector(".alert-danger, .alert-error, [data-test='error']");

        public RegisterPage(IWebDriver d) => _driver = d;

        public void Open(string baseUrl) => _driver.Navigate().GoToUrl(baseUrl.TrimEnd('/') + RelativeUrl);

        public void Register(string fullName, string email, string password, string confirm)
        {
            _driver.UntilVisible(_fullName).ClearAndType(fullName);
            _driver.UntilVisible(_email).ClearAndType(email);
            _driver.UntilVisible(_password).ClearAndType(password);
            _driver.UntilVisible(_confirm).ClearAndType(confirm);
            _driver.ClickWithScrollAndRetry(_submit);
        }

        public string SuccessText() { try { return _driver.UntilVisible(_success, 3).Text.Trim(); } catch { return string.Empty; } }
        public string ErrorText()   { try { return _driver.UntilVisible(_error, 3).Text.Trim(); } catch { return string.Empty; } }
    }
}
