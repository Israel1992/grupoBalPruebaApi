namespace grupoBalPruebaAPI.Models.Request
{
    //Se puede crear anotaciones para validar los elementos de entrada, pero por tiempo no agrege
    public class LoginRequest
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }

    }
}
