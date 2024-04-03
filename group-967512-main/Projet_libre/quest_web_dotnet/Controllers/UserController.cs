using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using quest_web.Models ;
using quest_web.Utils ;

namespace quest_web.Controllers
{
    //un utilisateur peut supprimer son compte
    //modifier
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
     	private readonly APIDbContext db;

        public UserController(APIDbContext db)
        {
            this.db = db;
        }
    
        [ApiExplorerSettings(IgnoreApi = true)]
        public User get_user()
        {
        string token ;
        
            try{
                token = Request.Headers["Authorization"].FirstOrDefault().Split(" ").Last() ;        
            }
            
            catch(Exception){
                return null;
            }

            var tokenUtils = new JwtTokenUtil() ;
            var usernameToken = tokenUtils.GetUsernameFromToken(token) ;

            return  db.Users.Where(a => a.Username == usernameToken).FirstOrDefault() ;
        }     	     	
         
        [HttpGet("/user")]
        public ActionResult<User> get()
        {   
            var users = db.Users.ToList();
            return Ok(users) ; 
        }

        [HttpGet("/user/{id}")]
        public ActionResult<User> get_id(int id)
        {   
            var user = db.Users.Where(u => u.Id == id).Include(a => a.Address).FirstOrDefault();

            if(user is not null)
                return Ok(user) ;

            return NotFound( new { id = id } ) ; 
        }

        [HttpGet("/user/name/{username}")]
        public ActionResult<User> get_name(string username)
        {   
            var user = db.Users.Where(a => a.Username == username).FirstOrDefault();

            if(user is not null)
                return Ok(user) ;

            return NotFound( new { username = username } ) ; 
        }

        [HttpPut("/user/{id}")]
        public ActionResult<User> modify(int id, [FromBody] User UserEntry)
        {   
            User userdbco = get_user() ;
   
            if(userdbco is null)
                return Unauthorized(new { message = "Mauvais token" } ) ; 

            User userdb = db.Users.Where(a => a.Id == id).Include(u => u.Address).FirstOrDefault();        
            
            if(userdb is null)
                return NotFound(new { id = id }) ;
       
            if(userdbco.Role == UserRole.ROLE_ADMIN || userdb == userdbco )
            {
                userdb.Role = (UserEntry.Role is null) ? userdb.Role : UserEntry.Role ;
                userdb.Username = (UserEntry.Username is null) ? userdb.Username : UserEntry.Username ;       
                userdb.FirstName = (UserEntry.FirstName is null) ? userdb.FirstName : UserEntry.FirstName ;       
                userdb.LastName = (UserEntry.LastName is null) ? userdb.LastName : UserEntry.LastName ;       
                
                if( !String.IsNullOrEmpty(UserEntry.Password) )  
                userdb.Password =  BCrypt.Net.BCrypt.HashPassword(UserEntry.Password) ;       
                
                if(userdb.Address != UserEntry.Address )
                userdb.Address = UserEntry.Address ;       
                
                userdb.UpdatedDate = DateTime.Now ;

                try
                {
                    db.Users.Update(userdb) ;
                    var res = db.SaveChanges();

                    if(res > 0)
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
            User userdbco = get_user() ;
   
            if(userdbco is null)
                return Unauthorized(new { message = "Mauvais token" } ) ; 

            var verif = false ;

            User userdb = db.Users.Where(a => a.Id == id).FirstOrDefault() ;

            if(userdb is null)
                return NotFound(new { success = verif }) ;
            
            if( userdbco.Role == UserRole.ROLE_ADMIN || userdbco == userdb )
            {
                db.Users.Remove(userdb);            
                
                var r = db.SaveChanges();
                
                if(r > 0)
                    verif = true ;
            
                return Ok(new { success = verif }) ;
            }

            return StatusCode(403, new { success = verif }) ;
        }

        
    }
}

