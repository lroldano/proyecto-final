using Npgsql;

namespace punto_de_venta.Model;

public class RepositorioProveedor
{

    private readonly SqlConection _connectionProvider;

    public RepositorioProveedor(SqlConection connectionProvider){
        _connectionProvider = connectionProvider;
    }

        //Busqueda
        public List<Proveedor> BuscarProveedorPorNombre(string nombre)
        {
            var proveedores = new List<Proveedor>();

            string sql = "SELECT idProveedor, nombre, codigo, correo, numeroTelefonico FROM proveedores WHERE nombre LIKE @nombre";

            using (var conn = _connectionProvider.GetConnection())
            {
                try
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("nombre", nombre);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var proveedor = new Proveedor(
                                    reader.GetInt32(reader.GetOrdinal("idProveedor")),
                                    reader.GetString(reader.GetOrdinal("nombre")),
                                    reader.GetString(reader.GetOrdinal("codigo")),
                                    reader.GetString(reader.GetOrdinal("correo")),
                                    reader.GetString(reader.GetOrdinal("numeroTelefonico"))
                                    );
                                proveedores.Add(proveedor);
                            }
                        }
                    }
                }
                catch (PostgresException e)
                {
                    Console.WriteLine("Error en el SELECT de proveedores", e.Message);
                    throw;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error en el metodo SELECT de proveedores", e.Message);
                    throw;
                }
            }
            return proveedores;
        }

        //Actualizar
        void ActualizarProveedor(int idProveedor, string nombre, string codigo, string correo, string numeroTelefonico )
        { 
            string query = @"UPDATE productos SET 
                     nombre = @nombre, codigo = @codigo, correo = @correo, numeroTelefonico = @numeroTelefonico,      
                     WHERE idProveedor = @idProveedor";
            using (var conn = _connectionProvider.GetConnection())
            {
                try
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@nombre", nombre);
                        cmd.Parameters.AddWithValue("@codigo", codigo);
                        cmd.Parameters.AddWithValue("@correo", correo);
                        cmd.Parameters.AddWithValue("@numeroTelefonico", numeroTelefonico);

                        int filasAfectadas = cmd.ExecuteNonQuery();

                        if (filasAfectadas == 0)
                        {
                            // Exception por si no se hallan coincidencias con el id
                            throw new KeyNotFoundException(
                                $"No se encontró el proveedor con ID {idProveedor} para actualizar.");
                        }
                    }
                }
                catch (PostgresException e)
                {
                    Console.WriteLine("Error en el metodo en el UPPDATE de proveedor", e.Message);
                    throw;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error en el metodo UPDATE de proveedor", e.Message);
                    throw;
                }
            }
        }

        
        //Crear
        public void CrearProveedor(string nombre, string codigo, string correo, string numeroTelefonico){
            string query = @"INSERT INTO proveedores VALUES 
                     nombre = @nombre, codigo = @codigo, correo = @correo, numeroTelefonico = @numeroTelefonico";
            using (var conn = _connectionProvider.GetConnection())
            {
                try
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@nombre", nombre);
                        cmd.Parameters.AddWithValue("@codigo", codigo);
                        cmd.Parameters.AddWithValue("@correo", correo);
                        cmd.Parameters.AddWithValue("@numeroTelefonico", numeroTelefonico);
                    
                        int filasAfectadas = cmd.ExecuteNonQuery();

                        if (filasAfectadas != 1)
                        {
                            // Exception solo por seguridad
                            throw new InvalidOperationException("La operación INSERT no afectó exactamente una fila.");
                        }
                    }
                }
                catch (PostgresException e)
                {
                    Console.WriteLine("Error en el metodo en el CREATE de proveedores", e);
                    throw;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error en el metodo CREATE de proveedores", e.Message);
                    throw;
                }
            }
        }

        //Eliminar
        public void EliminarProveedor(int idProveedor)
        {
            string query  = @"DELETE FROM proveedores WHERE id_proveedor = @idProveedor";
        
        
            using (var conn = _connectionProvider.GetConnection())
            {
                try
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@idProveedor", idProveedor);
                    
                        int filasAfectadas = cmd.ExecuteNonQuery();

                        if (filasAfectadas != 1)
                        {
                            // Exception solo por seguridad
                            throw new KeyNotFoundException($"El producto con ID {idProveedor} no fue encontrado para eliminar.");
                        }
                    }
                }
                catch (PostgresException e)
                {
                    Console.WriteLine("Error en el metodo en el DELETE de Proveedores", e);
                    throw;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error en el metodo DELETE de Proveedores", e.Message);
                    throw;
                }
            }
        }

}