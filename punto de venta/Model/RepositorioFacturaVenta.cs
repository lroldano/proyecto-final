using System.Data;
using Npgsql;

namespace punto_de_venta.Model;

public class RepositorioFacturaVenta
{
    private readonly SqlConection _connectionProvider;

    public RepositorioFacturaVenta(SqlConection connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }
    
    //Crear
    public void CrearFacturaVenta(int id_productos, int cantidad, float valor_unitario, int id_cajero){
        string query = @"INSERT INTO facturas_ventas VALUES 
                     id_productos = @id_productos, cantidad = @cantidad,  valor_unitario = @valor_unitario, id_cajero = @id_cajero";
        using (var conn = _connectionProvider.GetConnection())
        {
            try
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id_productos", id_productos);
                    cmd.Parameters.AddWithValue("@cantidad", cantidad);
                    cmd.Parameters.AddWithValue("@valor_unitario", valor_unitario);
                    cmd.Parameters.AddWithValue("@id_cajero", id_cajero);
                    
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
    public List<FacturaVenta> BuscarVentaFecha(DateTime fecha)
    {
        var ventas = new List<FacturaVenta>();

        string sql = "SELECT id_venta , id_producto, cantidad, valor_unitario, fecha, id_cajero FROM facturas_compras WHERE fecha = @fecha";

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
                            var venta = new FacturaVenta(
                                reader.GetInt32(reader.GetInt32("id_venta")),
                                reader.GetInt32(reader.GetOrdinal("id_producto")),
                                reader.GetInt32(reader.GetOrdinal("cantidad")),
                                reader.GetFloat(reader.GetOrdinal("valor_unitario")),
                                reader.GetDateTime(reader.GetOrdinal("fecha")),
                                reader.GetInt32(reader.GetOrdinal("id_cajero"))
                            );
                            ventas.Add(venta);
                        }
                    }
                }
            }
            catch (PostgresException e)
            {
                Console.WriteLine("Error en el SELECT de Ventas", e.Message);
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error en el metodo SELECT de Ventas", e.Message);
                throw;
            }
        }
        return ventas;
    }
    
    
    //Actualizar 
    void ActualizarFacturaVenta(int id_venta, int id_productos, int cantidad, float valor_unitario, DateTime fecha, int id_cajero)
    { 
        string query = @"UPDATE facturas_compras SET 
                     id_compra = @id_venta, id_producto = @id_producto, cantidad = @cantidad, valor_unitario = @valor_unitario,fecha = @fecha, id_cajero = @id_cajero      
                     WHERE id_venta = @id_venta";
        using (var conn = _connectionProvider.GetConnection())
        {
            try
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id_venta", id_venta);
                    cmd.Parameters.AddWithValue("@id_producto", id_productos);
                    cmd.Parameters.AddWithValue("@cantidad", cantidad);
                    cmd.Parameters.AddWithValue("@valor_unitario", valor_unitario);
                    cmd.Parameters.AddWithValue("@fecha", fecha);
                    cmd.Parameters.AddWithValue("@id_cajero", id_cajero);

                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if (filasAfectadas == 0)
                    {
                        // Exception por si no se hallan coincidencias con el id
                        throw new KeyNotFoundException(
                            $"No se encontró la factura con ID {id_venta} para actualizar.");
                    }
                }
            }
            catch (PostgresException e)
            {
                Console.WriteLine("Error en el metodo en el UPPDATE de factura ventas", e.Message);
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error en el metodo UPDATE de factura ventas", e.Message);
                throw;
            }
        }
    }
    
    //Eliminar
    public void EliminarFacturaVenta(int idFacturaVenta)
    {
        string query  = @"DELETE FROM facturas_ventas WHERE id_compra = @id";
        
        
        using (var conn = _connectionProvider.GetConnection())
        {
            try
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idVenta", idFacturaVenta);
                    
                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if (filasAfectadas != 1)
                    {
                        // Exception solo por seguridad
                        throw new KeyNotFoundException($"El producto con ID {idFacturaVenta} no fue encontrado para eliminar.");
                    }
                }
            }
            catch (PostgresException e)
            {
                Console.WriteLine("Error en el metodo en el DELETE de Ventas", e);
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error en el metodo DELETE de Ventas", e.Message);
                throw;
            }
        }
    }
}