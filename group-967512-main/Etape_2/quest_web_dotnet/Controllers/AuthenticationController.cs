using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics ;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using quest_web.Models ;
using System.ComponentModel.DataAnnotations ;

namespace quest_web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    
    
    public class AuthenticationController : ControllerBase
    {
     	private readonly APIDbContext db;

        public AuthenticationController(APIDbContext db)
        {
            this.db = db;
        }     	
         
        [HttpPost("/register")]
        //[Required(AllowEmptyStrings =true)]
        
        public ActionResult register([FromBody] User UserEntry)
        {
             

            if( UserEntry.Username is null|| UserEntry.Password is null )
            {
                var ex = new Exception("UserName ou Password ne doit pas etre null") ;
                return StatusCode(400, ex ) ;
            }    
            
            var user_existant = db.Users.FromSqlRaw("SELECT * FROM user").ToList();
        
            var user = new User {
                Username = UserEntry.Username,
                Password = UserEntry.Password,
                CreationDate = DateTime.Now 
                };

            if(user_existant.Contains(user))
            {
                var ex = new Exception("Utilisateur deja existant") ;
                return StatusCode(409, ex ) ;
            }    
                
            try 
            {
                db.Users.Add(user);
                var res = db.SaveChanges();

                if(res == 1)
                {
                    var result = new UserDetails 
                                {
                                    username = user.Username,
                                    role = user.Role   
                                } ;

                    return StatusCode(201,result ) ;
                }

                else 
                    throw new Exception("Erreur avec la base de donnees") ;   

            }
            
            catch(Exception e) 
            {
                return StatusCode(400, e) ;       
            }       

        }

        [HttpPost("/viewdb")]
        public IActionResult viewdb()
        {
            var user_existant = db.Users.FromSqlRaw("SELECT * FROM user").ToList();
            
            return Ok(user_existant) ;
                
        }

        
    }
}

