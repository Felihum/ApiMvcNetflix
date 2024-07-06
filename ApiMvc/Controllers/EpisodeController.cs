using ApiMvc.Contexts;
using ApiMvc.Dtos;
using ApiMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiMvc.Controllers
{
    [ApiController]
    [Route("api/episodes")]
    public class EpisodeController : Controller
    {
        private readonly ApplicationDbContext context;

        public EpisodeController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult> CreateEpisode([FromBody] EpisodeDTO episodeDTO)
        {
            try
            {
                var existingEpisode = await context.Episodes.FirstOrDefaultAsync(e => e.number == episodeDTO.number && e.idSeason == episodeDTO.idSeason);
                var episodeSeason = await context.Seasons.FirstOrDefaultAsync(s => s.id == episodeDTO.idSeason);

                if (existingEpisode != null)
                {
                    return BadRequest("Episódio já existente!");
                }

                Episode episode = new Episode
                {
                    title = episodeDTO.title,
                    description = episodeDTO.description,
                    duration = episodeDTO.duration,
                    number = episodeDTO.number,
                    idSeason = episodeDTO.idSeason,
                    season = episodeSeason
                };

                await context.Episodes.AddAsync(episode);
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
        public async Task<ActionResult<List<Episode>>> GetAllEpisodes()
        {
            try
            {
                return await context.Episodes.Include(e => e.season).ToListAsync();
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }
    }
}
