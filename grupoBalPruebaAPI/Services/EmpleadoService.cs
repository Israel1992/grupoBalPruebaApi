
using grupoBalPruebaAPI.Models;
using grupoBalPruebaAPI.Models.Request;
using grupoBalPruebaAPI.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace grupoBalPruebaAPI.Services
{
    public class EmpleadoService
    {
        private readonly GbPruebaContext _context;

        //Mensajes de respuesta
        private String msgCorrecto = "El Empleado se ha ";
        private String msgExiste = "El Empleado ya existe";
        private String msgNoExiste = "El Empleado no existe";

        public EmpleadoService(GbPruebaContext context) { _context = context; }


        public async Task<StandarResponse<T>> GetEmpleados<T>()
        {
            var response = new StandarResponse<T>();
            try
            {
                var empleados = await _context.Empleados.Include(e => e.IdAreaNavigation) // Incluye la relación con la entidad Area
                .ToListAsync();

                List<T> datos = empleados.Cast<T>().ToList();
                response.datos = datos;
                response.estatus = true;
            }
            catch (Exception ex)
            {
                response.msg = ex.Message;//IMPORTANTE Cambiar mensaje de execpción por otro por seguridad, mensaje de excepción mejor guardar en bitacora
                response.estatus = false;
            }
            return response;
        }


        public async Task<StandarResponse<T>> GetEmpleado<T>(int id)
        {
            var response = new StandarResponse<T>();
            try
            {
                var empleado = await _context.Empleados.FindAsync(id);

                if (empleado != null)
                {
                    // Usar reflexión para crear una instancia de T y copiar los valores de empleado
                    var instance = Activator.CreateInstance<T>();
                    foreach (var property in typeof(Empleado).GetProperties())
                    {
                        var value = property.GetValue(empleado);
                        typeof(T).GetProperty(property.Name)?.SetValue(instance, value);
                    }

                    response.dato = instance;
                    response.estatus = true;
                }
                else
                {
                    response.msg = msgNoExiste;
                    response.estatus = false;
                }


            }
            catch (Exception ex)
            {
                response.msg = ex.Message;//IMPORTANTE Cambiar mensaje de execpción por otro por seguridad, mensaje de excepción mejor guardar en bitacora
                response.estatus = false;
            }
            return response;
        }

        public async Task<StandarResponse<T>> PutEmpleado<T>(int id, EmpleadoRequest empleado)
        {
            const string metodo = "Actualizado";
            var response = new StandarResponse<T>();

            using (var transaction = _context.Database.BeginTransaction()) //Es importante ya que en un proceso más grande se podra rebertir los cambios realizados en base de datos si algo en el proceso sale mal
            {
                try{

                    Empleado e = new Empleado();
                    e.IdEmpleado = id;
                    e.Nombre = empleado.Nombre;
                    e.Correo = empleado.Correo;
                    e.Telefono = empleado.Telefono;
                    e.IdArea = empleado.IdArea;

                    _context.Entry(e).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    response.msg = msgCorrecto + metodo;
                    response.estatus = true;
                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    response.msg = ex.Message;//IMPORTANTE Cambiar mensaje de execpción por otro por seguridad, mensaje de excepción mejor guardar en bitacora
                    response.estatus = false;
                    transaction.Rollback();
                }
                return response;
            }

        }

        public async Task<StandarResponse<T>> PostEmpleado<T>(EmpleadoRequest empleado)
        {
            const string metodo = "Creado";
            var response = new StandarResponse<T>();
            using (var transaction = _context.Database.BeginTransaction()) //Es importante ya que en un proceso más grande se podra rebertir los cambios realizados en base de datos si algo en el proceso sale mal
            {
                try
                {
                    Empleado e = new Empleado();

                    e.Nombre = empleado.Nombre;
                    e.Correo = empleado.Correo;
                    e.Telefono = empleado.Telefono;
                    e.IdArea = empleado.IdArea;

                    _context.Empleados.Add(e);
                    await _context.SaveChangesAsync();
                    response.estatus = true;
                    response.msg = msgCorrecto + metodo + " con id " + e.IdEmpleado;
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    response.msg = ex.Message;//IMPORTANTE Cambiar mensaje de execpción por otro por seguridad, mensaje de excepción mejor guardar en bitacora
                    response.estatus = false;
                    transaction.Rollback();

                }
                return response;
            }
        }

        public async Task<StandarResponse<T>> DeleteEmpleado<T>(int id)
        {
            const string metodo = "Eliminado";
            var response = new StandarResponse<T>();
            using (var transaction = _context.Database.BeginTransaction()) //Es importante ya que en un proceso más grande se podra rebertir los cambios realizados en base de datos si algo en el proceso sale mal
            {
                try
                {
                    var empleado = await _context.Empleados.FindAsync(id);
                    if (empleado == null)
                    {
                        response.msg = msgNoExiste;
                        response.estatus = false;
                    }
                    else
                    {
                        _context.Empleados.Remove(empleado);
                        await _context.SaveChangesAsync();
                        response.msg = msgCorrecto + metodo;
                        response.estatus = true;
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    if (!EmpleadoExists(id))
                    {
                        response.msg = msgExiste;
                        response.estatus = false;
                    }
                    else
                    {
                        response.msg = ex.Message;//IMPORTANTE Cambiar mensaje de execpción por otro por seguridad, mensaje de excepción mejor guardar en bitacora
                        response.estatus = false;

                    }
                    transaction.Rollback();
                }
                return response;
            }
        }


        private bool EmpleadoExists(int id)
        {
            return (_context.Empleados?.Any(e => e.IdEmpleado == id)).GetValueOrDefault();
        }

    }
}
