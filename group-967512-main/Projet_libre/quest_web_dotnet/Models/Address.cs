using System;
using System.ComponentModel.DataAnnotations.Schema ;

namespace quest_web.Models
{
    public class Address
    {
        public Address() 
        {
        } 

        [Column("user_id")]
        [ForeignKey("User")]
        public int Id { get; set; }
    
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        
        public DateTime? CreationDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        
        public virtual User User { get; set; }
        
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
                   User == obj.User &&
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

