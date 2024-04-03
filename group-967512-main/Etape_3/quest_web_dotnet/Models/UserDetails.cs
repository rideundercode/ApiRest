using System;
using System.ComponentModel.DataAnnotations ;

namespace quest_web.Models
{
    public class UserDetails
    {
        public UserDetails() 
        {
        } 

        public UserDetails(string name, UserRole role) 
        {
            Username = name ;
            this.Role = role ;
        } 

        [Required(AllowEmptyStrings = true)] 
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Username { get; set; }
        
        //public string role
        //{
        //    get { return Role.ToString(); }
        //    set { Role = value.ParseEnum<UserRole>(); }
        //}

        public UserRole Role {get; set;}   
        
        public override string ToString()
        {
            return "Username: " + Username + " Role : " + Role.ToString() ;
        }    
        
        public override bool Equals(object obj)
        {
            return Equals(obj as UserDetails);
        }

        public bool Equals(UserDetails obj)
        {
            return obj != null &&
                   Username == obj.Username;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Username, Role);
        }
    }  
  
}  

