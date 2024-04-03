using System;
using System.Collections.Generic ;
using System.ComponentModel.DataAnnotations.Schema ;

namespace quest_web.Models
{

    public class Commande
    {
        public Commande() 
        {
        } 

        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("article_id")]
        public int ArticleId { get; set; }
        public DateTime? CreationDate { get; set; }
        
        public virtual User User { get; set; }
        public virtual Article Article { get; set; }
        
        public override string ToString()
        {
            return "Client : " + User.Username + "\nArticle : " + Article.Titre ;
        }    
        
        public override bool Equals(object obj)
        {
            return Equals(obj as Commande);
        }

        public bool Equals(Commande obj)
        {
            return obj != null &&
                   //Id == obj.Id &&
                   UserId == obj.UserId &&
                   ArticleId == obj.ArticleId ;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(UserId, ArticleId);
        }
    }  
  
}  

