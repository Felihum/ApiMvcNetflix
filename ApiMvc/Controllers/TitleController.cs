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

        /*public async Task<ActionResult> GetTitleByTitle()
        {

        }


        public async Task<ActionResult> UpdateTitle()
        {

        }

        public async Task<ActionResult> DeleteTitle()
        {

        }*/
    }
}
