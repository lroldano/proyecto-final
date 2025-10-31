namespace punto_de_venta.Model;

public record Producto(
    int idProducto, 
    string nombre, 
    int cantidad, 
    float precioBase, 
    int valorIva, 
    float precioTotal, 
    int descuento, 
    int idProveedor);