using Npgsql;

namespace punto_de_venta.Model;

public class RepositorioEmpleado
{

    private readonly SqlConection _connectionProvider;

    public RepositorioEmpleado(SqlConection connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }
    public bool Login(string cedula, string contrasena)
    {
        Boolean sudo;
        string sql = "SELECT esSupervisor FROM empleados WHERE cedula = @cedula AND contrasena = @contrasena";
        using (var conn = _connectionProvider.GetConnection())
        {
            try
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("cedula", cedula);
                    cmd.Parameters.AddWithValue("contrasena", contrasena);
                    using (var reader = cmd.ExecuteReader())
                    {
                       sudo = reader.GetBoolean(reader.GetOrdinal("es_supervisor"));
                    }
                }
            }
            
            catch (Exception )
            {
                Console.WriteLine();
                throw;
            }
            return sudo;
        }
    }
}