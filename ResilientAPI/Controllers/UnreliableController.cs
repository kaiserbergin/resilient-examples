using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ResilientAPI.Models;

namespace ResilientAPI.Controllers
{
    [Route("unreliable-endpoints")]
    [ApiController]
    public class UnreliableController : ControllerBase
    {

        [HttpPost]
        public async Task<ActionResult> ChooseYourDestiny(GenerateResponseRequestWaitWhat shrug)
        {
            await Task.Delay(shrug.DelayInMilliseconds);
            return new StatusCodeResult((int)shrug.HttpStatusCode);
        }
    }
}
