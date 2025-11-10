namespace punto_de_venta.Model; 

public record FacturaVenta(
    int id_venta,
    int id_productos,
    int cantidad,
    float valor_unitario,
    DateTime fecha,
    int id_cajero);