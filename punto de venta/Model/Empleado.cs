namespace punto_de_venta.Model;

public record Empleado(
    int idEmpleado,
    bool esSupervisor,
    string nombreCompleto,
    string correo,
    string numeroTelefonico);