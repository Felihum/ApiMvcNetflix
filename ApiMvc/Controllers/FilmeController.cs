using ApiMvc.Models;
using ApiMvc.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace ApiMvc.Controllers
{
    [Route("api/filmes")]
    [ApiController]
    public class FilmeController : Controller
    {
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateFilme([FromServices] IOptions<ConnectionStringOptions> options, Filme filme)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(options.Value.MyConnectionHome))
                {
                    await connection.OpenAsync();

                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;

                    command.CommandText = @"insert into Filmes (title, description, gender, releaseDate, classification, rate) values (@title, @description, @gender, @releaseDate, @classification, @rate)";
                    command.CommandType = System.Data.CommandType.Text;

                    command.Parameters.Add(new SqlParameter("title", filme.title));
                    command.Parameters.Add(new SqlParameter("description", filme.description));
                    command.Parameters.Add(new SqlParameter("gender", filme.gender));
                    command.Parameters.Add(new SqlParameter("releaseDate", filme.releaseDate));
                    command.Parameters.Add(new SqlParameter("classification", filme.classification));
                    command.Parameters.Add(new SqlParameter("rate", filme.rate));

                    await command.ExecuteNonQueryAsync();

                    await connection.CloseAsync();
                }

                return Ok();
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllFilmes([FromServices] IOptions<ConnectionStringOptions> options)
        {
            try
            {
                List<Filme> filmes = new List<Filme>();

                using (SqlConnection connection = new SqlConnection(options.Value.MyConnectionHome))
                {
                    await connection.OpenAsync();

                    SqlCommand command = new SqlCommand();

                    command.Connection = connection;
                    command.CommandText = @"select * from Filmes";
                    command.CommandType = System.Data.CommandType.Text;

                    using (SqlDataReader dr = await command.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            filmes.Add(new Filme()
                            {
                                id = dr.GetInt32(dr.GetOrdinal("id")),
                                title = dr["title"] as string,
                                description = dr["description"] as string,
                                gender = dr["gender"] as string,
                                classification = dr.GetInt32(dr.GetOrdinal("classification")),
                                rate = dr.GetInt32(dr.GetOrdinal("rate")),
                                releaseDate = dr["releaseDate"] as DateTime? ?? DateTime.MinValue
                            });
                        }
                    }
                }

                return Ok(filmes);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpPut]
        [Route("{idFilme}")]
        public async Task<IActionResult> PutFilme([FromServices] IOptions<ConnectionStringOptions> options, [FromRoute] int idFilme, [FromBody] Filme filme)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(options.Value.MyConnectionHome))
                {
                    await connection.OpenAsync();

                    //SqlCommand command = connection.CreateCommand();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;

                    command.CommandText = @"update Filmes set title = @title, description = @description, gender = @gender, releaseDate = @releaseDate, classification = @classification, rate = @rate where id = @idFilme";
                    command.CommandType = System.Data.CommandType.Text;

                    command.Parameters.Add(new SqlParameter("@idFilme", idFilme));
                    command.Parameters.Add(new SqlParameter("title", filme.title));
                    command.Parameters.Add(new SqlParameter("description", filme.description));
                    command.Parameters.Add(new SqlParameter("gender", filme.gender));
                    command.Parameters.Add(new SqlParameter("releaseDate", filme.releaseDate));
                    command.Parameters.Add(new SqlParameter("classification", filme.classification));
                    command.Parameters.Add(new SqlParameter("rate", filme.rate));

                    await command.ExecuteNonQueryAsync();

                    await connection.CloseAsync();
                }

                return Ok();
            }
            catch (Exception error) 
            {
                return BadRequest(error.Message);
            }
            
        }
    }
}
