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

        [HttpGet("{idSeason}")]
        public async Task<ActionResult<Season>> GetSeasonById([FromRoute] int idSeason)
        {
            try
            {
                return await context.Seasons.FirstOrDefaultAsync(s => s.id == idSeason);
            } 
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpPut("{idSeason}")]
        public async Task<ActionResult> UpdateSeason([FromRoute] int idSeason, [FromBody] SeasonDTO seasonDTO)
        {
            try
            {
                var season = await context.Seasons.FirstOrDefaultAsync(s => s.id == idSeason);

                if (season == null)
                {
                    return NotFound("Essa temporada não existe!");
                }

                var seasonTitle = await context.Titles.FirstOrDefaultAsync(t => t.id == seasonDTO.idTitle);

                season.number = seasonDTO.number;
                season.idTitle = seasonDTO.idTitle;
                season.title = seasonTitle;

                context.Seasons.Update(season);

                await context.SaveChangesAsync();

                return Ok(season);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpDelete("{idSeason}")]
        public async Task<ActionResult> DeleteSeason([FromRoute] int idSeason)
        {
            try
            {
                await context.Seasons.Where(s => s.id == idSeason).ExecuteDeleteAsync();

                return Ok();
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }
    }
}
