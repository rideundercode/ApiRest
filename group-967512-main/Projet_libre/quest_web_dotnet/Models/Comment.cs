using System;
using System.Collections.Generic ;
using System.ComponentModel.DataAnnotations.Schema ;



namespace quest_web.Models
{

    public class Comment
    {
        public Comment() 
        {
        } 

        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("article_id")]
        public int ArticleId { get; set; }
        public string Contenu { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        
        public virtual User User { get; set; }
        public virtual Article Article { get; set; }

        public override string ToString()
        {
            return "Auther : " + User.Username + "\nContenu : " + Contenu ;
        }    
        
        public override bool Equals(object obj)
        {
            return Equals(obj as Comment);
        }

        public bool Equals(Comment obj)
        {
            return obj != null &&
                   //Id == obj.Id &&
                   UserId == obj.UserId &&
                   ArticleId == obj.ArticleId &&
                   (String.Compare(Contenu, obj.Contenu, StringComparison.InvariantCultureIgnoreCase) == 0);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(UserId, ArticleId, Contenu);
        }
    }  
  
}  

