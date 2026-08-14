using MySqlConnector;

namespace CadastroCliente.Infraestrutura.Persistence;
public sealed class DatabaseConnection
{
    private static DatabaseConnection? _instance;
    private static readonly object _lock = new();
    private readonly string _connectionString;

    private DatabaseConnection()
    {
        _connectionString = "Server=localhost;Port=3306;Database=cadastroclientes;Uid=root;Pwd=admin;";
    }
    public static DatabaseConnection Instance
    {
        get
        {
            lock (_lock)
            {
                _instance ??= new DatabaseConnection();
            }
            return _instance;
        }
    }
    public MySqlConnection GetConnection()
    {
        var connection = new MySqlConnection(_connectionString);
        connection.Open();
        return connection;
    }
}