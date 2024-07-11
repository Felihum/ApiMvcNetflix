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
    [ApiController]
    [Route("api/titles")]
    public class TitleController : Controller
    {
        private readonly ApplicationDbContext context;

        public TitleController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateTitle([FromBody] TitleDTO titleDTO)
        {
            try
            {
                var existingTitle = await context.Titles.FirstOrDefaultAsync(t => t.title == titleDTO.title);
                var titleSeasons = await context.Seasons.Where(s => s.idTitle == titleDTO.id).ToListAsync();

                if (existingTitle != null)
                {
                    return BadRequest("Esse Título já existe!");
                }

                Title title = new Title {
                    title = titleDTO.title,
                    releaseYear = titleDTO.releaseYear,
                    gender = titleDTO.gender,
                    image = titleDTO.image,
                    logo = titleDTO.logo,
                    description = titleDTO.description,
                    type = titleDTO.type,
                    ageRating = titleDTO.ageRating,
                    seasons = titleSeasons
                };

                await context.Titles.AddAsync(title);

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
        public async Task<ActionResult<List<Title>>> GetAllTitles()
        {
            try
            {
                return await context.Titles.Include(t => t.seasons).ThenInclude(s => s.episodes).ToListAsync();
            }
            catch (Exception error) { 
                return BadRequest(error.Message);
            }
        }

        [HttpGet("{title}")]
        public async Task<ActionResult<Title>> GetTitleByTitle([FromRoute] string title)
        {
            try
            {
                return await context.Titles.Include(t => t.seasons).ThenInclude(s => s.episodes).FirstOrDefaultAsync(t => t.title == title);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpPut("{idTitle}")]
        public async Task<ActionResult> UpdateTitle([FromBody] TitleDTO titleDTO, [FromRoute] int idTitle)
        {
            try
            {
                var title = await context.Titles.FirstOrDefaultAsync(t => t.id == idTitle);
                var titleSeasons = await context.Seasons.Where(s => s.idTitle == idTitle).ToListAsync();

                if (title == null)
                {
                    return NotFound("Esse Título não existe!");
                }

                title.title = titleDTO.title;
                title.releaseYear = titleDTO.releaseYear;
                title.gender = titleDTO.gender;
                title.image = titleDTO.image;
                title.logo = titleDTO.logo;
                title.description = titleDTO.description;
                title.type = titleDTO.type;
                title.ageRating = titleDTO.ageRating;
                title.seasons = titleSeasons;

                context.Titles.Update(title);

                await context.SaveChangesAsync();
                
                return Ok(title);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpDelete("{idTitle}")]
        public async Task<ActionResult> DeleteTitle([FromRoute] int idTitle)
        {
            try
            {
                await context.Titles.Where(t => t.id == idTitle).ExecuteDeleteAsync();

                return Ok();
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }
    }
}
