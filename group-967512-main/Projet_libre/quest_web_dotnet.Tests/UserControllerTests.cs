using System;
using Xunit;
using quest_web.Models ;
using System.Net.Http ;
using System.Net ;
using System.Net.Http.Headers ;
using quest_web.Utils ;

namespace quest_web_dotnet.Tests
{
    public class UsercontrollerTest
    {
        private static readonly Uri localhost = new Uri("http://localhost:8090") ; 
        private HttpClient _client = new HttpClient { BaseAddress = localhost } ;
        
        [Fact]
        public void UserTestNoTokenBearer()
        {
            _client.DefaultRequestHeaders.Authorization = null ;
            
            var result = _client.GetAsync("/user").Result ; 

            //HttpStatusCode.Unauthorized = 401
            Assert.Equal(HttpStatusCode.Unauthorized , result.StatusCode )  ;            
        }
        
        [Fact]
        public void UserTestTokenBearer()
        {
            var userco = new UserDetails{Username = "MyUserName", Role = UserRole.ROLE_USER } ;
            var tokenclass = new JwtTokenUtil() ;

            var token = tokenclass.GenerateToken(userco) ;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token) ;
                
            var result =  _client.GetAsync("/user").Result ;  
            var StatusCodeTest = result.StatusCode ; 
            
            //HttpStatusCode.Ok = 200
            Assert.Equal(HttpStatusCode.OK, StatusCodeTest)  ; 
        }
        

        [Fact]
        public void UserTestDeleteUser()
        {            
            //utilisateur qui existe bien en base
            var userco = new UserDetails{Username = "MyUserName", Role = UserRole.ROLE_USER } ;
            
            //on recupere 
            var tokenclass = new JwtTokenUtil() ;
            var token = tokenclass.GenerateToken(userco) ;
            
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token) ;
                
            //en admettant que son id n'est pas 1 et qu'il y a des utilisateurs dans la table  
            var result =  _client.DeleteAsync("/user/1").Result ; 
    
            var StatusCodeTest = result.StatusCode ;
            
            //HttpStatusCode.Forbidden = 403
            Assert.Equal(HttpStatusCode.Forbidden, StatusCodeTest)  ; 

        }

        [Fact]
        public void UserTestDeleteAdmin()
        {
            var userco = new UserDetails{Username = "MyUserNameAdmin", Role = UserRole.ROLE_ADMIN } ;
            var tokenclass = new JwtTokenUtil() ;

            var token = tokenclass.GenerateToken(userco) ;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token) ;
                
            //en admettant qu'il y a des utilisateurs dans la table  et que "MyUserNameAdmin" est admin
            var result =  _client.DeleteAsync("/user/1").Result ; 
    
            var StatusCodeTest = result.StatusCode ; // montest.StatusCode ;
            
            Assert.Equal(HttpStatusCode.OK, StatusCodeTest) ;
        }
    }
}
