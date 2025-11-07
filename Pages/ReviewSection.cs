using System;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using WPSF.NUnitSelenium.Tests.Utils;

namespace WPSF.NUnitSelenium.Tests.Pages
{
    /// <summary>
    /// Sekcja opinii na stronie produktu: ocena + treœæ.
    /// </summary>
    public class ReviewSection
    {
        private readonly IWebDriver _driver;

        // Formularz (elastycznie)
        private readonly By _form = By.CssSelector(
            "form#review-form, form[data-test='review-form'], form[action*='review'], form[id*='review']"
        );

        // RATING – ró¿ne warianty: select, radio, „gwiazdki”
        private readonly By _ratingSelect = By.CssSelector("select[name='rating'], select[id*='rating']");
        private readonly By _ratingRadios = By.CssSelector("input[type='radio'][name='rating'], input[type='radio'][id*='rating']");
        private readonly By _ratingStars = By.CssSelector(
            "[data-test='rating'] [data-value], .rating [data-value], " +
            "[role='radiogroup'] [role='radio'], .stars [data-value]"
        );

        // TEXTAREA – spróbujemy znaleŸæ mo¿liwie specyficzne, ale mamy te¿ „ostatni¹ deskê”: pierwszy widoczny <textarea> w formie
        private readonly By _textareaPreferred = By.CssSelector(
            "textarea[name='content'], textarea[name='review'], textarea#review, textarea[id*='review'], " +
            "textarea[placeholder*='opini'], textarea[placeholder*='opinia']"
        );
        private readonly By _anyTextareaInForm = By.CssSelector("textarea");

        // SUBMIT
        private readonly By _submitBtn = By.XPath(
            "//form[contains(@id,'review') or @id='review-form' or @data-test='review-form']" +
            "//button[@type='submit' or @data-test='submit-review' or normalize-space()='Dodaj opiniê' or normalize-space()='Dodaj recenzjê']"
        );

        // SUKCES
        private readonly By _success = By.CssSelector(
            ".alert-success, .toast-success, .callout-success, [data-test='review-success']"
        );

        private readonly TimeSpan _timeout = TimeSpan.FromSeconds(12);

        public ReviewSection(IWebDriver driver) => _driver = driver;

        public void AddReview(int rating, string content)
        {
            var wait = new WebDriverWait(_driver, _timeout);

            // 1) Upewnij siê, ¿e widaæ formularz
            wait.Until(drv => drv.Exists(_form, 5));
            var form = _driver.FindElement(_form);
            ScrollIntoView(form);

            // 2) Ustaw ocenê
            SetRating(rating, wait);

            // 3) ZnajdŸ i wype³nij textarea (po wybraniu oceny pole bywa odblokowywane)
            var textarea = FindTextarea(wait);

            // focus + send keys
            new Actions(_driver).MoveToElement(textarea).Click().Perform();
            textarea.Clear();
            textarea.SendKeys(content ?? string.Empty);

            // fallback: jeœli po SendKeys wartoœæ jest pusta/krótka, u¿yj JS + wywo³aj zdarzenia
            var val = (textarea.GetAttribute("value") ?? "").Trim();
            if (val.Length < Math.Min((content ?? "").Length, 3))
            {
                SetValueByJs(textarea, content ?? "");
            }

            // 4) Wyœlij formularz
            ClickSubmit(wait);

            // 5) Oczekuj na potwierdzenie
            wait.Until(drv =>
                drv.Exists(_success, 3) ||
                (drv.PageSource ?? "").IndexOf("dziêkuj", StringComparison.OrdinalIgnoreCase) >= 0
            );
        }

