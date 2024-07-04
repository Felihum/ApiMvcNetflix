using ApiMvc.Models;
using ApiMvc.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace ApiMvc.Controllers
{
    public class TitleController : Controller
    {
        /*public async Task<IActionResult> CreateTitle([FromServices] IOptions<ConnectionStringOptions> options, [FromBody] Title title)
        {
            try
            {
                using (SqlConnection connection = new(options.Value.MyConnection))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = @"insert into Titles (title, releaseYear, gender, image, description, type, ageRating)";
                }
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }*/
    }
}
