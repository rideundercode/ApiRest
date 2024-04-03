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
            username = name ;
            this.role = role ;
        } 

        [Required(AllowEmptyStrings = true)] 
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string username { get; set; }
        
        //public string role
        //{
        //    get { return Role.ToString(); }
        //    set { Role = value.ParseEnum<UserRole>(); }
        //}

        public UserRole role {get; set;}   
        
        public override string ToString()
        {
            return "Username: " + username + "\nRole : " + role.ToString() ;
        }    
        
        public override bool Equals(object obj)
        {
            return Equals(obj as UserDetails);
        }

        public bool Equals(UserDetails obj)
        {
            return obj != null &&
                   username == obj.username;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(username, role);
        }
    }  
  
}  

