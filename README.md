# WPSF NUnit+Selenium – Stable v7 (baseline)

Ta **bazowa** wersja 7 odpowiada dokładnie paczce z wczoraj: POM-y, 9 testów, utilsy, logowanie.
Domyślnie `Headless=false`, aby można było obserwować przebieg testów.

## Uruchomienie
```bash
dotnet test -v n
```
Skonfiguruj `AppSettings/appsettings.json` (BaseUrl/Browser/Headless).

Wygenerowano 2025-09-22 18:00 UTC.


## Zmiany (logic-fix: kolejność i dane logowania)
- Dodane rozróżnienie testów logowania: **Admin** i **Użytkownik** (TC-01 / TC-01B).
- `AdminTests` ma test **Admin_Login_Smoke** z `Order(0)` przed `Admin_Add_Product`.
- **UWAGA**: każdy test startuje z nową sesją przeglądarki, więc kolejność nie przenosi zalogowania — logowanie w `Admin_Add_Product` pozostaje w teście.
- Dane logowania używane w testach:
  - Admin: `admin@example.com` / `admin123`
  - Użytkownik: `test@gmail.com` / `test12`

Wygenerowano 2025-09-22 18:12 UTC.
