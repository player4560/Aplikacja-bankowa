KoKaBank - Aplikacja Bankowa

Opis Projektu
KoKaBank to demonstracyjna aplikacja desktopowa stworzona w technologii WPF (Windows Presentation Foundation) na projekt zaliczeniowy z programowania obiektowego. Aplikacja symuluje podstawowe funkcjonalności dla użytkownika aplikacji bankowej, umożliwiając użytkownikom zarządzanie kontem bankowym, wykonywanie przelewów oraz przeglądanie historii transakcji.

Główne Funkcjonalności

Zarządzanie Kontem
- **Rejestracja nowego konta** - tworzenie konta z danymi osobowymi
- **Logowanie do systemu** - bezpieczny dostęp z numerem klienta i hasłem
- **Informacje o koncie** - przeglądanie danych osobowych i stanu konta
- **Zmiana hasła** - możliwość resetowania hasła dostępu

Operacje Finansowe
- **Przeglądanie salda** - aktualne środki na koncie
- **Historia transakcji** - kompletna lista wszystkich operacji
- **Przelewy wewnętrzne** - transfery między kontami w systemie KoKaBank
- **Przelewy zewnętrzne** - transfery do innych banków
- **Bonus powitalny** - 1000 PLN dla nowych użytkowników

Bezpieczeństwo
- **Hashowanie haseł** - bezpieczne przechowywanie danych dostępowych
- **Walidacja danych** - sprawdzanie poprawności wprowadzanych informacji
- **Kontrola dostępu** - autoryzacja przed dostępem do funkcji

Technologie

- **C#** - język programowania
- **WPF (.NET Framework 4.7.2)** - framework interfejsu użytkownika
- **SQLite** - lokalna baza danych
- **XAML** - język znaczników dla interfejsu
- **Visual Studio 2022** - środowisko deweloperskie

Wymagania Systemowe

### Wymagania Minimalne:
- **System Operacyjny:** Windows 7/8/10/11
- **Framework:** .NET Framework 4.7.2 lub nowszy
- **RAM:** 512 MB (zalecane 2 GB)
- **Miejsce na dysku:** 50 MB

### Wymagania Deweloperskie:
- **Visual Studio 2022** (Community/Professional/Enterprise)
- **NuGet Package Manager**
- **Git** (opcjonalnie)

Instalacja i Uruchomienie

Otwarcie w Visual Studio
1. Uruchom **Visual Studio 2022**
2. Otwórz plik `KoKaBank.sln`
3. Poczekaj na załadowanie projektu

2. Instalacja Zależności
W **Package Manager Console** wykonaj:
```
Install-Package System.Data.SQLite.Core
```

3. Kompilacja i Uruchomienie
1. **Build** → **Build Solution** (Ctrl+Shift+B)
2. **Debug** → **Start Debugging** (F5)

Architektura Projektu

Struktura Plików
```
KoKaBank/
├── 📄 MainWindow.xaml/.cs          # Okno logowania
├── 📄 ClientPanelWindow.xaml/.cs   # Panel główny klienta
├── 📄 RegisterWindow.xaml/.cs      # Okno rejestracji
├── 📄 InternalTransferWindow.xaml/.cs # Okno przelewów wewnętrznych
├── 📄 ExTransferWindow.xaml/.cs    # Okno przelewów zewnętrznych
├── 📄 ResetPasswordWindow.xaml/.cs # Okno zmiany hasła
├── 📄 AlertWindow.xaml/.cs         # Okno alertów bezpieczeństwa
├── 📄 DatabaseHelper.cs            # Obsługa bazy danych
├── 📄 AmountToColorConverter.cs    # Konwerter kolorów kwot
├── 📄 NegativeValueConverter.cs    # Konwerter wartości ujemnych
├── 📄 Transaction.cs               # Model transakcji
└── 📄 App.xaml/.cs                 # Konfiguracja aplikacji
```

