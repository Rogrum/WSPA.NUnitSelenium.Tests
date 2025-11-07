using System;
using System.IO;
using NUnit.Framework;
using OpenQA.Selenium;
using WPSF.NUnitSelenium.Tests.Drivers;
using WPSF.NUnitSelenium.Tests.Logging;

namespace WPSF.NUnitSelenium.Tests
{
    [Parallelizable(ParallelScope.Fixtures)]
    public abstract class TestBase
    {
        protected IWebDriver Driver = default!;

        [SetUp]
        public virtual void SetUp()
        {
            Directory.CreateDirectory(Path.Combine(TestContext.CurrentContext.WorkDirectory, "artifacts", "screenshots"));
            Logger.Info($"Starting {TestContext.CurrentContext.Test.FullName}");
            Driver = WebDriverManager.Create();
            Driver.Manage().Window.Size = new System.Drawing.Size(1280, 900);
        }

        [TearDown]
        public virtual void TearDown()
        {
            try
            {
                var status = TestContext.CurrentContext.Result.Outcome.Status;
                if (status == NUnit.Framework.Interfaces.TestStatus.Failed && Driver is ITakesScreenshot ts)
                {
                    var file = Path.Combine(TestContext.CurrentContext.WorkDirectory, "artifacts", "screenshots", $"{San(TestContext.CurrentContext.Test.Name)}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.png");
                    ts.GetScreenshot().SaveAsFile(file);
                    TestContext.AddTestAttachment(file, "Screenshot on failure");
                    Logger.Error($"Saved screenshot: {file}");
                }
            }
            finally
            {
                try { Driver?.Quit(); } catch {}
                try { Driver?.Dispose(); } catch {}
            }
        }

        protected static string San(string s)
        {
            foreach (var c in Path.GetInvalidFileNameChars()) s = s.Replace(c, '_');
            return s;
        }
    }
}
