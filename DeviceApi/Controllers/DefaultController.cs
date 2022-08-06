using Microsoft.AspNetCore.Mvc;

namespace DeviceApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class DefaultController : ControllerBase
    {

        [HttpGet]
        public ActionResult Status()
        {
            return Ok();
        }
    }
}