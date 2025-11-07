using NUnit.Framework;
using NUnit.Framework.Legacy;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using WPSF.NUnitSelenium.Tests.Utils;

namespace WPSF.NUnitSelenium.Tests.Tests.Negative
{
    [TestFixture]
    public class SearchNegativeTests : TestBase
    {
        private string BaseUrl => ConfigHelper.GetString("BaseUrl", "http://localhost:5000");

        [TestCase(TestName = "TC-06"), Order(1), Retry(1), CancelAfter(20000), Category("Search")]
        public void Search_EmptyPhrase_ShowsValidation()
        {
            Driver.Navigate().GoToUrl(BaseUrl);

            // 1) klik"Szukaj"
            var searchBtn = Driver.FindElement(By.XPath("//button[@type='submit' and contains(normalize-space(.), 'Szukaj')]"));

            // 2) input z tego samego formularza
            var form = searchBtn.FindElement(By.XPath("ancestor::form[1]"));
            var input = form.FindElement(By.CssSelector("input[type='search'], input[name='q'], input[name='query'], input[required]"));

            // 3) klik bez wpisywania czegokolwiek
            searchBtn.Click();

            // 4) natywny komunikat walidacji HTML5
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(3));
            string msg = wait.Until(_ =>
                (string)((IJavaScriptExecutor)Driver).ExecuteScript("return arguments[0].validationMessage;", input)
            );

            StringAssert.Contains("wypełnij to pole", msg.ToLowerInvariant(), "Powinien pojawić się komunikat „wypełnij to pole”.");
        }
    }
}
