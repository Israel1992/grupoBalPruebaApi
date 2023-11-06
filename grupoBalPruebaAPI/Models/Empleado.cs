using System;
using System.Collections.Generic;

namespace grupoBalPruebaAPI.Models;

public partial class Empleado
{
    public int IdEmpleado { get; set; }

    public string? Nombre { get; set; }

    public string? Correo { get; set; }

    public string? Telefono { get; set; }

    public int IdArea { get; set; }

    public virtual Area IdAreaNavigation { get; set; } = null!;
}
