using System.Diagnostics.Eventing.Reader;

namespace grupoBalPruebaAPI.Models.Response
{
    public class StandarResponse<T>
    {
        public Boolean? estatus { get; set; }
        public string? msg { get; set; }
        public List<T>? datos { get; set; }
        public T? dato { get; set; }
    }
}
