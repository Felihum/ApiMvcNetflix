using ApiMvc.Contexts;
using ApiMvc.Dtos;
using ApiMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiMvc.Controllers
{
    [ApiController]
    [Route("api/subscriptions")]
    public class SubscriptionController : Controller
    {
        private readonly ApplicationDbContext context;

        public SubscriptionController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("")]
        public async Task<ActionResult<List<Subscription>>> GetAllSubscriptions()
        {
            try
            {
                return await context.Subscriptions.Include(s => s.users).ToListAsync();
            }
            catch (Exception error) 
            {
                return BadRequest(error.Message);
            }
        }

        [HttpGet("{nameSubscription}")]
        public async Task<ActionResult<Subscription>> GetSubscriptionByName([FromRoute] string nameSubscription)
        {
            try
            {
                return await context.Subscriptions.Include(s => s.users).FirstOrDefaultAsync(s => s.name == nameSubscription);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpPost("")]
        public async Task<ActionResult> CreateSubscription([FromBody] SubscriptionDTO subscriptionDTO)
        {
            try
            {
                var existingSubscription = await context.Subscriptions.FirstOrDefaultAsync(s => s.id == subscriptionDTO.id);

                if (existingSubscription != null)
                {
                    return BadRequest("Essa Assinatura já existe!");
                }

                var subscriptionUsers = await context.Users.Where(u => u.idSubscription == subscriptionDTO.id).ToListAsync();

                Subscription subscription = new Subscription { 
                    name = subscriptionDTO.name,
                    value = subscriptionDTO.value,
                    period = subscriptionDTO.period,
                    users = subscriptionUsers
                };

                await context.Subscriptions.AddAsync(subscription);

                await context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpPut("{idSubscription}")]
        public async Task<ActionResult> UpdateSubscription([FromRoute] int idSubscription, [FromBody] SubscriptionDTO subscriptionDTO)
        {
            try
            {
                var subscription = await context.Subscriptions.FirstOrDefaultAsync(s => s.id == idSubscription);

                if (subscription == null) 
                {
                    return NotFound("Essa assinatura não existe!");
                }

                var subscriptionUsers = await context.Users.Where(u => u.idSubscription == subscriptionDTO.id).ToListAsync();

                subscription.name = subscriptionDTO.name;
                subscription.value = subscriptionDTO.value;
                subscription.period = subscriptionDTO.period;
                subscription.users = subscriptionUsers;

                context.Subscriptions.Update(subscription);

                await context.SaveChangesAsync();

                return Ok(subscription);

            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpDelete("{idSubscription}")]
        public async Task<ActionResult> DeleteSubscription([FromRoute] int idSubscription)
        {
            try
            {
                await context.Subscriptions.Where(s => s.id == idSubscription).ExecuteDeleteAsync();

                return Ok();
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }
    }
}
