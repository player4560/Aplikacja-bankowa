KoKaBank - Aplikacja bankowa projekt zaliczeniowy (WPF) 


KoKaBank to demonstracyjna aplikacja desktopowa stworzona w technologii WPF (Windows Presentation Foundation). Umożliwia użytkownikowi założenie konta bankowego z dostępem do salda, listy transakcji oraz wykonanie przelewu. 

Dane użytkowników są przechowywane lokalnie w pamięci aplikacji (w liście użytkowników). //będzie baza danych w pliku JSON? 

Technologie:

C#

WPF (.NET Framework) — projekt uruchamiany w Visual Studio 2022.

Uruchomienie: 

Visual Studio 2022.

Wymagania:

Visual Studio 2022

.NET Framework 4.7.2 

Architektura projektu: 

MainWindow.xaml — okno logowania i rejestracji.

ClientPanelWindow.xaml — panel klienta z saldem i historią transakcji.

RegisterPanel.xaml - panel rejestracji nowego klienta

User.cs — klasa reprezentująca użytkownika (dane konta).

UserStorage.cs — klasa przechowująca i zarządzająca listą użytkowników.

Funkcjonalności: 

Logowanie do systemu,

Rejestrację nowego konta,

Przeglądanie aktualnego salda,

Podgląd historii transakcji i informacji o koncie,

Wykonanie przelewu wychodzącego,

Wylogowanie się z aplikacji.
