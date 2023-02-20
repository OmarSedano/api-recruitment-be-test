using System;
using Microsoft.AspNetCore.Mvc;
using ApiApplication.Resources;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        [HttpGet]
        public IMDBStatus Get()
        {
            throw new NotImplementedException();
        }
    }
}
