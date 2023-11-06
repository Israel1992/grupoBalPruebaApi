using grupoBalPruebaAPI.Models;
using grupoBalPruebaAPI.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace grupoBalPruebaAPI.Services
{


    public class AreaService
    {
        private readonly GbPruebaContext _context;

        //Mensajes de respuesta
        private String msgCorrecto = "El Area se ha ";
        private String msgExiste = "El Area ya existe";
        private String msgNoExiste = "El Area no existe";

        public AreaService(GbPruebaContext context){_context = context;}

        public async Task<StandarResponse<T>> GetAreas<T>(){
            var response = new StandarResponse<T>();
            try {

                response.datos = await _context.Areas.ToListAsync() as List<T>;
                response.estatus = true;
            }catch (Exception ex){
                response.msg = ex.Message;//IMPORTANTE Cambiar mensaje de execpción por otro por seguridad, mensaje de excepción mejor guardar en bitacora
                response.estatus = false;
            }
            return response;
        }


        public async Task<StandarResponse<T>> GetAreas<T>(int id){
            var response = new StandarResponse<T>();
            try{
                var area = await _context.Areas.FindAsync(id);

                if (area != null){
                    // Usar reflexión para crear una instancia de T y copiar los valores de area
                    var instance = Activator.CreateInstance<T>();
                    foreach (var property in typeof(Area).GetProperties()){
                        var value = property.GetValue(area);
                        typeof(T).GetProperty(property.Name)?.SetValue(instance, value);
                    }

                    response.dato = instance;
                    response.estatus = true;
                }else{
                    response.msg = msgNoExiste; 
                    response.estatus = false;
                }


            }catch (Exception ex){
                response.msg = ex.Message;//IMPORTANTE Cambiar mensaje de execpción por otro por seguridad, mensaje de excepción mejor guardar en bitacora
                response.estatus = false;
            }
            return response;
        }

        public async Task<StandarResponse<T>> PutArea<T>(int id, Area area) {
            const string metodo = "Actualizado";
            var response = new StandarResponse<T>();

            using (var transaction = _context.Database.BeginTransaction()) //Es importante ya que en un proceso más grande se podra rebertir los cambios realizados en base de datos si algo en el proceso sale mal
            {
                try {
                    area.IdArea = id;
                    _context.Entry(area).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    response.msg = msgCorrecto + metodo;
                    response.estatus = true;
                    transaction.Commit();
                   
                }catch (Exception ex){
                    response.msg = ex.Message;//IMPORTANTE Cambiar mensaje de execpción por otro por seguridad, mensaje de excepción mejor guardar en bitacora
                    response.estatus = false;
                    transaction.Rollback();
                }
                return response;
            }
           
        }

        public async Task<StandarResponse<T>> PostArea<T>(Area area){
            const string metodo = "Creado";
            var response = new StandarResponse<T>();
            using (var transaction = _context.Database.BeginTransaction()) //Es importante ya que en un proceso más grande se podra rebertir los cambios realizados en base de datos si algo en el proceso sale mal
            {
                try
                {
                    _context.Areas.Add(area);
                    await _context.SaveChangesAsync();
                    response.estatus = true;
                    response.msg = msgCorrecto + metodo + " con id " + area.IdArea;
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

        public async Task<StandarResponse<T>> DeleteArea<T>(int id){
            const string metodo = "Eliminado";
            var response = new StandarResponse<T>();
            using (var transaction = _context.Database.BeginTransaction()) //Es importante ya que en un proceso más grande se podra rebertir los cambios realizados en base de datos si algo en el proceso sale mal
            {
                try
                {
                    var area = await _context.Areas.FindAsync(id);
                    if (area == null)
                    {
                        response.msg = msgNoExiste;
                        response.estatus = false;
                    }
                    else
                    {
                        _context.Areas.Remove(area);
                        await _context.SaveChangesAsync();
                        response.msg = msgCorrecto + metodo;
                        response.estatus = true;
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    if (!AreaExists(id))
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


        private bool AreaExists(int id)
        {
            return (_context.Areas?.Any(e => e.IdArea == id)).GetValueOrDefault();
        }


    }
}
