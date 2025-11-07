using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using WPSF.NUnitSelenium.Tests.Utils;

namespace WPSF.NUnitSelenium.Tests.Tests.Negative
{
    [TestFixture]
    public class LoginNegativeTests : TestBase
    {
        private string BaseUrl => ConfigHelper.GetString("BaseUrl", "http://localhost:5000");

        [TestCase(TestName = "TC-04"), Order(1), Retry(1), CancelAfter(30000), Category("Login")]
        public void Login_InvalidCredentials_ShowsError()
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/login");

            var email = Driver.FindElement(By.CssSelector("input[name='email'], #email"));
            var pass = Driver.FindElement(By.CssSelector("input[name='password'], #password"));
            var submit = Driver.FindElement(By.CssSelector("button[type='submit'], [data-test='login-submit'], button[name='login']"));

            email.SendKeys("nieistniejacy@example.com");
            pass.SendKeys("zlehaslo");
            submit.Click();

            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(6));
            wait.Until(_ => Driver.FindElements(By.CssSelector(".alert-danger, [data-test='login-error'], .invalid-feedback")).Count > 0);

            Assert.That(Driver.FindElements(By.CssSelector(".alert-danger, [data-test='login-error'], .invalid-feedback")).Count,
                Is.GreaterThan(0), "Powinien pojawić się komunikat o błędnych danych logowania.");
        }
    }
}
