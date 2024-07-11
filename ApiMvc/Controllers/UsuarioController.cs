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

                var subscriptionObj = await context.Subscriptions.FirstOrDefaultAsync(sub => sub.id == usuarioDTO.idSubscription);
                var userProfiles = await context.Profiles.Where(profile => profile.idUser == usuarioDTO.id).ToListAsync();

                Usuario usuario = new Usuario
                {
                    cpf = usuarioDTO.cpf,
                    email = usuarioDTO.email,
                    password = usuarioDTO.password,
                    birthday = usuarioDTO.birthday,
                    role = usuarioDTO.role,
                    idSubscription = usuarioDTO.idSubscription,
                    profiles = userProfiles

                };

                context.Users.Add(usuario);

                await context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception error)
            {
                return BadRequest(new { message = error.Message, innerException = error.InnerException?.Message });
            }
        }

        [HttpGet]
        [Route("{idUsuario}")]
        public async Task<ActionResult<Usuario>> GetUsuarioById([FromServices] IOptions<ConnectionStringOptions> options, [FromRoute] int idUsuario)
        {
            try
            {
                return await context.Users.Include(u => u.subscription)
                                          .Include(u => u.profiles)
                                          .FirstOrDefaultAsync(u => u.id == idUsuario);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetAllUsuarios()
        {
            try
            {
                return await context.Users.Include(u => u.subscription).Include(u => u.profiles).ToListAsync();
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpGet]
        [Route("findByCpf/{cpfUsuario}")]
        public async Task<ActionResult<Usuario>> GetUsuarioByCpf([FromServices] IOptions<ConnectionStringOptions> options, [FromRoute] string cpfUsuario)
        {
            try
            {
                return await context.Users.Include(u => u.subscription).Include(u => u.profiles).FirstOrDefaultAsync(u => u.cpf == cpfUsuario);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpGet]
        [Route("findByEmail/{email}")]
        public async Task<ActionResult<Usuario>> GetUsuarioByEmail([FromServices] IOptions<ConnectionStringOptions> options, [FromRoute] string email)
        {
            try
            {
                return await context.Users.Include(u => u.subscription).Include(u => u.profiles).FirstOrDefaultAsync(u => u.email == email);
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
                await context.Users.Where(u => u.id == idUsuario).ExecuteDeleteAsync();
                return Ok();
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
            
        }

        [HttpPut]
        [Route("{idUsuario}")]
        public async Task<IActionResult> PutUsuario([FromServices] IOptions<ConnectionStringOptions> options, [FromBody] UsuarioDTO usuarioDTO, [FromRoute] int idUsuario)
        {
            try
            {
                var user = await context.Users.FirstOrDefaultAsync(u => u.id == idUsuario);

                if (user == null)
                {
                    return NotFound("O Usuario não existe!");
                }

                var subscriptionObj = await context.Subscriptions.FirstOrDefaultAsync(sub => sub.id == usuarioDTO.idSubscription);
                var userProfiles = await context.Profiles.Where(profile => profile.idUser == idUsuario).ToListAsync();

                user.cpf = usuarioDTO.cpf;
                user.email = usuarioDTO.email;
                user.password = usuarioDTO.password;
                user.birthday = usuarioDTO.birthday;
                user.role = usuarioDTO.role;
                user.idSubscription = usuarioDTO.idSubscription;
                user.subscription = subscriptionObj;
                user.profiles = userProfiles;

                context.Users.Update(user);
                await context.SaveChangesAsync();

                return Ok(user);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }
    }
}
