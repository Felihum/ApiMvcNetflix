using ApiMvc.Contexts;
using ApiMvc.Models;
using ApiMvc.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace ApiMvc.Controllers
{
    public class TitleController : Controller
    {
        private readonly ApplicationDbContext context;

        public TitleController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Task<IActionResult> CreateTitle([FromBody] )
        {
            try
            {
                
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        public Task<ActionResult> GetAllTitles()
        {

        }

        public Task<ActionResult> GetTitleByTitle()
        {

        }


        public Task<ActionResult> UpdateTitle()
        {

        }

        public Task<ActionResult> DeleteTitle()
        {

        }
    }
}
