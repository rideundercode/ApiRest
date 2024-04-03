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
    //[Authorize]
    public class UserController : ControllerBase
    {
     	private readonly APIDbContext db;

        public UserController(APIDbContext db)
        {
            this.db = db;
        }     	
         
        [HttpGet("/user")]
        public ActionResult<User> get()
        {   
            var users = db.Users.FromSqlRaw("SELECT * FROM user").ToList();
            return Ok(users) ; 
        }

        [HttpGet("/user/{id}")]
        public ActionResult<User> get_id(int id)
        {   
            var user = db.Users.Where(a => a.Id == id).FirstOrDefault();

            if(user is not null)
                return Ok(user) ;

            return NotFound( new { id = id } ) ; 
        }

        [HttpPut("/user/{id}")]
        public ActionResult<User> modify(int id, [FromBody] User UserEntry)
        {
            string token ;
            var admin = false ;

            try
            {
                token = Request.Headers["Authorization"].FirstOrDefault().Split(" ").Last() ;        
            }
            
            catch(Exception e)
            {
                return Unauthorized(new { exception = e } );
            }

            var tokenUtils = new JwtTokenUtil() ;
            var usernameToken = tokenUtils.GetUsernameFromToken(token) ;

            User userdbco = db.Users.Where(a => a.Username == usernameToken).FirstOrDefault() ;
   
            if(userdbco is null)
                return Unauthorized(new { message = "Mauvais token" } ) ; 

            if(userdbco.Role == UserRole.ROLE_ADMIN)
                admin = true ; 
            
            User userdb = db.Users.Where(a => a.Id == id).FirstOrDefault();        
            
            if(userdb is null)
                return NotFound(new { id = id }) ;
       
            if(admin || userdb == userdbco )
            {
                userdb.Role = (UserEntry.Role is null) ? userdb.Role : UserEntry.Role ;
                userdb.Username = (UserEntry.Username is null) ? userdb.Username : UserEntry.Username ;       
                userdb.UpdatedDate = DateTime.Now ;

                try
                {
                    var res = db.SaveChanges();

                    if(res == 1)
                        return Ok(userdb) ;
                    
                    throw new DbUpdateException("Erreur mis a jour addresse") ;
                }
                
                catch (Exception e)
                {
                    return StatusCode(400, e) ;
                }
            }

            return StatusCode( 403, new {message = "Vous n'avez pas l'authorisation "}) ;
        }

        [HttpDelete("/user/{id}")]
        public ActionResult<User> delete(int id)
        {
            string token ;
            var admin = false ;

            try
            {
                token = Request.Headers["Authorization"].FirstOrDefault().Split(" ").Last() ;        
            }
            
            catch(Exception e)
            {
                return Unauthorized(new { exception = e } );
            }

            var tokenUtils = new JwtTokenUtil() ;
            var usernameToken = tokenUtils.GetUsernameFromToken(token) ;

            User userdbco = db.Users.Where(a => a.Username == usernameToken).FirstOrDefault() ;
   
            if(userdbco is null)
                return Unauthorized(new { message = "Mauvais token" } ) ; 

            if(userdbco.Role == UserRole.ROLE_ADMIN)
                admin = true ; 
            
            var verif = false ;

            User userdb = db.Users.Where(a => a.Id == id).FirstOrDefault() ;

            if(userdb is null)
                return NotFound(new { success = verif }) ;
            
            if(admin || userdbco == userdb )
            {
                db.Users.Remove(userdb);            
                
                var r = db.SaveChanges();
                
                if(r == 1)
                    verif = true ;
            
                return Ok(new { success = verif }) ;
            }

            return StatusCode(403, new { success = verif }) ;
        }

        
    }
}

