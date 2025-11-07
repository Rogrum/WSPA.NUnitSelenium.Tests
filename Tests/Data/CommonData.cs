using System.Collections.Generic;
using NUnit.Framework;
using WPSF.NUnitSelenium.Tests.Utils;

namespace WPSF.NUnitSelenium.Tests.Data
{
    public static class CommonData
    {
        // --- Credentials ---
        public static IEnumerable<TestCaseData> LoginValidAdmin()
        {
            yield return new TestCaseData("admin@example.com", "admin123")
                .SetName("TC-01 Logowanie poprawne (admin)")
                .SetCategory("Login").SetCategory("TC-01");
        }
        public static IEnumerable<TestCaseData> LoginValidUser()
        {
            yield return new TestCaseData("test@gmail.com", "test12")
                .SetName("TC-01B Logowanie poprawne (użytkownik)")
                .SetCategory("Login").SetCategory("TC-01B");
        }

        public static IEnumerable<TestCaseData> LoginInvalid()
        {
            yield return new TestCaseData("admin@example.com", "wrong")
                .SetName("TC-02 Logowanie błędne hasło")
                .SetCategory("Login").SetCategory("TC-02");
            yield return new TestCaseData("", "")
                .SetName("TC-03 Logowanie puste pola")
                .SetCategory("Login").SetCategory("TC-03");
        }

        // --- Registration ---
        public static IEnumerable<TestCaseData> RegistrationValid()
        {
            yield return new TestCaseData("Jan Kowalski", DataGen.UniqueEmail(), "Test1234!", "Test1234!")
                .SetName("TC-04 Rejestracja poprawna")
                .SetCategory("Registration").SetCategory("TC-04");
        }

        public static IEnumerable<TestCaseData> RegistrationInvalid()
        {
            yield return new TestCaseData("", "", "", "")
                .SetName("TC-05 Rejestracja puste pola")
                .SetCategory("Registration").SetCategory("TC-05");
        }

        // --- Search ---
        public static IEnumerable<TestCaseData> SearchTerms()
        {
            yield return new TestCaseData("aparat").SetName("TC-06 Szukaj prostej frazy").SetCategory("Search").SetCategory("TC-06");
        }

        // --- Checkout ---
        public static IEnumerable<TestCaseData> Addresses()
        {
            yield return new TestCaseData("Jan Kowalski", "ul. Testowa 1", "Warszawa", "00-001", "111111")
                .SetName("TC-08 Checkout podstawowy")
                .SetCategory("Checkout").SetCategory("TC-08");
        }

        // --- Review ---
        public static IEnumerable<TestCaseData> Reviews()
        {
            yield return new TestCaseData(5, "Świetny sprzęt!")
                .SetName("TC-10 Dodanie recenzji 5/5")
                .SetCategory("Review").SetCategory("TC-10");
        }

        // --- Admin add product ---
        public static IEnumerable<TestCaseData> AdminAddProductCases()
        {
            yield return new TestCaseData(
                "admin@example.com", "admin123",
                $"Testowy aparat {System.DateTime.UtcNow:HHmmss}",
                "WPSF", "1999.99", "5", "Aparat testowy", "Aparaty", "TestAssets/images/Aparat.jpg")
                .SetName("TC-11 Admin: dodanie produktu")
                .SetCategory("Admin").SetCategory("TC-11");
        }

        // NEGATYWNE: Różne hasła – email poprawny (izolujemy błąd haseł)
        public static IEnumerable<TestCaseData> RegistrationInvalidPasswords()
        {
            yield return new TestCaseData(
                "Jan Kowalski",
                $"neg.pw.{DateTime.UtcNow:yyyyMMddHHmmssfff}@example.com",
                "Haslo123!",
                "Haslo123!!"
            ).SetName("TC-02A_pw_mismatch_1");

            yield return new TestCaseData(
                "Anna Nowak",
                $"neg.pw.a.{DateTime.UtcNow:yyyyMMddHHmmssfff}@example.com",
                "Qwerty!1",
                "Qwerty!12"
            ).SetName("TC-02A_pw_mismatch_2");
        }

        // NEGATYWNE: Błędny email – hasła zgodne (izolujemy błąd email)
        public static IEnumerable<TestCaseData> RegistrationInvalidEmails()
        {
            yield return new TestCaseData(
                "Piotr Test",
                "zlyemail",          // brak '@'
                "Haslo123!",
                "Haslo123!"
            ).SetName("TC-02B_email_no_at");

            yield return new TestCaseData(
                "Ewa Zielińska",
                "user@@example.com", // podwójny '@'
                "Haslo123!",
                "Haslo123!"
            ).SetName("TC-02B_email_double_at");

            yield return new TestCaseData(
                "Marek Nowy",
                "test@invalid_domain", // zła domena
                "Haslo123!",
                "Haslo123!"
            ).SetName("TC-02B_email_bad_domain");

            yield return new TestCaseData(
                "Ola Maj",
                "test@",              // obcięty po '@'
                "Haslo123!",
                "Haslo123!"
            ).SetName("TC-02B_email_trailing_at");
        }
    }
}
