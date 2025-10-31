using Npgsql;

namespace punto_de_venta.Model;

public class SqlConection 
{
    private readonly string _connectionString;

    public SqlConection()
    {
        _connectionString = "Host=localhost;Username=postgres;Password=1234;Database=proyecto_final";
    }

    public NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}