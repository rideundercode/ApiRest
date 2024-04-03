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
       
    //on peut commenter une seule fois un article
    //on peut recuperer les comments 
    //on peut recuperer les comments d'un article
    //on peut recuperer les comments d'un utilisateur (*)
    //on peut supprimer et modifier les comments(seulement ceux qui l'ont creer)
    [ApiController]
    [Route("[controller]")]

    public class CommentController : ControllerBase
    {
     	private readonly APIDbContext db;

        public CommentController(APIDbContext db)
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
        [HttpGet("/comment")]
        public ActionResult<Comment> get()
        {
            var comments = db.Comments.ToList();
            return Ok(comments) ;
        }
        
        [HttpGet("/comment/{article}")]
        public ActionResult<Comment> get_id(Article id)
        {
            var comments = db.Comments.Where(a => a.Article == id).ToList();

            if(comments is not null)
                return Ok(comments) ;
            
            return NotFound( new { message = "Pas de commentaire", article_id = id } ) ; 
        }
        
        [Authorize]
        [HttpGet("/test-comment/{article}")]
        public bool already_comment(Article ArticleId)
        {
            var userdb = get_user() ;

            var test = db.Comments.Where(a => a.Article == ArticleId && a.User == userdb ).FirstOrDefault();

            if(test is not null)
                return true  ;
            
            return false ; 
        }
        
        //POST : création 
        //on peut commenter qu'une fois
        [Authorize]
        [HttpPost("/comment")]
        public ActionResult<Comment> create([FromBody] Comment UserEntry)
        { 
            var userdb = get_user() ;

            if(userdb is null ) //bloquer
                return Unauthorized(new { message = "Vous n'etes pas autorise !" } ) ; 

            var comment = new Comment {
                User = userdb ,
                ArticleId = UserEntry.ArticleId,
                Contenu = UserEntry.Contenu ,
                CreationDate = DateTime.Now  
            };
            
            var verif = already_comment(UserEntry.Article) ;

            if(verif)
                return StatusCode(409, new {message = "Vous avez deja commente" } ) ;

            try 
            {
                db.Comments.Add(comment);
                var res = db.SaveChanges();

                if(res > 0)
                    return StatusCode(201,comment) ;

                else 
                    throw new Exception("Erreur avec la base de donnees") ;   
            }
            
            catch(Exception e){
                return StatusCode(400, e) ;       
            }    
        }
    
        //PUT : Modification d'un article
        //Retourne l'entité “article” mise à jour
        [Authorize]
        [HttpPut("/comment/{id}")]
        public ActionResult<Comment> modify(int id, [FromBody] Comment UserEntry)
        {
            var userdb = get_user() ;
            
            if(userdb is null || ((userdb.Role != UserRole.ROLE_ADMIN) && (userdb.Role != UserRole.ROLE_REDACTEUR)) )
                return Unauthorized(new { message = "Vous n'avez pas l'autorisation !" } ) ; 
            
            Comment commentdb = db.Comments.Where(a => a.Id == id).FirstOrDefault() ;

            if(commentdb is null)
                return NotFound(new { id = id }) ;
                
           commentdb.Contenu = UserEntry.Contenu is null ? commentdb.Contenu : UserEntry.Contenu ; 
           commentdb.UpdatedDate = DateTime.Now ;
        
            try
            {
                db.Comments.Update(commentdb) ;
                var res = db.SaveChanges();

                if(res > 0)
                    return Ok(commentdb) ;
                    
                throw new DbUpdateException("Erreur mis a jour article") ;
            }
                
            catch (Exception e){
                return StatusCode(400, e) ;
            }
        }
        
        //DELETE : Suppression d'une adresse
        //Retourne un JSON avec une clé “success” à true ou false
        [HttpDelete("/comment/{id}")]
        public ActionResult<Comment> delete(int id)
        {
            var userdb = get_user() ;

            //a modifier... si on trouve le bon utilisateur et que c'est son comment ou si on est admin ou modo, on accepte 
            if(userdb is null || ((userdb.Role != UserRole.ROLE_ADMIN) && (userdb.Role != UserRole.ROLE_MODO)) )
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

