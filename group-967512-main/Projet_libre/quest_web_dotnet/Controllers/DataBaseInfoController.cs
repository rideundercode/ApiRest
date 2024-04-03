using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using quest_web.Models ;
using quest_web.Utils ;

namespace quest_web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    
    public class DataBaseInfoController : ControllerBase
    {
     	private readonly APIDbContext db;

        public DataBaseInfoController(APIDbContext db)
        {
            this.db = db;
        } 
         
        [HttpGet("/viewdb/user")]
        public IActionResult viewdbuser()
        {
            var user_existant = db.Users.ToList();
            
            return Ok(user_existant) ;
                
        }

        [HttpGet("/viewdb/address")]
        public IActionResult viewdbaddress()
        {
            var addresses = db.Addresses.ToList();
            
            return Ok(addresses) ;
                
        }
    }
}

