using System;
using System.Collections.Generic ;
using System.ComponentModel.DataAnnotations ;

namespace quest_web.Models
{
     
    public class Realisateur
    {
        public Realisateur() 
        {
        } 
        public int Id { get; set; }
        public string NomComplet { get; set; }
        public string Nationalite { get; set; }
        public string Photo { get; set; }
        public string Biographie { get; set; }
        public DateTime? Naissance { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual ICollection<Article> Article { get; set; }
                
        public override string ToString()
        {
            return "Nom : " + NomComplet + "\nBiographie : " + Biographie ;
        }    
        
        public override bool Equals(object obj)
        {
            return Equals(obj as Realisateur);
        }

        public bool Equals(Realisateur obj)
        {
            return obj != null &&
                   //Id == obj.Id &&
                   (String.Compare(NomComplet, obj.NomComplet, StringComparison.InvariantCultureIgnoreCase) == 0) &&
                   (String.Compare(Nationalite, obj.Nationalite, StringComparison.InvariantCultureIgnoreCase) == 0) &&
                   Naissance == obj.Naissance;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NomComplet, Biographie);
        }
    }  
  
}  

