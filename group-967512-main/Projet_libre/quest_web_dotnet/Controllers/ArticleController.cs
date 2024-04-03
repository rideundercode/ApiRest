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

    public class ArticleController : ControllerBase
    {
     	private readonly APIDbContext db;

        public ArticleController(APIDbContext db)
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
        //http://localhost:8090/article/ renvoie la liste des articles
        //http://localhost:8090/article/1/ ne renvoie que le article 
        //TestOne
        [HttpGet("/article")]
        public ActionResult<Article> get()
        {
            var article = db.Articles.Include(a => a.Realisateur).ToList();
            return Ok(article) ;
        }
        
        //testSecond
        [HttpGet("/article/{id}")]
        public ActionResult<Article> get_id(int id)
        {
            var article = db.Articles.Where(a => a.Id == id).Include(a => a.Realisateur).FirstOrDefault();

            if(article is not null)
                return Ok(article) ;
            
            return NotFound( new { message = "article non trouve", article_id = id } ) ; 
        }
        //testThird
        //POST : création
        [Authorize]
        [HttpPost("/article")]
        public ActionResult<Article> create([FromBody] Article UserEntry)
        {
            User userdb = get_user() ;
   
            if(userdb is null || ((userdb.Role != UserRole.ROLE_ADMIN) && (userdb.Role != UserRole.ROLE_REDACTEUR)) )
                return Unauthorized(new { message = "Vous n'etes pas autorise !" } ) ; 

            var article = new Article {
                Realisateur = UserEntry.Realisateur,
                Titre = UserEntry.Titre,
                TypeArticle = UserEntry.TypeArticle,
                Duree = UserEntry.Duree,
                Synopsis = UserEntry.Synopsis,
                Categorie = UserEntry.Categorie,
                DateSortie = UserEntry.DateSortie, 
                CreationDate = DateTime.Now 
            };
            
            var article_existant = db.Articles.ToList();
        
            if(article_existant.Contains(article))
            {
                var ex = new Exception("article deja en base de donnee") ;
                return StatusCode(409, ex );
            }

            try 
            {
                db.Articles.Add(article);
                var res = db.SaveChanges();

                if(res > 0)
                    return StatusCode(201,article) ;

                else 
                    throw new Exception("Erreur avec la base de donnees") ;   

            }
            
            catch(Exception e){
                return StatusCode(400, e) ;       
            }    
        }
        //testFourth
        //PUT : Modification d'un article
        //Retourne l'entité “article” mise à jour
        [Authorize]
        [HttpPut("/article/{id}")]
        public ActionResult<Article> modify(int id, [FromBody] Article UserEntry)
        {
            User userdb = get_user() ;
   
            if(userdb is null || ((userdb.Role != UserRole.ROLE_ADMIN) && (userdb.Role != UserRole.ROLE_REDACTEUR)) )
                return Unauthorized(new { message = "Vous n'avez pas l'autorisation !" } ) ; 
            
            Article articledb = db.Articles.Where(a => a.Id == id).FirstOrDefault() ;

            if(articledb is null)
                return NotFound(new { id = id }) ;
                
            articledb.Realisateur = (UserEntry.Realisateur is null) ? articledb.Realisateur : UserEntry.Realisateur ;
            articledb.Titre = (UserEntry.Titre is null) ? articledb.Titre : UserEntry.Titre ;
            articledb.TypeArticle = (UserEntry.TypeArticle != articledb.TypeArticle) ? UserEntry.TypeArticle : articledb.TypeArticle ;
            articledb.Duree = (UserEntry.Duree != articledb.Duree) ? articledb.Duree : UserEntry.Duree ;
            articledb.Categorie = (UserEntry.Categorie is null) ? articledb.Categorie : UserEntry.Categorie ;
            articledb.DateSortie = (UserEntry.DateSortie is null) ? articledb.DateSortie : UserEntry.DateSortie ;    
            articledb.UpdatedDate = DateTime.Now ;
        
            try
            {
                db.Articles.Update(articledb) ;
                var res = db.SaveChanges();

                if(res > 0)
                    return Ok(articledb) ;
                    
                throw new DbUpdateException("Erreur mis a jour article") ;
            }
                
            catch (Exception e){
                return StatusCode(400, e) ;
            }
        }
        
        //DELETE : Suppression d'une adresse
        //Retourne un JSON avec une clé “success” à true ou false
        [HttpDelete("/article/{id}")]
        public ActionResult<Article> delete(int id)
        {
            User userdb = get_user() ;
            
            if(userdb is null || ((userdb.Role != UserRole.ROLE_ADMIN) && (userdb.Role != UserRole.ROLE_REDACTEUR)) )
                return Unauthorized(new { message = "Vous n'etes pas autorise !" } ) ;
                        
            var verif = false ;

            Article articledb = db.Articles.Where(a => a.Id == id).FirstOrDefault() ;
            
            if(articledb is null)
                return NotFound(new { success = verif }) ;
            
            db.Articles.Remove(articledb);            
            var r = db.SaveChanges();
                
            if(r > 0)
                verif = true ;
            
            return Ok(new { success = verif }) ;
            
        }
    }
}

