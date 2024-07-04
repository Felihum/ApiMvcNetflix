using ApiMvc.Contexts;
using ApiMvc.Dtos;
using ApiMvc.Models;
using ApiMvc.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace ApiMvc.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuarioController : Controller
    {
        private readonly ApplicationDbContext context;

        public UsuarioController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateUsuario([FromBody] UsuarioDTO usuarioDTO)
        {
            try
            {
                var existingUser = await context.Users.FirstOrDefaultAsync(user => user.cpf == usuarioDTO.cpf || user.email == usuarioDTO.email);

                if (existingUser != null)
                {
                    return BadRequest("Usuário já existente com o seguinte email ou cpf");
                }

                Usuario usuario = new Usuario
                {
                    cpf = usuarioDTO.cpf,
                    email = usuarioDTO.email,
                    password = usuarioDTO.password,
                    birthday = usuarioDTO.birthday,
                    idSubscription = usuarioDTO.idSubscription,

                };

                context.Users.Add(usuario);

                await context.SaveChangesAsync();

                if (usuarioDTO.profiles != null)
                {
                    foreach (var profileDTO in usuarioDTO.profiles)
                    {
                        Profile profile = new Profile
                        {
                            name = profileDTO.name,
                            type = profileDTO.type,
                            image = profileDTO.image,
                            idUser = usuario.id
                        };

                        context.Profiles.Add(profile); // Adicionar perfil ao contexto
                    }

                    // Salvar perfis
                    await context.SaveChangesAsync();
                }

                return Ok();
            }
            catch (Exception error)
            {
                return BadRequest(new { message = error.Message, innerException = error.InnerException?.Message });
            }
        }

        [HttpGet]
        [Route("{idUsuario}")]
        public async Task<IActionResult> GetUsuarioById([FromServices] IOptions<ConnectionStringOptions> options, [FromRoute] int idUsuario)
        {
            try
            {
                Usuario usuario = null;

                using (SqlConnection connection = new(options.Value.MyConnection))
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
                                cpf = dr["cpf"] as string,
                                email = dr["email"] as string,
                                password = dr["password"] as string,
                                birthday = dr["birthday"] as DateTime? ?? DateTime.MinValue,
                                idSubscription = dr.GetInt32(dr.GetOrdinal("idSubscription"))
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

        /*[HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllUsuarios([FromServices] IOptions<ConnectionStringOptions> options)
        {
            List<Usuario> usuarios = new List<Usuario>();

            try
            {
                using (SqlConnection connection = new SqlConnection(options.Value.MyConnection))
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
                                cpf = dr["cpf"] as string,
                                email = dr["email"] as string,
                                password = dr["password"] as string,
                                birthday = dr["birthday"] as DateTime? ?? DateTime.MinValue,
                                idSubscription = dr.GetInt32(dr.GetOrdinal("idSubscription"))
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
        }*/

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetAllUsuarios()
        {
            try
            {
                return await context.Users.ToListAsync();
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

                using (SqlConnection connection = new(options.Value.MyConnection))
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
                                cpf = dr["cpf"] as string,
                                email = dr["email"] as string,
                                password = dr["password"] as string,
                                birthday = dr["birthday"] as DateTime? ?? DateTime.MinValue,
                                idSubscription = dr.GetInt32(dr.GetOrdinal("idSubscription"))
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
        [Route("findByEmail/{email}")]
        public async Task<IActionResult> GetUsuarioByEmail([FromServices] IOptions<ConnectionStringOptions> options, [FromRoute] string email)
        {
            Usuario usuario = null;

            try
            {
                using (SqlConnection connection = new(options.Value.MyConnection))
                {
                    await connection.OpenAsync();
                    SqlCommand command = connection.CreateCommand();

                    command.CommandText = @"select * from Users where email = @email";

                    command.Parameters.Add(new SqlParameter("email", email));

                    command.CommandType = System.Data.CommandType.Text;

                    using (SqlDataReader dr = await command.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            usuario = new Usuario()
                            {
                                id = dr.GetInt32(dr.GetOrdinal("id")),
                                cpf = dr["cpf"] as string,
                                email = dr["email"] as string,
                                password = dr["password"] as string,
                                birthday = dr["birthday"] as DateTime? ?? DateTime.MinValue,
                                idSubscription = dr.GetInt32(dr.GetOrdinal("idSubscription"))
                            };
                        }
                    }

                    await connection.CloseAsync();
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
                using (SqlConnection connection = new SqlConnection(options.Value.MyConnection))
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
                using (SqlConnection connection = new(options.Value.MyConnection))
                {
                    await connection.OpenAsync();

                    SqlCommand command = new();
                    command.Connection = connection;
                    command.CommandText = @"update Users set cpf = @cpf, email = @email, password = @password, birthday = @birthday, idSubscription = @idSubscription where id = @idUsuario";
                    command.Parameters.Add(new SqlParameter("cpf", usuario.cpf));
                    command.Parameters.Add(new SqlParameter("email", usuario.email));
                    command.Parameters.Add(new SqlParameter("password", usuario.password));
                    command.Parameters.Add(new SqlParameter("birthday", usuario.birthday));
                    command.Parameters.Add(new SqlParameter("@idUsuario", idUsuario));
                    command.Parameters.Add(new SqlParameter("@idSubscription", usuario.idSubscription));
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

        private async Task<bool> VerificarUsuarioExistente(IOptions<ConnectionStringOptions> options, string CPF, string Email)
        {
            using (SqlConnection connection = new SqlConnection(options.Value.MyConnection))
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
