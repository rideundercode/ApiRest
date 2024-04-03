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
        
    //on peut commander un film qu'une fois et on peut le supprimer
    //on peut voir la liste des commandes 
    //on peut supprimer les commande
    //on ne peut pas modifier les commandes
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class CommandeController : ControllerBase
    {
     	private readonly APIDbContext db;

        public CommandeController(APIDbContext db)
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
        //http://localhost:8090/Commande/ renvoie la liste des Commandes
        //http://localhost:8090/Commande/1/ ne renvoie que le Commande 
        [HttpGet("/commande-list")]
        public ActionResult get()
        {
            var commandes = db.Commandes.ToList();
            return Ok(commandes) ;
        }
        
        [HttpGet("/commande")]
        public ActionResult get_commandes_user()
        {
            var userdb = get_user() ;
            
            var liste_commande = db.Commandes.Where(a => a.User == userdb).ToList(); //FirstOrDefault();

            if(liste_commande is not null)
                return Ok(liste_commande) ;
            
            return NotFound( new { message = "Aucune commande trouve", User_id = userdb.Id } ) ; 
        }

        [HttpGet("/test-commande/{commande}")]
        public bool already_commander(Commande commande)
        {
            var userdb = get_user() ;

            var test = db.Commandes.Where(a => a.ArticleId == commande.ArticleId && a.User == userdb ).FirstOrDefault();

            if(test is not null)
                return true  ;
            
            return false ; 
        }
            
        //POST : création d'une nouvelle adresse
        //Retourne l'entité “Commande” récemment créée
        [Authorize]
        [HttpPost("/commande")]
        public ActionResult<Commande> create([FromBody] Commande UserEntry)
        {
            User userdb = get_user() ; 

            if(userdb is null ) //bloquer
                return Unauthorized(new { message = "Vous n'etes pas autorise !" } ) ; 

            var commande = new Commande {
                User = userdb ,
                ArticleId = UserEntry.ArticleId,
                CreationDate = DateTime.Now 
            };
            
            var test = already_commander(commande) ;
             
            if( test ) 
                return StatusCode(409, new{message = "Vous avez deja commande cet article"} ) ;
            
            try 
            {
                db.Commandes.Add(commande);
                var res = db.SaveChanges();

                if(res > 0)
                    return StatusCode(201,commande) ;

                else 
                    throw new Exception("Erreur avec la base de donnees") ;   

            }
            
            catch(Exception e){
                return StatusCode(400, e) ;       
            }    
        }
        
        //DELETE : Suppression d'une commande
        //pas de droit special admin
        //Retourne un JSON avec une clé “success” à true ou false
        [HttpDelete("/commande/{id}")]
        public ActionResult delete(int id)
        {
            User userdb = get_user() ; 

            if(userdb is null )
                return Unauthorized(new { message = "Vous n'etes pas autorise !" } ) ;
                        
            var verif = false ;

            Commande Commandedb = db.Commandes.Where(a => a.Id == id && a.User == userdb).FirstOrDefault() ;
            
            if(Commandedb is null)
                return NotFound(new { success = verif }) ;
    
            db.Commandes.Remove(Commandedb);            
            var r = db.SaveChanges();
                
            if(r > 0)
                verif = true ;
            
            return Ok(new { success = verif }) ;
        }
    }
}

