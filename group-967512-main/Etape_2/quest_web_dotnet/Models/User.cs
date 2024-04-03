using System;
using System.ComponentModel.DataAnnotations ;

namespace quest_web.Models
{
    //[JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
    public enum UserRole
    {
        ROLE_USER,
        ROLE_ADMIN
    }

    public class User
    {
        public User() 
        {
            Role = UserRole.ROLE_USER ;
        } 

        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public UserRole Role { get ; set ; }
        public DateTime? CreationDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
                
        public override string ToString()
        {
            return "Username: " + Username + "\nRole : " + Role.ToString() ;
        }    
        
        public override bool Equals(object obj)
        {
            return Equals(obj as User);
        }

        public bool Equals(User obj)
        {
            return obj != null &&
                   //Id == obj.Id &&
                   (String.Compare(Username, obj.Username, StringComparison.InvariantCultureIgnoreCase) == 0);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Username, Password);
        }
    }  
  
}  

