using System.Data;
using System.Data.Common;
using System.Text.Json;
using Npgsql;

namespace FirstHomework.DB.Config;

public static class DbLoader
{
    public static NpgsqlConnection? Connection { get; private set; }

    public static void LoadConnection(string dbConfigPath)
    {
        var dbConfig = File.ReadAllText(dbConfigPath);
        var dbModel = JsonSerializer.Deserialize<DbModel>(dbConfig);

        ValidateConfigParsed(dbModel ?? throw new InvalidOperationException());

        Connection ??= new NpgsqlConnection
            ($"Host={dbModel.Host};Port={dbModel.Port};Database={dbModel.DbName};Username={dbModel.User};Password={dbModel.Password}");

        CheckConnection();
    }

    private static void ValidateConfigParsed(DbModel dbModel)
    {
        if (string.IsNullOrEmpty(dbModel.Host))
        {
            throw new ArgumentException("Host is required");
        }
        if (string.IsNullOrEmpty(dbModel.Port))
        {
            throw new ArgumentException("Port is required");
        }
        if (string.IsNullOrEmpty(dbModel.DbName))
        {
            throw new ArgumentException("DbName is required");
        }
        if (string.IsNullOrEmpty(dbModel.User))
        {
            throw new ArgumentException("User is required");
        }
        if (string.IsNullOrEmpty(dbModel.Password))
        {
            throw new ArgumentException("Password is required");
        }
    }

    private static void CheckConnection()
    {
        if (Connection == null)
        {
            throw new InvalidOperationException("Connection is not initialized");
        }

        if (Connection.State == ConnectionState.Closed)
        {
            Connection.Open();
        }

        //close db connection
        if (Connection.State == ConnectionState.Open)
        {
            Connection.Close();
        }
    }
}