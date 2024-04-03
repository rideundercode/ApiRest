using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using quest_web.Models ;
using quest_web.Utils ;
using System.Collections.Generic ;

namespace quest_web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AddressController : ControllerBase
    {
     	private readonly APIDbContext db;

        public AddressController(APIDbContext db)
        {
            this.db = db;
        }     	
         
        //GET : pour récupérer une liste ou une seule adresse 
        //http://localhost:8090/address/ renvoie la liste des adresses
        //http://localhost:8090/address/1/ ne renvoie que l'adresse 
        [HttpGet("/address")]
        public ActionResult<Address> get()
        {
            //on recupere l'utilisateur connecter
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

            User userdb = db.Users.Where(a => a.Username == usernameToken).FirstOrDefault() ;
   
            if(userdb is null)
                return StatusCode(401, new {message = "Token non valide" }) ; // a modifier
            
            if(userdb.Role == UserRole.ROLE_ADMIN)
                admin = true ; 

            List<Address> addresses  ; //= null;

            if(admin)
                addresses = db.Addresses.ToList();
            
            else
                addresses = db.Addresses.Where(a => a.User == userdb ).ToList() ;
            

            return Ok(addresses) ;  
        }
        
        [HttpGet("/address/{id}")]
        public ActionResult<Address> get_id(int id)
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

            User userdb = db.Users.Where(a => a.Username == usernameToken).FirstOrDefault() ;
   
            if(userdb is null)
                return Unauthorized(new { message = "Mauvais token" } ) ; 

            if(userdb.Role == UserRole.ROLE_ADMIN)
                admin = true ; 
            
                        
            var address = db.Addresses.Where(a => a.Id == id).FirstOrDefault();

            if(address is not null)
            {
                if(admin || address.User == userdb )
                    return Ok(address) ;
                
                return Unauthorized(new { message = "Vous n'avez pas l'autorisation car ce n'est pas votre addresse" } ) ;
            }            

            return NotFound( new { id = id } ) ; 
        }
        
        //POST : création d'une nouvelle adresse
        //Retourne l'entité “address” récemment créée
        [HttpPost("/address")]
        public ActionResult<Address> register([FromBody] Address UserEntry)
        {
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

            User userdb = db.Users.Where(a => a.Username == usernameToken).FirstOrDefault() ;
   
            if(userdb is null)
                return Unauthorized(new { message = "Mauvais token" } ) ; 

            
            var address = new Address {
                User = userdb ,
                Street = UserEntry.Street,
                PostalCode = UserEntry.PostalCode,
                City = UserEntry.City,
                Country = UserEntry.Country, 
                CreationDate = DateTime.Now 
            };
            
            var address_existant = db.Addresses.ToList();
        
            if(address_existant.Contains(address))
            {
                var ex = new Exception("Addresse deja existant") ;
                return StatusCode(409, ex ) ;
            }

            try 
            {
                db.Addresses.Add(address);
                var res = db.SaveChanges();

                if(res > 0)
                    return StatusCode(201,address) ;

                else 
                    throw new Exception("Erreur avec la base de donnees") ;   

            }
            
            catch(Exception e) 
            {
                return StatusCode(400, e) ;       
            }    

        }
    
        //PUT : Modification d'un adresse
        //Retourne l'entité “address” mise à jour
        [HttpPut("/address/{id}")]
        public ActionResult<Address> modify(int id, [FromBody] Address UserEntry)
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

            User userdb = db.Users.Where(a => a.Username == usernameToken).FirstOrDefault() ;
   
            if(userdb is null)
                return Unauthorized(new { message = "Mauvais token" } ) ; 

            if(userdb.Role == UserRole.ROLE_ADMIN)
                admin = true ; 
            
            Address addressdb = db.Addresses.Where(a => a.Id == id).FirstOrDefault() ;

            if(addressdb is null)
                return NotFound(new { id = id }) ;
                    
            if(admin || addressdb.User == userdb)
            {
                addressdb.Street = (UserEntry.Street is null) ? addressdb.Street : UserEntry.Street ;
                addressdb.City = (UserEntry.City is null) ? addressdb.City : UserEntry.City ;
                addressdb.PostalCode = (UserEntry.PostalCode is null) ? addressdb.PostalCode : UserEntry.PostalCode ;
                addressdb.Country = (UserEntry.Country is null) ? addressdb.Country : UserEntry.Country ;    
                addressdb.UpdatedDate = DateTime.Now ;
        
                try
                {
                    db.Addresses.Update(addressdb) ;
                    var res = db.SaveChanges();

                    if(res > 0)
                        return Ok(addressdb) ;
                    
                    throw new DbUpdateException("Erreur mis a jour addresse") ;
                }
                
                catch (Exception e)
                {
                    return StatusCode(400, e) ;
                }
            }

            return StatusCode( 403, new {message = "Vous n'avez pas l'authorisation "}) ;
        }
        
        //DELETE : Suppression d'une adresse
        //Retourne un JSON avec une clé “success” à true ou false
        [HttpDelete("/address/{id}")]
        public ActionResult<Address> delete(int id)
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

            User userdb = db.Users.Where(a => a.Username == usernameToken).FirstOrDefault() ;
   
            if(userdb is null)
                return Unauthorized(new { message = "Mauvais token" } ) ; 

            if(userdb.Role == UserRole.ROLE_ADMIN)
                admin = true ; 
            
            var verif = false ;

            Address addressdb = db.Addresses.Where(a => a.Id == id).FirstOrDefault() ;
            
            if(addressdb is null)
                return NotFound(new { success = verif }) ;
            
            if(admin || addressdb.User == userdb)
            {
                db.Addresses.Remove(addressdb);            
                var r = db.SaveChanges();
                
                if(r > 0)
                    verif = true ;
            
                return Ok(new { success = verif }) ;
            }

            return StatusCode( 403, new {message = "Vous n'avez pas l'authorisation "}) ;
            
        }
    }
}

