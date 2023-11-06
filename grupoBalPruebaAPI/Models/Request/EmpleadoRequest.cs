namespace grupoBalPruebaAPI.Models.Request
{  //Se puede crear anotaciones para validar los elementos de entrada, pero por tiempo no agrege
    public class EmpleadoRequest
    {

        public int IdEmpleado { get; set; }

        public string? Nombre { get; set; }

        public string? Correo { get; set; }

        public string? Telefono { get; set; }

        public int IdArea { get; set; }

    }
}
