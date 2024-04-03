using System;
using System.Collections.Generic ;
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
            //Role = UserRole.ROLE_USER ;
        } 

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public UserRole? Role { get ; set ; }
        public DateTime? CreationDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
                
        public UserDetails GetUserDetails()
        {
            return new UserDetails{ Username = this.Username, Role = this.Role } ;
        }
        
        public override string ToString()
        {
            return "Username: " + Username + " Role : " + Role.ToString() ;
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

