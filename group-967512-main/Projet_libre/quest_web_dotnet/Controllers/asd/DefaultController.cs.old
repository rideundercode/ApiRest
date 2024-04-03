using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace quest_web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    
    public class DefaultController : ControllerBase
    {
     	[HttpGet("/testSuccess")]
        public IActionResult testSuccess()
        {
            return Ok("success") ;
        }
        
        [HttpGet("/testNotFound")]
        public IActionResult testNotFound()
        {
        	return NotFound("not found") ;
        }
        
        [HttpGet("/testError")]
        public IActionResult testError()
        {
	        return StatusCode(500, "error") ;
        }
   
    }
}

