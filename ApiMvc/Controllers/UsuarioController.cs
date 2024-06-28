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
            bool usuarioExiste = VerificarUsuarioExistente(options, usuario.cpf, usuario.email);

            if (usuarioExiste)
            {
                return BadRequest("O usuario ja existe com esse cpf e email.");
            }

            using (SqlConnection connection = new SqlConnection(options.Value.MyConnection))
            {
                connection.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = @"insert into Users (name, cpf, email, password, birthday) values(@name, @cpf, @email, @password, @birthday)";
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
        public IActionResult GetUsuarioById([FromServices] IOptions<ConnectionStringOptions> options, [FromRoute] int idUsuario)
        {
            Usuario usuario = null;

            using (SqlConnection connection = new(options.Value.MyConnection))
            {
                connection.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = @"select * from Users where id = @idUsuario";
                command.CommandType = System.Data.CommandType.Text;

                command.Parameters.Add(new SqlParameter("@idUsuario", idUsuario));

                using (SqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        usuario = new Usuario()
                        {
                            id = dr.GetInt32(dr.GetOrdinal("id")),
                            name = dr["name"] as string,
                            cpf = dr["cpf"] as string,
                            email = dr["email"] as string,
                            password = dr["password"] as string,
                            birthday = dr["birthday"] as DateTime? ?? DateTime.MinValue
                        };
                    }
                }

                connection.Close();
            }

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        [HttpGet]
        [Route("findByCpf/{cpfUsuario}")]
        public IActionResult GetUsuarioByCpf([FromServices] IOptions<ConnectionStringOptions> options, [FromRoute] string cpfUsuario)
        {
            Usuario usuario = null;

            using (SqlConnection connection = new(options.Value.MyConnection))
            {
                connection.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = @"select * from Users where cpf = @cpfUsuario";
                command.CommandType = System.Data.CommandType.Text;

                command.Parameters.Add(new SqlParameter("@cpfUsuario", cpfUsuario));

                using (SqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        usuario = new Usuario()
                        {
                            id = dr.GetInt32(dr.GetOrdinal("id")),
                            name = dr["name"] as string,
                            cpf = dr["cpf"] as string,
                            email = dr["email"] as string,
                            password = dr["password"] as string,
                            birthday = dr["birthday"] as DateTime? ?? DateTime.MinValue
                        };
                    }
                }

                connection.Close();
            }

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        [HttpDelete]
        [Route("{idUsuario}")]

        public IActionResult DeleteUsuario([FromServices] IOptions<ConnectionStringOptions> options, [FromRoute] int idUsuario)
        {
            using (SqlConnection connection = new SqlConnection(options.Value.MyConnection))
            {
                connection.Open();

                SqlCommand command = new();
                command.Connection = connection;
                command.CommandText = @"delete from Users where id = @idUsuario";

                command.Parameters.Add(new SqlParameter("@idUsuario", idUsuario));

                command.ExecuteNonQuery();
            }

            return Ok();
        }

        [HttpPut]
        [Route("{idUsuario}")]
        public IActionResult PutUsuario([FromServices] IOptions<ConnectionStringOptions> options, [FromBody] Usuario usuario, [FromRoute] int idUsuario)
        {
            using (SqlConnection connection = new(options.Value.MyConnection))
            {
                connection.Open();

                SqlCommand command = new();
                command.Connection = connection;
                command.CommandText = @"update Users set name = @name, cpf = @cpf, email = @email, password = @password, birthday = @birthday where id = @idUsuario";
                command.Parameters.Add(new SqlParameter("name", usuario.name));
                command.Parameters.Add(new SqlParameter("cpf", usuario.cpf));
                command.Parameters.Add(new SqlParameter("email", usuario.email));
                command.Parameters.Add(new SqlParameter("password", usuario.password));
                command.Parameters.Add(new SqlParameter("birthday", usuario.birthday));
                command.Parameters.Add(new SqlParameter("@idUsuario", idUsuario));
                command.ExecuteNonQuery();

                connection.Close();
            }

            return Ok();
        }

        private bool VerificarUsuarioExistente(IOptions<ConnectionStringOptions> options, string CPF, string Email)
        {
            using (SqlConnection connection = new SqlConnection(options.Value.MyConnection))
            {
                connection.Open();

                Usuario usuario = new Usuario();

                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = @"select Id from Users where cpf = @CPF or email = @Email";

                command.Parameters.Add(new SqlParameter("Email", Email));
                command.Parameters.Add(new SqlParameter("CPF", CPF));

                int? id = (int?)command.ExecuteScalar();


                return id != null;

            }
        }
    }
}
