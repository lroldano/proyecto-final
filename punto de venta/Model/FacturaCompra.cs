namespace punto_de_venta.Model;

public record FacturaCompra(
    int idCompra, 
    int idProveedor, 
    int idProducto, 
    int cantidad,
    date fecha);