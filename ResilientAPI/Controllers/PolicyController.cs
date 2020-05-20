using Microsoft.AspNetCore.Mvc;
using ResilientAPI.Models;
using System.Threading.Tasks;

namespace ResilientAPI.Controllers
{
    [Route("policies")]
    [ApiController]
    public class PolicyController : ControllerBase
    {
        [ActionName("single-policy")]
        public async Task<IActionResult> ExecuteSinglePolicy(PolicyExecutionRequest request)
        {
            
        }
    }
}
