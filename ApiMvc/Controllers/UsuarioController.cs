using ApiMvc.Models;
using ApiMvc.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace ApiMvc.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuarioController : Controller
    {
        [HttpPost]
        [Route("")]
        public IActionResult CreateUsuario([FromServices] IOptions<ConnectionStringOptions> options, Usuario usuario)
        {
            using (SqlConnection connection = new SqlConnection(options.Value.MyConnection))
            {
                connection.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = @"insert into Usuarios (name, cpf, email, password, birthday) values(@name, @cpf, @email, @password, @birthday)";
                command.CommandType = System.Data.CommandType.Text;

                command.Parameters.Add(new SqlParameter("name", usuario.name));
                command.Parameters.Add(new SqlParameter("cpf", usuario.cpf));
                command.Parameters.Add(new SqlParameter("email", usuario.email));
                command.Parameters.Add(new SqlParameter("password", usuario.password));
                command.Parameters.Add(new SqlParameter("birthday", usuario.birthday));

                command.ExecuteNonQuery();

                connection.Close();
            }

            return Ok();
        }

        [HttpGet]
        [Route("{idUsuario}")]
        public IActionResult GetUsuario([FromServices] IOptions<ConnectionStringOptions> options, [FromQuery] int idUsuario)
        {
            Usuario usuario = null;

            using (SqlConnection connection = new(options.Value.MyConnection))
            {
                connection.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = @"select * from Users where id = @idUsuario";
                command.CommandType = System.Data.CommandType.Text;

                command.Parameters.Add(new SqlParameter("id", idUsuario));

                using (SqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        usuario = new Usuario()
                        {
                            name = dr.GetString(0),
                            cpf = dr.GetString(1),
                            email = dr.GetString(2),
                            password = dr.GetString(3),
                            birthday = dr.GetDateTime(4),
                        };
                    }
                }

                    connection.Close();
            }

            return Ok(usuario);
        }
    }
}
