using System;
using System.Collections.Generic ;
using System.ComponentModel.DataAnnotations.Schema;



namespace quest_web.Models
{
    public enum TypeArticle
    {
        FILM,
        LIVRE
    }

    public class Article
    {
        
        public Article() 
        {
        } 
        public int Id { get; set; }

        [Column("realisateur_id")]
        [ForeignKey("realisateur_id")]
        public int RealisateurId {get; set;}
       
        public string Titre { get; set; }
        public double Duree {get; set; } //nb de pages
        public string Categorie {get; set;}
        public string Synopsis {get; set;}
        public TypeArticle TypeArticle {get; set;}
        public DateTime? DateSortie { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        
        public virtual Realisateur Realisateur { get; set; } //auteur
        public virtual ICollection<Commande> Commande { get; set; }
        public virtual ICollection<Comment> Comment { get; set; }

        public override string ToString()
        {
            return "Titre: " + Titre + " sortie le : " + DateSortie ;
        }    
        
        public override bool Equals(object obj)
        {
            return Equals(obj as Article);
        }

        public bool Equals(Article obj)
        {
            return obj != null &&
                   //Id == obj.Id &&
                   Realisateur == obj.Realisateur &&
                   DateSortie == obj.DateSortie &&
                   Duree == obj.Duree &&
                   (String.Compare(Titre, obj.Titre, StringComparison.InvariantCultureIgnoreCase) == 0) &&
                   (String.Compare(Categorie, obj.Categorie, StringComparison.InvariantCultureIgnoreCase) == 0);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Titre, Realisateur, DateSortie, Duree, Categorie); //TODO
        }
    }  
  
}  