        private void SetRating(int rating, WebDriverWait wait)
        {
            // a) SELECT
            if (_driver.Exists(_ratingSelect, 1))
            {
                var sel = new SelectElement(_driver.FindElement(_ratingSelect));
                sel.SelectByValue(rating.ToString());
                return;
            }

            // b) RADIO value=rating
            var radios = _driver.FindElements(_ratingRadios);
            var radio = radios.FirstOrDefault(r =>
                (r.GetAttribute("value") ?? "").Equals(rating.ToString(), StringComparison.OrdinalIgnoreCase)
            );
            if (radio != null)
            {
                ScrollIntoView(radio);
                if (!radio.Selected)
                    radio.Click();
                return;
            }

            // c) „Gwiazdki” – elementy z data-value / role=radio / aria-label
            var stars = _driver.FindElements(_ratingStars);
            // szukamy po data-value
            var star = stars.FirstOrDefault(s =>
                (s.GetAttribute("data-value") ?? "") == rating.ToString()
            )
            ?? stars.FirstOrDefault(s =>
                (s.GetAttribute("aria-label") ?? "").Trim().Equals(rating.ToString(), StringComparison.OrdinalIgnoreCase) ||
                (s.Text ?? "").Trim().Equals(rating.ToString(), StringComparison.OrdinalIgnoreCase)
            );

            if (star != null)
            {
                ScrollIntoView(star);
                new Actions(_driver).MoveToElement(star).Click().Perform();
                return;
            }

            // d) Awaryjnie: znajdŸ dowolny input liczbowy tekstowy rating i wpisz
            if (_driver.Exists(By.CssSelector("input[name='rating'], input[id*='rating']"), 1))
            {
                var inp = _driver.FindElement(By.CssSelector("input[name='rating'], input[id*='rating']"));
                ScrollIntoView(inp);
                inp.Clear();
                inp.SendKeys(rating.ToString());
            }
        }

        private IWebElement FindTextarea(WebDriverWait wait)
        {
            // Po ratingu czêœæ apek odblokowuje textarea, wiêc czekamy a¿ bêdzie enabled i displayed
            IWebElement textarea = null;

            // Preferowane dopasowania
            if (_driver.Exists(_textareaPreferred, 2))
            {
                textarea = _driver.FindElements(_textareaPreferred)
                    .FirstOrDefault(t => t.Displayed && t.Enabled);
            }

            // Jeœli nie znaleziono – weŸ pierwszy widoczny textarea w obrêbie formularza
            if (textarea == null)
            {
                var form = _driver.FindElement(_form);
                var anyTextareas = form.FindElements(_anyTextareaInForm);
                textarea = anyTextareas.FirstOrDefault(t => t.Displayed && t.Enabled)
                           ?? anyTextareas.FirstOrDefault(); // ostatecznie bierz pierwszy
            }

            if (textarea == null)
                throw new NoSuchElementException("Nie znaleziono pola tekstowego opinii (textarea).");

            // Poczekaj a¿ bêdzie interactable
            wait.Until(drv => textarea.Displayed && textarea.Enabled);
            ScrollIntoView(textarea);

            return textarea;
        }

        private void ClickSubmit(WebDriverWait wait)
        {
            var btns = _driver.FindElements(_submitBtn);
            if (btns.Count > 0)
            {
                var btn = btns[0];

                // przewiñ tak, by przycisk by³ w widoku (œrodek ekranu – stabilnie)
                ((IJavaScriptExecutor)_driver).ExecuteScript(
                    "arguments[0].scrollIntoView({block:'center', inline:'nearest'});", btn);

                // krótki wait a¿ Selenium uzna, ¿e da siê w niego klikn¹æ
                wait.Until(drv => btn.Displayed && btn.Enabled);

                try
                {
                    btn.Click();
                }
                catch (ElementClickInterceptedException)
                {
                    // awaryjnie – gdy coœ zas³oni³o przycisk w ostatniej chwili
                    ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", btn);
                }
                return;
            }

            // fallback: gdyby przycisku nie by³o, wyœlij formularz
            var form = _driver.FindElement(_form);
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].submit();", form);
        }


        private void SetValueByJs(IWebElement el, string value)
        {
            var js = (IJavaScriptExecutor)_driver;
            js.ExecuteScript(
                "arguments[0].value = arguments[1];" +
                "arguments[0].dispatchEvent(new Event('input', { bubbles: true }));" +
                "arguments[0].dispatchEvent(new Event('change', { bubbles: true }));",
                el, value
            );
        }

        private void ScrollIntoView(IWebElement el)
        {
            try
            {
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", el);
            }
            catch { /* ignore */ }
        }
    }
}
