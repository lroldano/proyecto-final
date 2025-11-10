namespace punto_de_venta.Model;

public record Proveedor(
    int idProveedor, 
    string nombre, 
    string codigo,
    string correo,
    string numeroTelefonico);