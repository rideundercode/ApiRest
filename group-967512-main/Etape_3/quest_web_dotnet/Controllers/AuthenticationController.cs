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
    
    public class AuthenticationController : ControllerBase
    {
     	private readonly APIDbContext db;

        public AuthenticationController(APIDbContext db)
        {
            this.db = db;
        }     	
         
        [HttpPost("/register")]
        public ActionResult<User> register([FromBody] User UserEntry)
        {
            if( UserEntry.Username is null|| UserEntry.Password is null )
            {
                var ex = new Exception("UserName ou Password ne doit pas etre null") ;
                return StatusCode(400, ex ) ;
            }    
            
            var user_existant = db.Users.FromSqlRaw("SELECT * FROM user").ToList();
        
            var user = new User {
                Username = UserEntry.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(UserEntry.Password),
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
                                    Username = user.Username,
                                    Role = user.Role   
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

        [HttpPost("/authenticate")]
        public IActionResult authenticate([FromBody] User UserEntry)
        {            
            var name = UserEntry.Username ;
            var pass = UserEntry.Password ;

            var verification = db.Users.FromSqlRaw("SELECT * FROM user where username = {0}", name ) ;

            if(verification.Any())
            {
                var UserConnecte = verification.AsEnumerable().First() ;
                
                if(BCrypt.Net.BCrypt.Verify(pass, UserConnecte.Password))
                {     
                    var result = new UserDetails 
                                {
                                    Username = UserConnecte.Username,
                                    Role = UserConnecte.Role   
                                } ;

                    var jwtTokenAuth = new JwtTokenUtil().GenerateToken(result) ; 
                        
                    return Ok( new { token = jwtTokenAuth });     
                }
            }            
        
            return Unauthorized() ;  
        } 

        [Authorize]
        [HttpGet("/me")]
        public IActionResult me()
        {
            if (Request.QueryString.ToString().Length > 0)
                return BadRequest(new { error = "Requête invalide"}) ;

            string token ;

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

            var verification = db.Users.FromSqlRaw("SELECT * FROM user where username = {0}", usernameToken ) ;            

            if(verification.Any())
            {
                var UserConnecte = verification.AsEnumerable().First() ;
                
                var result = new UserDetails 
                                {
                                    Username = UserConnecte.Username,
                                    Role = UserConnecte.Role   
                                } ;

                return Ok(result) ;
            }

            else
            {
                return BadRequest(new { error = "Requête invalide"}) ;   
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

