using OpenQA.Selenium;
using WPSF.NUnitSelenium.Tests.Utils;

namespace WPSF.NUnitSelenium.Tests.Pages
{
    /// <summary>
    /// Widok kategorii (np. „Aparaty”) wraz z akcją dodania produktu do koszyka.
    /// Zgodne z templates/category.html.
    /// </summary>
    public class Category
    {
        private readonly IWebDriver _driver;

        // Link w nawigacji do „Aparaty” (tekst lub link ze slugiem /category/aparaty)
        private readonly By _navAparaty = By.XPath("//a[normalize-space()='Aparaty' or contains(@href, '/category/aparaty')]");

        // Każdy produkt jest kotwicą do /product/{id}
        private readonly By _productAnchors = By.CssSelector("a[href^='/product/']");

        public Category(IWebDriver d) => _driver = d;

        public void GoToAparaty()
        {
            // Kliknięcie linku w top-nawie i oczekiwanie na pojawienie się kafelków
            _driver.ClickWithScrollAndRetry(_navAparaty, retries: 2, waitSeconds: 6);
            _driver.Exists(_productAnchors, 8);
        }

        public void OpenProductDetails(string productName)
        {
            var productAnchor = By.XPath(
                "//a[starts-with(@href,'/product/')][.//h3[normalize-space()=$name]]"
                .Replace("$name", EscapeXPathLiteral(productName))
            );
            _driver.ClickWithScrollAndRetry(productAnchor, retries: 1, waitSeconds: 4);
        }


        /// <summary>
        /// Dodaje do koszyka produkt po nazwie (nazwa w <h5>) z widoku kategorii.
        /// Jeśli w kafelku nie ma przycisku, wchodzi w szczegóły i dodaje z detalu.
        /// </summary>
        public void AddToCartFromCategoryByName(string productName)
        {
            // 1) Spróbuj kliknąć przycisk z kafelka w kategorii (formularz POST „Dodaj do koszyka”)
            var addBtnInTile = By.XPath(
                "//a[starts-with(@href,'/product/')][.//h5[normalize-space()=$name]]" +
                "/ancestor::*[self::div or self::a][1]/following-sibling::form//button[normalize-space()='Dodaj do koszyka' or @data-test='add-to-cart']"
                .Replace("$name", EscapeXPathLiteral(productName))
            );

            if (_driver.Exists(addBtnInTile, 3))
            {
                _driver.ClickWithScrollAndRetry(addBtnInTile, retries: 1, waitSeconds: 3);
                return;
            }

            // 2) Fallback – wejdź w szczegóły produktu i kliknij przycisk na stronie produktu
            var productAnchor = By.XPath(
                "//a[starts-with(@href,'/product/')][.//h3[normalize-space()=$name]]"
                .Replace("$name", EscapeXPathLiteral(productName))
            );

            _driver.ClickWithScrollAndRetry(productAnchor, retries: 1, waitSeconds: 4);

            var addOnDetails = By.XPath("//button[normalize-space()='Dodaj do koszyka' or @data-test='add-to-cart']");
            _driver.ClickWithScrollAndRetry(addOnDetails, retries: 1, waitSeconds: 4);
        }

        private static string EscapeXPathLiteral(string input)
        {
            if (input is null) return "''";
            if (!input.Contains("'")) return $"'{input}'";
            if (!input.Contains("\"")) return $"\"{input}\"";
            var parts = input.Split('\'');
            return "concat('" + string.Join("',\"'\",'", parts) + "')";
        }
    }
}
