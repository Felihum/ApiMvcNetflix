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
                    image = episodeDTO.image,
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

        [HttpPut("{idEpisode}")]
        public async Task<ActionResult> UpdateEpisode([FromRoute] int idEpisode, [FromBody] EpisodeDTO episodeDTO)
        {
            try
            {
                var episode = await context.Episodes.FirstOrDefaultAsync(e => e.id == idEpisode);

                if (episode == null)
                {
                    return NotFound("Esse episódio não existe!");
                }

                var episodeSeason = await context.Seasons.FirstOrDefaultAsync(s => s.id == episodeDTO.idSeason);

                episode.title = episodeDTO.title;
                episode.description = episodeDTO.description;
                episode.number = episodeDTO.number;
                episode.duration = episodeDTO.duration;
                episode.image = episodeDTO.image;
                episode.idSeason = episodeDTO.idSeason;
                episode.season = episodeSeason;

                context.Episodes.Update(episode);

                await context.SaveChangesAsync();

                return Ok(episode);
                
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpDelete("{idEpisode}")]
        public async Task<ActionResult> DeleteEpisode([FromRoute] int idEpisode)
        {
            try
            {
                await context.Episodes.Where(e => e.id == idEpisode).ExecuteDeleteAsync();

                return Ok();
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }
    }
}