Model Bazy Danych

#### Tabela `Users`
- `Id` - Klucz główny
- `ClientId` - Unikalny numer klienta
- `Password` - Hasło dostępu
- `FullName` - Imię i nazwisko
- `Nationality` - Obywatelstwo
- `Pesel` - Numer PESEL
- `IdNumber` - Numer dowodu osobistego
- `Balance` - Saldo konta
- `CreatedDate` - Data utworzenia konta

#### Tabela `Transactions`
- `Id` - Klucz główny
- `ClientId` - Powiązanie z użytkownikiem
- `Date` - Data transakcji
- `Description` - Opis operacji
- `Amount` - Kwota (+ wpływ, - wydatek)
- `BalanceAfter` - Saldo po transakcji
- `RecipientClientId` - Odbiorca (dla przelewów wewnętrznych)
- `TransferType` - Typ transakcji

## 💻 Jak Używać

Pierwsze Uruchomienie
- Aplikacja automatycznie utworzy bazę danych
- Dostępne domyślne konto: **123456** / **admin**

Rejestracja Nowego Konta
1. Kliknij **"Rejestracja"**
2. Wypełnij formularz danymi osobowymi
3. Zapamiętaj swój numer klienta
4. Otrzymasz bonus powitalny 1000 PLN

Logowanie
1. Wprowadź **numer klienta** i **hasło**
2. Kliknij **"Zaloguj"**
3. Przeczytaj alert bezpieczeństwa

Wykonywanie Przelewów
Przelew Wewnętrzny:
1. Kliknij **"Przelew wewnętrzny"**
2. Podaj numer klienta odbiorcy
3. Kliknij **"Sprawdź"** aby zweryfikować odbiorcę
4. Wprowadź kwotę i opis
5. Potwierdź operację

Przelew Zewnętrzny:
1. Kliknij **"Przelew zewnętrzny"**
2. Wprowadź dane odbiorcy
3. Podaj kwotę
4. Potwierdź przelew

Wzorce Projektowe

Repository Pattern
- `DatabaseHelper` - centralne zarządzanie dostępem do danych

Alerty Bezpieczeństwa
- Ostrzeżenia o ochronie danych
- Walidacja wprowadzanych danych
- Potwierdzenia operacji finansowych

Responsywny Design
- Intuicyjny interfejs
- Czytelne formularze
- Przejrzysta nawigacja

Testowanie

Konta Testowe:
```
Numer klienta: 123456
Hasło: admin
Saldo: ~5000 PLN
```

Scenariusze Testowe:
1. **Rejestracja** → Logowanie → Przelew wewnętrzny
2. **Zmiana hasła** → Wylogowanie → Logowanie nowym hasłem
3. **Transfer między kontami** → Sprawdzenie historii u obu stron

Dokumentacja Kodu

Kluczowe Klasy:

DatabaseHelper`
```csharp
// Obsługa wszystkich operacji bazodanowych
public static bool ValidateUser(string clientId, string password)
public static bool RegisterUser(...)
public static TransferResult ProcessInternalTransfer(...)
```

`BankTransaction`
```csharp
// Model transakcji bankowej
public class BankTransaction
{
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public decimal BalanceAfter { get; set; }
}

Przyszłe Rozszerzenia

- [ ] Integracja z zewnętrznymi API banków
- [ ] System powiadomień email/SMS
- [ ] Zaawansowane raporty finansowe
- [ ] Aplikacja mobilna
- [ ] Multi-walutowość

Autorzy

- Oktawiusz
- Jakub
- Projekt: Informatyka - Zaoczna, Programowanie Obiektowe, Semestr 2

Link do repozytorium na GitHubie

github.com/player4560/Aplikacja-bankowa

Licencja

Projekt stworzony na potrzeby edukacyjne. 
© 2025 KoKaBank - Projekt Zaliczeniowy

**"Twoje finanse, nasza działka"** 💳✨