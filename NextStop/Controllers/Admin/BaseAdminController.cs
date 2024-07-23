using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NextStop.Controllers.Admin
{
    [Route("api/admin/[controller]/[action]")]
    [ApiController]
    public class BaseAdminController : ControllerBase
    {
    }
}
