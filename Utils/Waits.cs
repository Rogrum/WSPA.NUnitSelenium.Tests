using System;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace WPSF.NUnitSelenium.Tests.Utils
{
    public static class Waits
    {
        private static int DefaultSeconds => ConfigHelper.GetInt("DefaultWaitSeconds", 12);

        public static WebDriverWait Wait(this IWebDriver driver, int seconds = 0)
        {
            var timeout = seconds > 0 ? TimeSpan.FromSeconds(seconds) : TimeSpan.FromSeconds(DefaultSeconds);
            return new WebDriverWait(new SystemClock(), driver, timeout, TimeSpan.FromMilliseconds(250));
        }

        public static IWebElement UntilVisible(this IWebDriver driver, By by, int seconds = 0) =>
            driver.Wait(seconds).Until(ExpectedConditions.ElementIsVisible(by));

        public static IWebElement UntilClickable(this IWebDriver driver, By by, int seconds = 0) =>
            driver.Wait(seconds).Until(ExpectedConditions.ElementToBeClickable(by));

        public static bool Exists(this IWebDriver driver, By by, int seconds = 0)
        {
            try
            {
                driver.Wait(seconds).Until(drv => drv.FindElements(by).Any());
                return true;
            }
            catch { return false; }
        }

        public static void ScrollIntoView(this IWebDriver driver, IWebElement el) =>
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'})", el);

        public static void WaitForDocumentReady(this IWebDriver driver, int seconds = 10)
        {
            var end = DateTime.UtcNow.AddSeconds(seconds);
            while (DateTime.UtcNow < end)
            {
                try
                {
                    var state = (string)((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState");
                    if (state == "complete") return;
                }
                catch { }
                System.Threading.Thread.Sleep(100);
            }
        }
    }
}
