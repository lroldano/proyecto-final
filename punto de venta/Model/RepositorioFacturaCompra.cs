using System.Data;
using Npgsql;

namespace punto_de_venta.Model;

public class RepositorioFacturaCompra
{
    private readonly SqlConection _connectionProvider;

    public RepositorioFacturaCompra(SqlConection connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }
    
    //Crear
    public void CrearFacturaCompra(int id_proveedor, int id_producto, int cantidad, DateOnly fecha){
        string query = @"INSERT INTO facturas_compras VALUES 
                     id_proveedor = @id_proveedor,  id_producto = @id_producto,  cantidad = @cantidad";
        using (var conn = _connectionProvider.GetConnection())
        {
            try
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id_proveedore", id_proveedor);
                    cmd.Parameters.AddWithValue("@id_producto", id_producto);
                    cmd.Parameters.AddWithValue("@cantidad", cantidad);
                    
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
                Console.WriteLine("Error en el metodo en el CREATE de ventas", e);
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error en el metodo CREATE de ventas", e.Message);
                throw;
            }
        }
    }
    
    
    //Busqueda
    public List<FacturaCompra> BuscarVentaFecha(DateTime fecha)
    {
        var compras = new List<FacturaCompra>();

        string sql = "SELECT id_compra, id_proveedor, id_producto, cantidad, fecha FROM facturas_compras WHERE fecha = @fecha";

        using (var conn = _connectionProvider.GetConnection())
        {
            try
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("fecha", fecha);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var compra = new FacturaCompra(
                                reader.GetInt32(reader.GetOrdinal("idCompra")),
                                reader.GetInt32(reader.GetOrdinal("idProveedor")),
                                reader.GetInt32(reader.GetOrdinal("idProducto")),
                                reader.GetInt32(reader.GetOrdinal("cantidad")),
                                reader.GetDateTime(reader.GetOrdinal("fecha"))
                            );
                            compras.Add(compra);
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
        return compras;
    }
    
    
    //Actualizar 
    void ActualizarFacturaCompra(int id_compra, int id_proveedor, int id_producto, int cantidad, DateOnly fecha)
    { 
        string query = @"UPDATE facturas_compras SET 
                     id_compra = @id_compra, id_proveedor = @id_proveedor, id_producto = @id_producto, cantidad = @cantidad, fecha = @fecha,      
                     WHERE id_compra = @id_compra";
        using (var conn = _connectionProvider.GetConnection())
        {
            try
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id_compra", id_compra);
                    cmd.Parameters.AddWithValue("@id_proveedor", id_compra);
                    cmd.Parameters.AddWithValue("@id_producto", id_compra);
                    cmd.Parameters.AddWithValue("@cantidad", cantidad);
                    cmd.Parameters.AddWithValue("@fecha", fecha);

                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if (filasAfectadas == 0)
                    {
                        // Exception por si no se hallan coincidencias con el id
                        throw new KeyNotFoundException(
                            $"No se encontró la factura con ID {id_compra} para actualizar.");
                    }
                }
            }
            catch (PostgresException e)
            {
                Console.WriteLine("Error en el metodo en el UPPDATE de factura compras", e.Message);
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error en el metodo UPDATE de factura compras", e.Message);
                throw;
            }
        }
    }
    
    //Eliminar
    public void EliminarFacturaCompra(int idFacturaCompra)
    {
        string query  = @"DELETE FROM facturas_compras WHERE id_compra = @idCompra";
        
        
        using (var conn = _connectionProvider.GetConnection())
        {
            try
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idCompra", idFacturaCompra);
                    
                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if (filasAfectadas != 1)
                    {
                        // Exception solo por seguridad
                        throw new KeyNotFoundException($"El producto con ID {idFacturaCompra} no fue encontrado para eliminar.");
                    }
                }
            }
            catch (PostgresException e)
            {
                Console.WriteLine("Error en el metodo en el DELETE de Compras", e);
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error en el metodo DELETE de Compras", e.Message);
                throw;
            }
        }
    }
    
    
    
    
    
    
}