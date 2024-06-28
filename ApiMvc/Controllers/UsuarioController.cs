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
        public async Task<IActionResult> CreateUsuario([FromServices] IOptions<ConnectionStringOptions> options, Usuario usuario)
        {
            try
            {
                bool usuarioExiste = await VerificarUsuarioExistente(options, usuario.cpf, usuario.email);

                if (usuarioExiste)
                {
                    return BadRequest("O usuario ja existe com esse cpf e email.");
                }

                using (SqlConnection connection = new SqlConnection(options.Value.MyConnectionLTA))
                {
                    await connection.OpenAsync();

                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = @"insert into Users (name, cpf, email, password, birthday) values(@name, @cpf, @email, @password, @birthday)";
                    command.CommandType = System.Data.CommandType.Text;

                    command.Parameters.Add(new SqlParameter("name", usuario.name));
                    command.Parameters.Add(new SqlParameter("cpf", usuario.cpf));
                    command.Parameters.Add(new SqlParameter("email", usuario.email));
                    command.Parameters.Add(new SqlParameter("password", usuario.password));
                    command.Parameters.Add(new SqlParameter("birthday", usuario.birthday));

                    await command.ExecuteNonQueryAsync();

                    await connection.CloseAsync();
                }

                return Ok();
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpGet]
        [Route("{idUsuario}")]
        public async Task<IActionResult> GetUsuarioById([FromServices] IOptions<ConnectionStringOptions> options, [FromRoute] int idUsuario)
        {
            try
            {
                Usuario usuario = null;

                using (SqlConnection connection = new(options.Value.MyConnectionLTA))
                {
                    await connection.OpenAsync();

                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = @"select * from Users where id = @idUsuario";
                    command.CommandType = System.Data.CommandType.Text;

                    command.Parameters.Add(new SqlParameter("@idUsuario", idUsuario));

                    using (SqlDataReader dr = await command.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
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

                    await connection.CloseAsync();
                }

                if (usuario == null)
                {
                    return NotFound();
                }

                return Ok(usuario);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllUsuarios([FromServices] IOptions<ConnectionStringOptions> options)
        {
            List<Usuario> usuarios = new List<Usuario>();

            try
            {
                using (SqlConnection connection = new SqlConnection(options.Value.MyConnectionLTA))
                {
                    await connection.OpenAsync();

                    SqlCommand command = new SqlCommand();

                    command.Connection = connection;
                    command.CommandText = @"select * from Users";
                    command.CommandType = System.Data.CommandType.Text;

                    using (SqlDataReader dr = await command.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            usuarios.Add(new Usuario() {
                                id = dr.GetInt32(dr.GetOrdinal("id")),
                                name = dr["name"] as string,
                                cpf = dr["cpf"] as string,
                                email = dr["email"] as string,
                                password = dr["password"] as string,
                                birthday = dr["birthday"] as DateTime? ?? DateTime.MinValue
                            });
                        }
                    }

                    await connection.CloseAsync();
                }

                return Ok(usuarios);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpGet]
        [Route("findByCpf/{cpfUsuario}")]
        public async Task<IActionResult> GetUsuarioByCpf([FromServices] IOptions<ConnectionStringOptions> options, [FromRoute] string cpfUsuario)
        {
            try
            {
                Usuario usuario = null;

                using (SqlConnection connection = new(options.Value.MyConnectionLTA))
                {
                    await connection.OpenAsync();

                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = @"select * from Users where cpf = @cpfUsuario";
                    command.CommandType = System.Data.CommandType.Text;

                    command.Parameters.Add(new SqlParameter("@cpfUsuario", cpfUsuario));

                    using (SqlDataReader dr = await command.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
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

                    await connection.CloseAsync();
                }

                if (usuario == null)
                {
                    return NotFound();
                }

                return Ok(usuario);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpDelete]
        [Route("{idUsuario}")]

        public async Task<IActionResult> DeleteUsuario([FromServices] IOptions<ConnectionStringOptions> options, [FromRoute] int idUsuario)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(options.Value.MyConnectionLTA))
                {
                    await connection.OpenAsync();

                    SqlCommand command = new();
                    command.Connection = connection;
                    command.CommandText = @"delete from Users where id = @idUsuario";

                    command.Parameters.Add(new SqlParameter("@idUsuario", idUsuario));

                    command.ExecuteNonQueryAsync();

                    await connection.CloseAsync();
                }

                return Ok();
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
            
        }

        [HttpPut]
        [Route("{idUsuario}")]
        public async Task<IActionResult> PutUsuario([FromServices] IOptions<ConnectionStringOptions> options, [FromBody] Usuario usuario, [FromRoute] int idUsuario)
        {
            try
            {
                using (SqlConnection connection = new(options.Value.MyConnectionLTA))
                {
                    await connection.OpenAsync();

                    SqlCommand command = new();
                    command.Connection = connection;
                    command.CommandText = @"update Users set name = @name, cpf = @cpf, email = @email, password = @password, birthday = @birthday where id = @idUsuario";
                    command.Parameters.Add(new SqlParameter("name", usuario.name));
                    command.Parameters.Add(new SqlParameter("cpf", usuario.cpf));
                    command.Parameters.Add(new SqlParameter("email", usuario.email));
                    command.Parameters.Add(new SqlParameter("password", usuario.password));
                    command.Parameters.Add(new SqlParameter("birthday", usuario.birthday));
                    command.Parameters.Add(new SqlParameter("@idUsuario", idUsuario));
                    command.ExecuteNonQueryAsync();

                    await connection.CloseAsync();
                }

                return Ok();
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
            
        }

        private async Task<bool> VerificarUsuarioExistente(IOptions<ConnectionStringOptions> options, string CPF, string Email)
        {
            using (SqlConnection connection = new SqlConnection(options.Value.MyConnectionLTA))
            {
                await connection.OpenAsync();

                Usuario usuario = new Usuario();

                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = @"select Id from Users where cpf = @CPF or email = @Email";

                command.Parameters.Add(new SqlParameter("Email", Email));
                command.Parameters.Add(new SqlParameter("CPF", CPF));

                int? id = (int?)command.ExecuteScalar();

                await connection.CloseAsync();

                return id != null;

            }
        }
    }
}
