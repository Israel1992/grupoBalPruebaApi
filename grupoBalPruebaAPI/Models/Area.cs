using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace grupoBalPruebaAPI.Models;

public partial class Area
{
    public int IdArea { get; set; }
    [Required]
    [MaxLength(80)]
    public string? Descripcion { get; set; }

    public virtual ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();
}
