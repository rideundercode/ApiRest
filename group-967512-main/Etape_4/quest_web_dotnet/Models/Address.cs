using System;
using System.ComponentModel.DataAnnotations.Schema ;

namespace quest_web.Models
{
    public class Address
    {
        public Address() 
        {
        } 

        public int Id { get; set; }
        
        [Column("user_id")]
        
        #nullable enable
        public virtual User? UserId { get; set; }
        #nullable disable 

        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        
        public DateTime? CreationDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        
        public override string ToString()
        {
            return String.Format("{0} \n{1}, {2} \n{3}  ", Street , City  , PostalCode , Country) ;
        }    
        
        public override bool Equals(object obj)
        {
            return Equals(obj as Address);
        }

        public bool Equals(Address obj)
        {
            return obj != null &&
                   //Id == obj.Id &&
                   UserId == obj.UserId &&
                   (String.Compare(Street, obj.Street, StringComparison.InvariantCultureIgnoreCase) == 0) &&
                   (String.Compare(City, obj.City, StringComparison.InvariantCultureIgnoreCase) == 0) && 
                   (String.Compare(Country, obj.Country, StringComparison.InvariantCultureIgnoreCase) == 0) &&
                   (String.Compare(PostalCode , obj.PostalCode, StringComparison.InvariantCultureIgnoreCase) == 0) ; 
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Street , City  , PostalCode , Country );
        }
    }  
  
}  

