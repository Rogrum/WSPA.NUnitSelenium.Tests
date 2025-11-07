using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using WPSF.NUnitSelenium.Tests.Utils;

namespace WPSF.NUnitSelenium.Tests.Drivers
{
    public static class WebDriverManager
    {
        public static IWebDriver Create()
        {
            var browser = ConfigHelper.GetString("Browser", "chrome").ToLowerInvariant();
            var headless = ConfigHelper.GetBool("Headless", false);

            switch (browser)
            {
                case "firefox":
                    var fo = new FirefoxOptions();
                    if (headless) fo.AddArgument("-headless");
                    return new FirefoxDriver(fo);
                case "chrome":
                default:
                    var co = new ChromeOptions();
                    if (headless) co.AddArgument("--headless=new");
                    co.AddArgument("--window-size=1920,1080");
                    return new ChromeDriver(co);
            }
        }
    }
}
