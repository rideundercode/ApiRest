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
            
            var user_existant = db.Users.Where(u => u.Username == UserEntry.Username).FirstOrDefault() ;

            if(user_existant is not null)
            {
                var ex = new Exception("Utilisateur deja existant") ;
                return StatusCode(409, ex ) ;
            }
        
            var user = new User {
                Username = UserEntry.Username,
                FirstName = UserEntry.FirstName,
                LastName = UserEntry.LastName,
                Role = UserRole.ROLE_USER,
                Password = BCrypt.Net.BCrypt.HashPassword(UserEntry.Password),
                Address = UserEntry.Address,
                CreationDate = DateTime.Now 
                };

            user.Address.CreationDate = DateTime.Now ;
                            
            try 
            {
                db.Users.Add(user);
                var res = db.SaveChanges();

                if(res > 0)
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

            var verification = db.Users.Where(u => u.Username == name ).FirstOrDefault() ;

            if(verification is not null)
            {
                var UserConnecte = verification ;
                
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
        
            return Unauthorized(new {error = "Mot de passe ou username incorrect"} ) ;  
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

            var verification = db.Users.Where(a => a.Username == usernameToken).FirstOrDefault() ;            

            if(verification is not null)
            {
                var UserConnecte = verification ;
                
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
    }
}

