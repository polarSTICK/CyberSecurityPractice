using System.Data.SQLite;
using System.Diagnostics;
using Shared;

namespace IdentityProvider;

public class Database
{
    private const string DatabaseName = "UserDb.sqlite";
    private const string ConnectionString = $"Data Source={DatabaseName};Version=3;";
    
    private SQLiteConnection _connection;
    
    public Database()
    {
        InitializeDb();
    }

    public void AddUser(string userId, UserRoles role, string pw)
    {
        try
        {
            _connection.Open();
        
            string hashedPw = Cryptography.GetHashString(pw);

            var insertSql = $"""
                             INSERT OR IGNORE INTO Users (UserId, RoleId, Password)
                             VALUES ('{userId}', {(int)role}, '{hashedPw}');
                             """;
        
            SQLiteCommand insertCommand = new SQLiteCommand(insertSql, _connection);
            insertCommand.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            _connection.Close();
        }
    }

    // looking for user with salt
    /*public bool UserExists(string userId, string pwHash)
    {
        _connection.Open();
        
        var userSaltSql = $"SELECT Salt FROM Users WHERE UserId = '{userId}'";

        SQLiteCommand userSaltCommand = new SQLiteCommand(userSaltSql, _connection);
        var userSalt = (string?)userSaltCommand.ExecuteScalar();

        if (userSalt == null)
        {
            Debug.WriteLine("User not found");
            _connection.Close();
            return false;
        }

        Debug.WriteLine("User found");

        var saltedHashedPw =  Cryptography.GetHashString(userSalt + pwHash);
        
        var existsSql = $"SELECT EXISTS(SELECT 1 FROM Users WHERE UserId = '{userId}' AND Password = '{saltedHashedPw}');";
        
        SQLiteCommand existsCommand = new SQLiteCommand(existsSql, _connection);
        var exists = Convert.ToBoolean((long)existsCommand.ExecuteScalar());
        
        Debug.WriteLine(exists ? "User match" : "No user match");
        
        _connection.Close();

        return exists;
    } */
    
    public bool UserExists(string userId, string pwHash)
    {
        _connection.Open();
        
        var existsSql = $"SELECT EXISTS(SELECT 1 FROM Users WHERE UserId = '{userId}' AND Password = '{pwHash}');";
        
        SQLiteCommand existsCommand = new SQLiteCommand(existsSql, _connection);
        var exists = Convert.ToBoolean((long)existsCommand.ExecuteScalar());
        
        Debug.WriteLine(exists ? "User match" : "No user match");
        
        _connection.Close();

        return exists;
    }

    private void InitializeDb()
    {
        SQLiteConnection.CreateFile(DatabaseName);
        _connection = new SQLiteConnection(ConnectionString);
        
        CreateUserTable();
    }

    private void CreateUserTable()
    {
        _connection.Open();
        
        const string createSql = """
                                 CREATE TABLE IF NOT EXISTS Users (
                                 ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                 UserId TEXT NOT NULL,
                                 RoleId INTEGER NOT NULL,
                                 Password TEXT NOT NULL,
                                 UNIQUE(UserId));
                                 """;
        SQLiteCommand createCommand = new SQLiteCommand(createSql, _connection);
        createCommand.ExecuteNonQuery();
        
        _connection.Close();
        
        // TODO: Dont attempt to add these at every start of the program
        // Add default users.
        // Ideally PW would be coming from somewhere else, not hardcoded.
        AddUser("admin", UserRoles.Admin, "admin");
        AddUser("user", UserRoles.User, "user");
    }
}