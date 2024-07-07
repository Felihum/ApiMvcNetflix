using ApiMvc.Contexts;
using ApiMvc.Dtos;
using ApiMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiMvc.Controllers
{
    [ApiController]
    [Route("api/profiles")]
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext context;

        public ProfileController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("")]
        public async Task<ActionResult<List<Profile>>> GetAllProfiles()
        {
            try
            {
                return await context.Profiles.Include(p => p.usuario).ToListAsync();
            }
            catch (Exception error) 
            {
                return BadRequest(error.Message);
            }
        }

        [HttpGet("{idProfile}")]
        public async Task<ActionResult<Profile>> GetProfileById([FromRoute] int idProfile)
        {
            try
            {
                return await context.Profiles.Include(p => p.usuario).FirstOrDefaultAsync(p => p.id == idProfile);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpPost("")]
        public async Task<ActionResult> CreateProfile([FromBody] ProfileDTO profileDTO)
        {
            try
            {
                var existingProfile = await context.Profiles.FirstOrDefaultAsync(p => p.id == profileDTO.id);
                
                if (existingProfile != null)
                {
                    return BadRequest("Episódio já existente!");
                }

                var profileUser = await context.Users.FirstOrDefaultAsync(u => u.id == profileDTO.idUser);

                Profile profile = new Profile { 
                    name = profileDTO.name,
                    type = profileDTO.type,
                    image = profileDTO.image,
                    idUser = profileDTO.idUser,
                    usuario = profileUser
                };

                await context.Profiles.AddAsync(profile);

                await context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpPut("{idProfile}")]
        public async Task<ActionResult> UpdateProfile([FromRoute] int idProfile, [FromBody] ProfileDTO profileDTO)
        {
            try
            {
                var profile = await context.Profiles.FirstOrDefaultAsync(p => p.id == idProfile);
                
                if (profile == null)
                {
                    return NotFound("Esse Perfil não existe!");
                }

                var profileUser = await context.Users.FirstOrDefaultAsync(u => u.id == profileDTO.idUser);

                profile.name = profileDTO.name;
                profile.type = profileDTO.type;
                profile.image = profileDTO.image;
                profile.idUser = profileDTO.idUser;
                profile.usuario = profileUser;

                context.Profiles.Update(profile);

                await context.SaveChangesAsync();

                return Ok(profile);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpDelete("{idProfile}")]
        public async Task<ActionResult> DeleteProfile([FromRoute] int idProfile)
        {
            try
            {
                await context.Profiles.Where(p => p.id == idProfile).ExecuteDeleteAsync();

                return Ok();
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }
    }
}
