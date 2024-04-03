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
    //seul les admin et redacteur ont les droits de creer/modifier/supprimer
    //on peut voir la liste
    //on peut voir un selon son id
    [ApiController]
    [Route("[controller]")]

    public class RealisateurController : ControllerBase
    {
     	private readonly APIDbContext db;

        public RealisateurController(APIDbContext db)
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
         
        //GET : pour récupérer une liste ou une seule adresse 
        //http://localhost:8090/realisateur/ renvoie la liste des realisateurs
        //http://localhost:8090/realisateur/1/ ne renvoie que le realisateur 
        [HttpGet("/realisateur")]
        public ActionResult<Realisateur> get()
        {
            var realisateur = db.Realisateurs.ToList();
            return Ok(realisateur) ;
        }
        
        [HttpGet("/realisateur/{id}")]
        public ActionResult<Realisateur> get_id(int id)
        {
            var realisateur = db.Realisateurs.Where(a => a.Id == id).FirstOrDefault();

            if(realisateur is not null)
                return Ok(realisateur) ;
            
            return NotFound( new { message = "realisateur non trouve", realisateur_id = id } ) ; 
        }
        
        
        //POST : création
		[Authorize]
        [HttpPost("/realisateur")]
        public ActionResult<Realisateur> create([FromBody] Realisateur UserEntry)
        {
            User userdb = get_user() ;
   
            if(userdb is null || ((userdb.Role != UserRole.ROLE_ADMIN) && (userdb.Role != UserRole.ROLE_REDACTEUR)) )
                return Unauthorized(new { message = "Vous n'etes pas autorise !" } ) ; 

            var realisateur = new Realisateur {
                NomComplet = UserEntry.NomComplet,
                Nationalite = UserEntry.Nationalite,
                Biographie = UserEntry.Biographie,
                Naissance = UserEntry.Naissance,
                CreationDate = DateTime.Now 
            };
            
            var realisateur_existant = db.Realisateurs.ToList();
        
            if(realisateur_existant.Contains(realisateur))
            {
                var ex = new Exception("realisateur deja en base de donnee") ;
                return StatusCode(409, ex ) ;
            }

            try 
            {
                db.Realisateurs.Add(realisateur);
                var res = db.SaveChanges();

                if(res > 0)
                    return StatusCode(201,realisateur) ;

                else 
                    throw new Exception("Erreur avec la base de donnees") ;   

            }
            
            catch(Exception e){
                return StatusCode(400, e) ;       
            }    
        }
    
        //PUT : Modification d'un realisateur
        //Retourne l'entité “realisateur” mise à jour
        [Authorize]
        [HttpPut("/realisateur/{id}")]
        public ActionResult<Realisateur> modify(int id, [FromBody] Realisateur UserEntry)
        {
            User userdb = get_user() ;
   
            if(userdb is null || ((userdb.Role != UserRole.ROLE_ADMIN) && (userdb.Role != UserRole.ROLE_REDACTEUR)) )
                return Unauthorized(new { message = "Vous n'avez pas l'autorisation !" } ) ; 
            
            Realisateur realisateurdb = db.Realisateurs.Where(a => a.Id == id).FirstOrDefault() ;

            if(realisateurdb is null)
                return NotFound(new { id = id }) ;
                
            realisateurdb.NomComplet = (UserEntry.NomComplet is null) ? realisateurdb.NomComplet : UserEntry.NomComplet ;
            realisateurdb.Nationalite = (UserEntry.Nationalite is null) ? realisateurdb.Nationalite : UserEntry.Nationalite ;
            realisateurdb.Biographie = (UserEntry.Biographie != realisateurdb.Biographie) ? realisateurdb.Biographie : UserEntry.Biographie ;
            realisateurdb.Naissance = (UserEntry.Naissance != realisateurdb.Naissance) ? realisateurdb.Naissance : UserEntry.Naissance ;
            realisateurdb.UpdatedDate = DateTime.Now ;
        
            try
            {
                db.Realisateurs.Update(realisateurdb) ;
                var res = db.SaveChanges();

                if(res > 0)
                    return Ok(realisateurdb) ;
                    
                throw new DbUpdateException("Erreur mis a jour realisateur") ;
            }
                
            catch (Exception e){
                return StatusCode(400, e) ;
            }
        }
        
        //DELETE : Suppression d'une adresse
        //Retourne un JSON avec une clé “success” à true ou false
        [HttpDelete("/realisateur/{id}")]
        public ActionResult<Realisateur> delete(int id)
        {
            User userdb = get_user() ;
            
            if(userdb is null || ((userdb.Role != UserRole.ROLE_ADMIN) && (userdb.Role != UserRole.ROLE_REDACTEUR)) )
                return Unauthorized(new { message = "Vous n'etes pas autorise !" } ) ;
                        
            var verif = false ;

            Realisateur realisateurdb = db.Realisateurs.Where(a => a.Id == id).FirstOrDefault() ;
            
            if(realisateurdb is null)
                return NotFound(new { success = verif }) ;
            
            db.Realisateurs.Remove(realisateurdb);            
            var r = db.SaveChanges();
                
            if(r > 0)
                verif = true ;
            
            return Ok(new { success = verif }) ;
            
        }
    }
}

