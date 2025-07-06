using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace KoKaBank
{
    public class DatabaseHelper
    {
        private static string connectionString = "Data Source=KoKaBank.db;Version=3;";

        // Inicjalizacja bazy danych
        public static void InitializeDatabase()
        {
            if (!File.Exists("KoKaBank.db"))
            {
                SQLiteConnection.CreateFile("KoKaBank.db");
            }

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Tabela użytkowników
                string createUsersTable = @"
                    CREATE TABLE IF NOT EXISTS Users (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        ClientId TEXT UNIQUE NOT NULL,
                        Password TEXT NOT NULL,
                        FullName TEXT NOT NULL,
                        Nationality TEXT,
                        Pesel TEXT,
                        IdNumber TEXT,
                        Balance DECIMAL DEFAULT 0,
                        CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP
                    )";

                // Tabela transakcji
                string createTransactionsTable = @"
                    CREATE TABLE IF NOT EXISTS Transactions (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        ClientId TEXT NOT NULL,
                        Date DATETIME NOT NULL,
                        Description TEXT NOT NULL,
                        Amount DECIMAL NOT NULL,
                        BalanceAfter DECIMAL NOT NULL,
                        RecipientClientId TEXT,
                        TransferType TEXT DEFAULT 'external',
                        FOREIGN KEY (ClientId) REFERENCES Users(ClientId)
                    )";

                var command = new SQLiteCommand(createUsersTable, connection);
                command.ExecuteNonQuery();

                command = new SQLiteCommand(createTransactionsTable, connection);
                command.ExecuteNonQuery();

                // Dodaj domyślnego użytkownika jeśli nie istnieje
                AddDefaultUser(connection);
            }
        }

        private static void AddDefaultUser(SQLiteConnection connection)
        {
            string checkUser = "SELECT COUNT(*) FROM Users WHERE ClientId = '123456'";
            var checkCommand = new SQLiteCommand(checkUser, connection);
            int userExists = Convert.ToInt32(checkCommand.ExecuteScalar());

            if (userExists == 0)
            {
                string insertUser = @"
                    INSERT INTO Users (ClientId, Password, FullName, Balance) 
                    VALUES ('123456', 'admin', 'WSB Merito', 5000.00)";
                var insertCommand = new SQLiteCommand(insertUser, connection);
                insertCommand.ExecuteNonQuery();

                // Dodaj przykładowe transakcje
                AddSampleTransactions(connection, "123456");
            }
        }

        private static void AddSampleTransactions(SQLiteConnection connection, string clientId)
        {
            var transactions = new[]
            {
                new { Date = "2025-04-01", Description = "Wpłata własna", Amount = 5000m, Balance = 5000m },
                new { Date = "2025-04-03", Description = "Przelew do Jan Kowalski", Amount = -120m, Balance = 4880m },
                new { Date = "2025-04-05", Description = "Zakupy Allegro", Amount = -350.5m, Balance = 4529.5m },
                new { Date = "2025-04-07", Description = "Przelew od Anna Nowak", Amount = 250m, Balance = 4779.5m }
            };

            foreach (var transaction in transactions)
            {
                string insertTransaction = @"
                    INSERT INTO Transactions (ClientId, Date, Description, Amount, BalanceAfter, TransferType) 
                    VALUES (@clientId, @date, @description, @amount, @balanceAfter, 'external')";

                var command = new SQLiteCommand(insertTransaction, connection);
                command.Parameters.AddWithValue("@clientId", clientId);
                command.Parameters.AddWithValue("@date", transaction.Date);
                command.Parameters.AddWithValue("@description", transaction.Description);
                command.Parameters.AddWithValue("@amount", transaction.Amount);
                command.Parameters.AddWithValue("@balanceAfter", transaction.Balance);
                command.ExecuteNonQuery();
            }
        }

        // Rejestracja użytkownika
        public static bool RegisterUser(string clientId, string password, string fullName, string nationality, string pesel, string idNumber)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string insertUser = @"
                        INSERT INTO Users (ClientId, Password, FullName, Nationality, Pesel, IdNumber, Balance) 
                        VALUES (@clientId, @password, @fullName, @nationality, @pesel, @idNumber, 1000)";

                    var command = new SQLiteCommand(insertUser, connection);
                    command.Parameters.AddWithValue("@clientId", clientId);
                    command.Parameters.AddWithValue("@password", password);
                    command.Parameters.AddWithValue("@fullName", fullName);
                    command.Parameters.AddWithValue("@nationality", nationality);
                    command.Parameters.AddWithValue("@pesel", pesel);
                    command.Parameters.AddWithValue("@idNumber", idNumber);

                    command.ExecuteNonQuery();

                    // Dodaj transakcję wpłaty powitalnej
                    AddTransaction(clientId, "Wpłata powitalna studencie Merito", 1000m, 1000m, null, "system");

                    return true;
                }
            }
            catch (SQLiteException)
            {
                return false; // Użytkownik już istnieje lub inny błąd
            }
        }

        // Logowanie użytkownika
        public static bool ValidateUser(string clientId, string password)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Users WHERE ClientId = @clientId AND Password = @password";
                var command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@clientId", clientId);
                command.Parameters.AddWithValue("@password", password);

                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
        }

        // Pobierz saldo użytkownika
        public static decimal GetUserBalance(string clientId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Balance FROM Users WHERE ClientId = @clientId";
                var command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@clientId", clientId);

                var result = command.ExecuteScalar();
                return result != null ? Convert.ToDecimal(result) : 0;
            }
        }

        // Pobierz transakcje użytkownika
        public static List<BankTransaction> GetUserTransactions(string clientId)
        {
            var transactions = new List<BankTransaction>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Date, Description, Amount, BalanceAfter FROM Transactions WHERE ClientId = @clientId ORDER BY Date DESC";
                var command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@clientId", clientId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        transactions.Add(new BankTransaction
                        {
                            Date = Convert.ToDateTime(reader["Date"]),
                            Description = reader["Description"].ToString(),
                            Amount = Convert.ToDecimal(reader["Amount"]),
                            BalanceAfter = Convert.ToDecimal(reader["BalanceAfter"])
                        });
                    }
                }
            }

            return transactions;
        }

        // Sprawdź czy użytkownik istnieje (po ID)
        public static bool UserExists(string clientId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Users WHERE ClientId = @clientId";
                var command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@clientId", clientId);

                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
        }

        // Pobierz nazwę użytkownika
        public static string GetUserName(string clientId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT FullName FROM Users WHERE ClientId = @clientId";
                var command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@clientId", clientId);

                var result = command.ExecuteScalar();
                return result?.ToString() ?? "";
            }
        }

        // NOWA FUNKCJA: Przelew wewnętrzny między użytkownikami
        public static TransferResult ProcessInternalTransfer(string fromClientId, string toClientId, decimal amount, string description)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Sprawdź czy odbiorca istnieje
                        if (!UserExists(toClientId))
                        {
                            return new TransferResult { Success = false, Message = "Odbiorca o podanym numerze klienta nie istnieje." };
                        }

                        // Pobierz aktualne salda
                        decimal fromBalance = GetUserBalance(fromClientId);
                        decimal toBalance = GetUserBalance(toClientId);

                        // Sprawdź czy nadawca ma wystarczające środki
                        if (fromBalance < amount)
                        {
                            return new TransferResult { Success = false, Message = "Niewystarczające środki na koncie." };
                        }

                        // Oblicz nowe salda
                        decimal newFromBalance = fromBalance - amount;
                        decimal newToBalance = toBalance + amount;

                        // Aktualizuj saldo nadawcy
                        string updateFromBalance = "UPDATE Users SET Balance = @balance WHERE ClientId = @clientId";
                        var updateFromCommand = new SQLiteCommand(updateFromBalance, connection);
                        updateFromCommand.Parameters.AddWithValue("@balance", newFromBalance);
                        updateFromCommand.Parameters.AddWithValue("@clientId", fromClientId);
                        updateFromCommand.ExecuteNonQuery();

                        // Aktualizuj saldo odbiorcy
                        string updateToBalance = "UPDATE Users SET Balance = @balance WHERE ClientId = @clientId";
                        var updateToCommand = new SQLiteCommand(updateToBalance, connection);
                        updateToCommand.Parameters.AddWithValue("@balance", newToBalance);
                        updateToCommand.Parameters.AddWithValue("@clientId", toClientId);
                        updateToCommand.ExecuteNonQuery();

                        // Pobierz nazwę odbiorcy
                        string recipientName = GetUserName(toClientId);
                        string senderName = GetUserName(fromClientId);

                        // Dodaj transakcję dla nadawcy (wydatek)
                        string insertFromTransaction = @"
                            INSERT INTO Transactions (ClientId, Date, Description, Amount, BalanceAfter, RecipientClientId, TransferType) 
                            VALUES (@clientId, @date, @description, @amount, @balanceAfter, @recipientId, 'internal_sent')";
                        var fromCommand = new SQLiteCommand(insertFromTransaction, connection);
                        fromCommand.Parameters.AddWithValue("@clientId", fromClientId);
                        fromCommand.Parameters.AddWithValue("@date", DateTime.Now);
                        fromCommand.Parameters.AddWithValue("@description", $"Przelew do {recipientName} ({toClientId}): {description}");
                        fromCommand.Parameters.AddWithValue("@amount", -amount);
                        fromCommand.Parameters.AddWithValue("@balanceAfter", newFromBalance);
                        fromCommand.Parameters.AddWithValue("@recipientId", toClientId);
                        fromCommand.ExecuteNonQuery();

                        // Dodaj transakcję dla odbiorcy (wpływ)
                        string insertToTransaction = @"
                            INSERT INTO Transactions (ClientId, Date, Description, Amount, BalanceAfter, RecipientClientId, TransferType) 
                            VALUES (@clientId, @date, @description, @amount, @balanceAfter, @senderId, 'internal_received')";
                        var toCommand = new SQLiteCommand(insertToTransaction, connection);
                        toCommand.Parameters.AddWithValue("@clientId", toClientId);
                        toCommand.Parameters.AddWithValue("@date", DateTime.Now);
                        toCommand.Parameters.AddWithValue("@description", $"Przelew od {senderName} ({fromClientId}): {description}");
                        toCommand.Parameters.AddWithValue("@amount", amount);
                        toCommand.Parameters.AddWithValue("@balanceAfter", newToBalance);
                        toCommand.Parameters.AddWithValue("@senderId", fromClientId);
                        toCommand.ExecuteNonQuery();

                        transaction.Commit();
                        return new TransferResult
                        {
                            Success = true,
                            Message = $"Przelew {amount:N2} PLN do {recipientName} został zrealizowany pomyślnie."
                        };
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return new TransferResult { Success = false, Message = "Wystąpił błąd podczas przelewu: " + ex.Message };
                    }
                }
            }
        }

        // Dodaj transakcję (stara funkcja dla zewnętrznych przelewów)
        public static void AddTransaction(string clientId, string description, decimal amount, decimal newBalance, string recipientClientId = null, string transferType = "external")
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Rozpocznij transakcję bazodanową
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Zaktualizuj saldo użytkownika
                        string updateBalance = "UPDATE Users SET Balance = @balance WHERE ClientId = @clientId";
                        var updateCommand = new SQLiteCommand(updateBalance, connection);
                        updateCommand.Parameters.AddWithValue("@balance", newBalance);
                        updateCommand.Parameters.AddWithValue("@clientId", clientId);
                        updateCommand.ExecuteNonQuery();

                        // Dodaj transakcję
                        string insertTransaction = @"
                            INSERT INTO Transactions (ClientId, Date, Description, Amount, BalanceAfter, RecipientClientId, TransferType) 
                            VALUES (@clientId, @date, @description, @amount, @balanceAfter, @recipientId, @transferType)";
                        var insertCommand = new SQLiteCommand(insertTransaction, connection);
                        insertCommand.Parameters.AddWithValue("@clientId", clientId);
                        insertCommand.Parameters.AddWithValue("@date", DateTime.Now);
                        insertCommand.Parameters.AddWithValue("@description", description);
                        insertCommand.Parameters.AddWithValue("@amount", amount);
                        insertCommand.Parameters.AddWithValue("@balanceAfter", newBalance);
                        insertCommand.Parameters.AddWithValue("@recipientId", recipientClientId);
                        insertCommand.Parameters.AddWithValue("@transferType", transferType);
                        insertCommand.ExecuteNonQuery();

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        // Zmień hasło
        public static bool ChangePassword(string clientId, string newPassword)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string updatePassword = "UPDATE Users SET Password = @password WHERE ClientId = @clientId";
                    var command = new SQLiteCommand(updatePassword, connection);
                    command.Parameters.AddWithValue("@password", newPassword);
                    command.Parameters.AddWithValue("@clientId", clientId);

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        // Pobierz informacje o koncie
        public static AccountInfo GetAccountInfo(string clientId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"SELECT ClientId, FullName, Nationality, Pesel, IdNumber, Balance, CreatedDate 
                               FROM Users WHERE ClientId = @clientId";
                var command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@clientId", clientId);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new AccountInfo
                        {
                            ClientId = reader["ClientId"].ToString(),
                            FullName = reader["FullName"].ToString(),
                            Nationality = reader["Nationality"].ToString(),
                            Pesel = reader["Pesel"].ToString(),
                            IdNumber = reader["IdNumber"].ToString(),
                            Balance = Convert.ToDecimal(reader["Balance"]),
                            CreatedDate = Convert.ToDateTime(reader["CreatedDate"])
                        };
                    }
                }
            }
            return null;
        }

        // Pobierz listę użytkowników (do podpowiedzi)
        public static List<UserSummary> GetAllUsers()
        {
            var users = new List<UserSummary>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT ClientId, FullName FROM Users ORDER BY FullName";
                var command = new SQLiteCommand(query, connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new UserSummary
                        {
                            ClientId = reader["ClientId"].ToString(),
                            FullName = reader["FullName"].ToString()
                        });
                    }
                }
            }
            return users;
        }
    }

    // Klasa wyniku przelewu
    public class TransferResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    // Klasa do przechowywania informacji o koncie
    public class AccountInfo
    {
        public string ClientId { get; set; }
        public string FullName { get; set; }
        public string Nationality { get; set; }
        public string Pesel { get; set; }
        public string IdNumber { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    // Klasa dla listy użytkowników
    public class UserSummary
    {
        public string ClientId { get; set; }
        public string FullName { get; set; }
    }
}