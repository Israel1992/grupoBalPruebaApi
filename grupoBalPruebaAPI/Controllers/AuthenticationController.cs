using grupoBalPruebaAPI.Models.Request;
using grupoBalPruebaAPI.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol.Plugins;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace grupoBalPruebaAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest user)
        {
            if (user is null)
            {
                return BadRequest("Solicitud de usuario no válida.");
            }
            //Para razones de ejemplificación colocamos un usuario directamente
            if (user.UserName == "Admin" && user.Password == "1234567")
            {
                var JWTSecret = ConfigurationManager.AppSetting["JWT:Secret"];
                if (JWTSecret != null) {
                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTSecret));
                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                    var tokeOptions = new JwtSecurityToken(
                        issuer: ConfigurationManager.AppSetting["JWT:ValidIssuer"],
                        audience: ConfigurationManager.AppSetting["JWT:ValidAudience"],
                        claims: new List<Claim>(),
                        expires: DateTime.Now.AddMinutes(6),
                        signingCredentials: signinCredentials
                    );
                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                    return Ok(new JWTTokenResponse { token = tokenString, estatus = true, msg = "Correcto." });
                }
              
            }
            return Unauthorized();
        }
    }
}
