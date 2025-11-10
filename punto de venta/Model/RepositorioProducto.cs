using Npgsql;

namespace punto_de_venta.Model;

public class RepositorioProducto
{
    private readonly SqlConection _connectionProvider;
    
    public RepositorioProducto(SqlConection connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }
    
    public List<Producto> BuscarProductosPorNombre(string nombre)
    {
        var productos = new List<Producto>();

        string sql = "SELECT idProducto, nombre, cantidad, precioBase, valorIva, precioTotal, descuento, idProveedor FROM producto WHERE nombre LIKE @nombre";

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
                            var producto = new Producto(
                                reader.GetInt32(reader.GetOrdinal("idProducto")),
                                reader.GetString(reader.GetOrdinal("nombre")),
                                reader.GetInt32(reader.GetOrdinal("cantidad")),
                                reader.GetFloat(reader.GetOrdinal("precioBase")),
                                reader.GetInt32(reader.GetOrdinal("valorIva")),
                                reader.GetFloat(reader.GetOrdinal("precioTotal")),
                                reader.GetInt32(reader.GetOrdinal("descuento")),
                                reader.GetInt32(reader.GetOrdinal("idProveedor"))
                            );
                            productos.Add(producto);
                        }
                    }
                }
            }
            catch (PostgresException e)
            {
                Console.WriteLine("Error en el SELECT en el UPPDATE de productos", e.Message);
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error en el metodo SELECT de productos", e.Message);
                throw;
            }
        } 
        return productos;
    }

        void ActualizarProducto(int idProducto, string nombre, int cantidad, float precioBase, int valorIva, float precioTotal, int descuento, int idProveedor)
    {
        string query = @"UPDATE productos SET 
                     nombre = @nombre, cantidad = @cantidad, precio_base = @precioBase, valor_iva = @valorIva, valor_total = @precioTotal, descuento = @descuento, id_proveedor = @idProveedor
                 WHERE idProducto = @idProducto";
        using (var conn = _connectionProvider.GetConnection())
        {
            try
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    cmd.Parameters.AddWithValue("@cantidad", cantidad);
                    cmd.Parameters.AddWithValue("@precioBase", precioBase);
                    cmd.Parameters.AddWithValue("@valorIva", valorIva);
                    cmd.Parameters.AddWithValue("@precioTotal", precioTotal);
                    cmd.Parameters.AddWithValue("@descuento", descuento);
                    cmd.Parameters.AddWithValue("@idProveedor", idProveedor);

                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if (filasAfectadas == 0)
                    {
                        // Exception por si no se hallan coincidencias con el id
                        throw new KeyNotFoundException(
                            $"No se encontró el producto con ID {idProducto} para actualizar.");
                    }
                }
            }
            catch (PostgresException e)
            {
                Console.WriteLine("Error en el metodo en el UPPDATE de productos", e.Message);
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error en el metodo UPDATE de productos", e.Message);
                throw;
            }
        }
    }
    
    
    public void CrearProducto(string nombre, int cantidad, float precioBase, int valorIva, float precioTotal, int descuento, int idProveedor)
    {
        string query = @"INSERT INTO productos VALUES 
                     nombre = @nombre, cantidad = @cantidad, precio_base = @precioBase, valor_iva = @valorIva, valor_total = @precioTotal, descuento = @descuento, id_proveedor = @idProveedor";
        using (var conn = _connectionProvider.GetConnection())
        {
            try
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    cmd.Parameters.AddWithValue("@cantidad", cantidad);
                    cmd.Parameters.AddWithValue("@precioBase", precioBase);
                    cmd.Parameters.AddWithValue("@valorIva", valorIva);
                    cmd.Parameters.AddWithValue("@precioTotal", precioTotal);
                    cmd.Parameters.AddWithValue("@descuento", descuento);
                    cmd.Parameters.AddWithValue("@idProveedor", idProveedor);
                    
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
                Console.WriteLine("Error en el metodo en el CREATE de productos", e);
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error en el metodo CREATE de productos", e.Message);
                throw;
            }
        }
    }
    
    
    public void EliminarProducto(int idProducto)
    {
        string query  = @"DELETE FROM productos WHERE id_producto = @idProducto";
        
        
        using (var conn = _connectionProvider.GetConnection())
        {
            try
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idProducto", idProducto);
                    
                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if (filasAfectadas != 1)
                    {
                        // Exception solo por seguridad
                        throw new KeyNotFoundException($"El producto con ID {idProducto} no fue encontrado para eliminar.");
                    }
                }
            }
            catch (PostgresException e)
            {
                Console.WriteLine("Error en el metodo en el DELETE de productos", e);
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error en el metodo DELETE de productos", e.Message);
                throw;
            }
        }
    }
}