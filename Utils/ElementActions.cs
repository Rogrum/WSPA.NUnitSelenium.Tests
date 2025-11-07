using System;
using OpenQA.Selenium;

namespace WPSF.NUnitSelenium.Tests.Utils
{
    public static class ElementActions
    {
        public static void JsClick(this IWebDriver driver, IWebElement el) =>
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click()", el);

        public static void ClickWithScrollAndRetry(this IWebDriver driver, By by, int retries = 2, int waitSeconds = 0)
        {
            for (int i = 0; i <= retries; i++)
            {
                try
                {
                    var el = driver.UntilClickable(by, waitSeconds);
                    driver.ScrollIntoView(el);
                    el.Click();
                    return;
                }
                catch (StaleElementReferenceException)
                {
                    if (i == retries) throw;
                    System.Threading.Thread.Sleep(150);
                }
                catch (ElementClickInterceptedException)
                {
                    if (i == retries)
                    {
                        var el = driver.UntilVisible(by, waitSeconds);
                        driver.ScrollIntoView(el);
                        driver.JsClick(el);
                        return;
                    }
                    System.Threading.Thread.Sleep(200);
                }
            }
        }

        public static void ClearAndType(this IWebElement el, string text)
        {
            try { el.Clear(); } catch {}
            el.SendKeys(text);
        }
    }
}
