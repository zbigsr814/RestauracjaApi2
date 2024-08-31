Projekt strony internetowej wykonanej w ASP.NET.  Strona tworzy endpointy dla operacji CRUD encji restaurant i dishes. Dodano autentykację i autoryzację użytkowników.

Wykorzystano takie elementy jak:
- Entity Framework do zarządzania bazą danych
- LINQ dla operacji zarządzania BD
- NLog króry loguje informacje działania aplikacji do pliku
- Automapper do mapowania encji BD do klas DTO
- Autentykacje użytkowników z wykorzystaniem tokenu JWT oraz autoryzacje dostępu do akcji kontrolera
- Middleware dla wyłapywania wyjąków działania aplikacji

Aby strona zadziałała należy w Package Manager Console wpisać update-database. Komenda utworzy lokalną bazę danych. Seedowanie bazy danych wykonuje się podczas pierwszego uruchomienia programu (profil RestauracjaApi2) w pliku Restaurant2Db.
