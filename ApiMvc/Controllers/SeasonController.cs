using ApiMvc.Contexts;
using ApiMvc.Dtos;
using ApiMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiMvc.Controllers
{
    [ApiController]
    [Route("api/seasons")]
    public class SeasonController : Controller
    {
        private readonly ApplicationDbContext context;
        
        public SeasonController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult> CreateSeason([FromBody] SeasonDTO seasonDTO)
        {
            try
            {
                var existingSeason = await context.Seasons.FirstOrDefaultAsync(s => s.idTitle == seasonDTO.idTitle && s.number == seasonDTO.number);
                var seasonTitle = await context.Titles.FirstOrDefaultAsync(t => t.id == seasonDTO.idTitle);
                var seasonEpisodes = await context.Episodes.Where(e => e.idSeason == seasonDTO.id).ToListAsync();

                if (existingSeason != null)
                {
                    return BadRequest("Essa temporada já existe!");
                }

                Season season = new Season {
                    number = seasonDTO.number,
                    idTitle = seasonDTO.idTitle,
                    title = seasonTitle,
                    episodes = seasonEpisodes
                };

                await context.Seasons.AddAsync(season);

                await context.SaveChangesAsync();

                return Ok();

            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Season>>> GetAllSeasons()
        {
            try
            {
                return await context.Seasons.Include(s => s.episodes).ToListAsync();
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }
    }
}
